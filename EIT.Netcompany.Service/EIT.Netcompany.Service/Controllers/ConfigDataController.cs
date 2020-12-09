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
    public class ConfigDataController : ApiController
    {
        [HttpGet]
        public string GetConfigData()
        {
            ConfigData configData = CommonService.GetAllConfigData();
            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(configData);
            return jsonResult;
        }

        [HttpPost]
        public string UpdateConfigData(int maxWeight, string pricePerSegment114, string pricePerSegment510 )
        {
            try
            {
                string isError = string.Empty;
                string[] pricePerSegment114Arr = pricePerSegment114.Split(',');
                string[] pricePerSegment510Arr = pricePerSegment510.Split(',');

                ConfigData config = new ConfigData()
                {
                    MaxWeight = maxWeight,
                    PriceNovApr10 = Double.Parse(pricePerSegment114Arr[0]),
                    PriceNovApr1050 = Double.Parse(pricePerSegment114Arr[1]),
                    PriceNovApr50 = Double.Parse(pricePerSegment114Arr[2]),
                    PriceMayOct10 = Double.Parse(pricePerSegment510Arr[0]),
                    PriceMayOct1050 = Double.Parse(pricePerSegment510Arr[1]),
                    PriceMayOct50 = Double.Parse(pricePerSegment510Arr[2]),
                };

                // Update MaxWeight
                SqlConnectionStringBuilder builder = CommonService.GetSqlConnectionBuilder();
                StringBuilder sbUpdateMaxWeight = new StringBuilder();
                sbUpdateMaxWeight.Append("UPDATE Prices ");
                sbUpdateMaxWeight.Append($"SET Value = {config.MaxWeight} ");
                sbUpdateMaxWeight.Append("WHERE Code = 'MaxWeight' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdateMaxWeight.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceNovApr10
                StringBuilder sbUpdatePriceNovApr10 = new StringBuilder();
                sbUpdatePriceNovApr10.Append("UPDATE Prices ");
                sbUpdatePriceNovApr10.Append($"SET Value = {config.PriceNovApr10} ");
                sbUpdatePriceNovApr10.Append("WHERE Code = 'Nov-Apr-10' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceNovApr10.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceNovApr1050
                StringBuilder sbUpdatePriceNovApr1050 = new StringBuilder();
                sbUpdatePriceNovApr1050.Append("UPDATE Prices ");
                sbUpdatePriceNovApr1050.Append($"SET Value = {config.PriceNovApr1050} ");
                sbUpdatePriceNovApr1050.Append("WHERE Code = 'Nov-Apr-10-50' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceNovApr1050.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceNovApr50
                StringBuilder sbUpdatePriceNovApr50 = new StringBuilder();
                sbUpdatePriceNovApr50.Append("UPDATE Prices ");
                sbUpdatePriceNovApr50.Append($"SET Value = {config.PriceNovApr50} ");
                sbUpdatePriceNovApr50.Append("WHERE Code = 'Nov-Apr-50' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceNovApr50.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceMayOct10
                StringBuilder sbUpdatePriceMayOct10 = new StringBuilder();
                sbUpdatePriceMayOct10.Append("UPDATE Prices ");
                sbUpdatePriceMayOct10.Append($"SET Value = {config.PriceMayOct10} ");
                sbUpdatePriceMayOct10.Append("WHERE Code = 'May-Oct-10' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceMayOct10.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceMayOct1050
                StringBuilder sbUpdatePriceMayOct1050 = new StringBuilder();
                sbUpdatePriceMayOct1050.Append("UPDATE Prices ");
                sbUpdatePriceMayOct1050.Append($"SET Value = {config.PriceMayOct1050} ");
                sbUpdatePriceMayOct1050.Append("WHERE Code = 'May-Oct-10-50' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceMayOct1050.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                // Update PriceMayOct50
                StringBuilder sbUpdatePriceMayOct50 = new StringBuilder();
                sbUpdatePriceMayOct50.Append("UPDATE Prices ");
                sbUpdatePriceMayOct50.Append($"SET Value = {config.PriceMayOct50} ");
                sbUpdatePriceMayOct50.Append("WHERE Code = 'May-Oct-50' ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = sbUpdatePriceMayOct50.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows <= 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }
                    }
                }

                return "Success";
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
