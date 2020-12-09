using EIT.Netcompany.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace EIT.Netcompany.Service.Common
{
    public class CommonService
    {
        public static SqlConnectionStringBuilder GetSqlConnectionBuilder()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "dbs-eitvn.database.windows.net";
            builder.UserID = "admin-eitvn";
            builder.Password = "Eastindia4thewin";
            builder.InitialCatalog = "db-eitvn-27072020";
            return builder;
        }

        public static string ExecuteFindSqlRequest(string requestQuery)
        {
            string resultStr = String.Empty;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "dbs-eitvn.database.windows.net";
                builder.UserID = "admin-eitvn";
                builder.Password = "Eastindia4thewin";
                builder.InitialCatalog = "db-eitvn-27072020";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = requestQuery;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultStr += reader.GetString(0) + " " + reader.GetString(1);
                            }

                            if (String.IsNullOrEmpty(resultStr))
                            {
                                return "Not found.";
                            }

                            return resultStr;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        public static string ExecuteGetUserRequest(string requestQuery)
        {
            string resultStr = String.Empty;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "dbs-eitvn.database.windows.net";
                builder.UserID = "admin-eitvn";
                builder.Password = "Eastindia4thewin";
                builder.InitialCatalog = "db-eitvn-27072020";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = requestQuery;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetBoolean(2))
                                {
                                    return "Admin";
                                }
                                else
                                {
                                    return "Employee";
                                }
                            }

                            return "Not found.";

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        public static List<ParcelType> GetAllParcelType()
        {
            List<ParcelType> lstParcelType = new List<ParcelType>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ef.CodeItem,ef.Charge ");
            sb.Append("FROM extrafees ef ");
            try
            {
                SqlConnectionStringBuilder builder = GetSqlConnectionBuilder();
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
                                lstParcelType.Add(new ParcelType { ParcelCode = reader.GetString(0), Charge = reader.GetDouble(1) });
                            }

                            if (lstParcelType.Count < 1)
                            {
                                throw new HttpResponseException(HttpStatusCode.InternalServerError);
                            }
                            else {
                                return lstParcelType;
                            }

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return lstParcelType;
            }
        }

        public static ConfigData GetAllConfigData()
        {
            try
            {
                ConfigData listAllConfigData = new ConfigData();
                SqlConnectionStringBuilder builder = CommonService.GetSqlConnectionBuilder();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT p.Code,p.Value ");
                sb.Append("FROM Prices p");
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
                                switch (reader.GetString(0))
                                {
                                    case "MaxWeight":
                                        listAllConfigData.MaxWeight = reader.GetDouble(1);
                                        break;
                                    case "Nov-Apr-10":
                                        listAllConfigData.PriceNovApr10 = reader.GetDouble(1);
                                        break;
                                    case "Nov-Apr-10-50":
                                        listAllConfigData.PriceNovApr1050 = reader.GetDouble(1);
                                        break;
                                    case "Nov-Apr-50":
                                        listAllConfigData.PriceNovApr50 = reader.GetDouble(1);
                                        break;
                                    case "May-Oct-10":
                                        listAllConfigData.PriceMayOct10 = reader.GetDouble(1);
                                        break;
                                    case "May-Oct-10-50":
                                        listAllConfigData.PriceMayOct1050 = reader.GetDouble(1);
                                        break;
                                    case "May-Oct-50":
                                        listAllConfigData.PriceMayOct50 = reader.GetDouble(1);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            return listAllConfigData;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public static string ExecuteUpdateSertSqlRequest(string requestQuery)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "dbs-eitvn.database.windows.net";
                builder.UserID = "admin-eitvn";
                builder.Password = "Eastindia4thewin";
                builder.InitialCatalog = "db-eitvn-27072020";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = requestQuery;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            return "Success";
                        }
                        else 
                        {
                            return "No data effected";
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                return e.ToString();
            }
        }
    }
}