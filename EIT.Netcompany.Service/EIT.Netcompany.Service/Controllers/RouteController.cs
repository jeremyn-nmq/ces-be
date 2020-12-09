using EIT.Netcompany.Service.Common;
using EIT.Netcompany.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EIT.Netcompany.Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RouteController : ApiController
    {
        public string GetRouteResult(string from, string to, double weight, string parcelType)
        {
            if (from == to)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var lstParcelType = CommonService.GetAllParcelType();
            var parcelTypeValue = lstParcelType.Where(x => x.ParcelCode.Trim() == parcelType).FirstOrDefault();
            if (parcelTypeValue == null || parcelTypeValue.Charge < 0)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            ConfigData configData = CommonService.GetAllConfigData();
            if (weight > configData.MaxWeight)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            int currentMonth = DateTime.Now.Month;
            double? basePrice = null;

            if (currentMonth >= 5 && currentMonth <= 10)
            {
                if (weight < 10)
                {
                    basePrice = configData.PriceMayOct10;
                }
                if (weight <= 50 && weight >= 10)
                {
                    basePrice = configData.PriceMayOct1050;
                }
                if (weight > 50)
                {
                    basePrice = configData.PriceMayOct50;
                }
            }
            else
            {
                if (weight < 10)
                {
                    basePrice = configData.PriceNovApr10;
                }
                if (weight <= 50 && weight >= 10)
                {
                    basePrice = configData.PriceNovApr1050;
                }
                if (weight > 50)
                {
                    basePrice = configData.PriceNovApr50;
                }
            }
            if (basePrice == null)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var chargeFee = parcelTypeValue.Charge;

            List<RouteResult> lstResult = GetRouteResultExternal(from, to, chargeFee, basePrice.Value);

            if (lstResult.Count < 1)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(lstResult.OrderBy(x => x.Price).FirstOrDefault());
            return jsonResult;
        }

        private List<RouteResult> GetRouteResultExternal(string from, string to, double chargeFee, double basePrice)
        {
            List<RouteResult> routeResults = new List<RouteResult>();
            List<RouteData> listStartData = new List<RouteData>();
            CityInfo fromCitiInfo = GetCityInfo(from);
            CityInfo finalCitiInfo = GetCityInfo(to);

            if (!fromCitiInfo.SeaSupported || !finalCitiInfo.SeaSupported)
            {
                return null;
            }

            if (fromCitiInfo.SeaSupported)
            {
                List<DBRoute> listDbRoute = GetDBRoutes(from);
                foreach (DBRoute dbRoute in listDbRoute)
                {
                    double finalPrice = dbRoute.Segmets * basePrice * (1+ chargeFee);
                    double finalDuration = dbRoute.Segmets * 12;
                    // TODO: Get price per segment and extra from DB

                    listStartData.Add(new RouteData { Point1 = from, Point2 = dbRoute.To, Price = finalPrice, Duration = finalDuration });
                }
            }

            List<Path> lstPath = CalculateAllRoute(listStartData, to, chargeFee, basePrice);

            foreach (Path path in lstPath)
            {
                routeResults.Add(new RouteResult { Price = path.totalNumber, Duration = path.totalSubNumber });
            }

            return routeResults;
        }

        private static CityInfo GetCityInfo(string cityCodeName)
        {
            try
            {
                ConfigData listAllConfigData = new ConfigData();
                SqlConnectionStringBuilder builder = CommonService.GetSqlConnectionBuilder();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT TOP 1 ct.Name,ct.CityCode,ct.SeaSupported,ct.LandSupported,ct.SkySupported ");
                sb.Append("FROM city ct ");
                sb.Append($"WHERE ct.CityCode = '{cityCodeName}'");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            CityInfo citiInfor = new CityInfo();
                            while (reader.Read())
                            {
                                citiInfor.CityLabel = reader.GetString(0);
                                citiInfor.CityCodeName = reader.GetString(1);
                                citiInfor.SeaSupported = reader.GetBoolean(2);
                                citiInfor.LandSupported = reader.GetBoolean(3);
                                citiInfor.SkySupported = reader.GetBoolean(4);
                            }
                            if (citiInfor == null)
                            {
                                throw new HttpResponseException(HttpStatusCode.InternalServerError);
                            }
                            else
                            {
                                return citiInfor;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        public static List<DBRoute> GetDBRoutes(string fromStr)
        {
            try
            {
                List<DBRoute> listDBRoutes = new List<DBRoute>();
                SqlConnectionStringBuilder builder = CommonService.GetSqlConnectionBuilder();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT r.Dest1, r.Dest2, r.Segment ");
                sb.Append("FROM routes r ");
                sb.Append($"WHERE r.Dest1 = '{fromStr}' OR r.Dest2 = '{fromStr}'");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetString(0) == fromStr)
                                {
                                    listDBRoutes.Add(new DBRoute() { From = reader.GetString(0), To = reader.GetString(1), Segmets = reader.GetInt32(2) });
                                }
                                if (reader.GetString(1) == fromStr)
                                {
                                    listDBRoutes.Add(new DBRoute() { From = reader.GetString(1), To = reader.GetString(0), Segmets = reader.GetInt32(2) });
                                }
                            }

                            if (listDBRoutes == null)
                            {
                                return null;
                            }
                            else
                            {
                                return listDBRoutes;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        class GFG : IComparer<Path>
        {
            public int Compare(Path x, Path y)
            {
                return x.totalNumber.CompareTo(y.totalNumber);
            }
        }
        class Path
        {
            public double totalNumber;
            public double totalSubNumber;
            public bool isCompleted;
            public RouteData NextRoute;
            public List<RouteHistory> log;



            public Path(RouteData NextRoute, double totalNumber, double toltalSubNumber)
            {
                this.totalNumber = totalNumber;
                this.totalSubNumber = toltalSubNumber;
                this.NextRoute = NextRoute;
                log = new List<RouteHistory>();
            }



            public List<Path> NextPath(string endPoint, double chargeFee, double basePrice)
            {
                if (NextRoute == null)
                {
                    return null;
                }
                if (log.Exists(id =>
                                    (id.From == NextRoute.Point1 && id.To == NextRoute.Point2 && id.Price == NextRoute.Price && id.Duration == NextRoute.Duration) ||
                                    (id.From == NextRoute.Point2 && id.To == NextRoute.Point1 && id.Price == NextRoute.Price && id.Duration == NextRoute.Duration)))
                {
                    return null;
                }

                totalNumber += NextRoute.Price*(1+ chargeFee);
                totalSubNumber += NextRoute.Duration;
                isCompleted = NextRoute.Point2 == endPoint || NextRoute.Point1 == endPoint;
                log.Add(new RouteHistory() { From = NextRoute.Point1, To = NextRoute.Point2, Duration = NextRoute.Duration, Price = NextRoute.Price });



                if (isCompleted)
                {
                    Path path = new Path(null, totalNumber, totalSubNumber);
                    path.log.AddRange(log);
                    path.isCompleted = true;
                    return new List<Path>() { path };
                }
                List<Path> nextPaths = new List<Path>() { };

                if (log.Count > 0 && log[log.Count - 1] != null)
                {
                    RouteHistory latestPoint = log[log.Count - 1];
                    CityInfo fromCitiInfo = GetCityInfo(latestPoint.To);
                    CityInfo finalCitiInfo = GetCityInfo(endPoint);
                    if (fromCitiInfo.SeaSupported && finalCitiInfo.SeaSupported)
                    {
                        List<DBRoute> listDbRoute = GetDBRoutes(latestPoint.To);
                        foreach (DBRoute dbRoute in listDbRoute)
                        {
                            double finalPrice = dbRoute.Segmets * basePrice;
                            double finalDuration = dbRoute.Segmets * 12;
                            // TODO: Get price per segment and extra from DB
                            Path path = new Path(new RouteData()
                            {
                                Point1 = dbRoute.From,
                                Point2 = dbRoute.To,
                                Duration = finalDuration,
                                Price = finalPrice ,
                            },
                                totalNumber, totalSubNumber);

                            path.log.AddRange(log);
                            nextPaths.Add(path);
                        }
                    }

                }
                return nextPaths;
            }


        }

        private List<Path> CalculateAllRoute(List<RouteData> startRoutes, string endPoint, double chargeFee, double basePrice)
        {
            List<Path> returnPaths = new List<Path>();
            List<Path> inputPaths = new List<Path>();
            foreach (RouteData routeData in startRoutes)
            {
                Path path = new Path(routeData, 0, 0);
                inputPaths.Add(path);
            }



            GFG gFG = new GFG();
            while (inputPaths.Exists(path => !path.isCompleted))
            {
                inputPaths.Sort(gFG);
                Path currentPath = inputPaths[0];
                inputPaths.Remove(currentPath);
                var result = currentPath.NextPath(endPoint, chargeFee, basePrice);
                if (result == null)
                {
                    inputPaths.Remove(currentPath);
                }
                else
                {
                    inputPaths.AddRange(result);
                }
                returnPaths.AddRange(inputPaths.FindAll(path => path.isCompleted));
                inputPaths.RemoveAll(path => path.isCompleted);
            }
            return returnPaths;
        }
    }
}
