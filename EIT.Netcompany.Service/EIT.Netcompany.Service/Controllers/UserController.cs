using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using EIT.Netcompany.Service.Common;

namespace EIT.Netcompany.Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        /// <summary>Upsert the user data.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        public string UpsertUserData(string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 20 e.Username as Username, e.Password as Password ");
            sb.Append("FROM [employee] e ");
            sb.Append($"WHERE e.Username ='{username}'");
            String sql = sb.ToString();
            string resultStr = CommonService.ExecuteFindSqlRequest(sql);

            if (string.IsNullOrEmpty(resultStr))
            {
                // Create User
                StringBuilder createUserSb = new StringBuilder();
                sb.Append("INSERT INTO ememployee (Username, Password)");
                sb.Append($"VALUES ({username}, {password})");
                String createUserSql = createUserSb.ToString();
                resultStr = CommonService.ExecuteUpdateSertSqlRequest(createUserSql);
            }
            else if (string.IsNullOrEmpty(resultStr) && resultStr.IndexOf("Not found.") > 0)
            {
                // Update User
                StringBuilder updateUserSb = new StringBuilder();
                sb.Append("INSERT INTO ememployee (Username, Password)");
                sb.Append($"VALUES ({username}, {password})");
                String UpdateUserSql = updateUserSb.ToString();
                resultStr = CommonService.ExecuteUpdateSertSqlRequest(UpdateUserSql);
            }

            return resultStr;
        }

        [HttpGet]
        public string CheckLogin(string username, string password, string isLogin)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 20 e.Username as Username, e.Password as Password, e.IsAdmin as IsAdmin ");
            sb.Append("FROM [employee] e ");
            sb.Append($"WHERE e.Username ='{username}' AND e.Password = '{password}'");
            String sql = sb.ToString();
            string resultStr = CommonService.ExecuteGetUserRequest(sql);

            if (string.IsNullOrEmpty(resultStr) || resultStr == "Not found.")
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return resultStr;


        }
    }
}
