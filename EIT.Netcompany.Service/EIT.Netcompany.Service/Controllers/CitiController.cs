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
    public class CitiController : ApiController
    {
        [HttpGet]
        public string GetAllUsers()
        {
            try
            {
                List<CityInfo> listAllCities = new List<CityInfo>();

                SqlConnectionStringBuilder builder = CommonService.GetSqlConnectionBuilder();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT Name, CityCode ");
                sb.Append("FROM city");
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
                                listAllCities.Add(new CityInfo() { CityLabel = reader.GetString(0), CityCodeName = reader.GetString(1) });
                            }

                            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(listAllCities);
                            return jsonResult;
                        }
                    }
                }
            }
            catch (Exception e) {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
