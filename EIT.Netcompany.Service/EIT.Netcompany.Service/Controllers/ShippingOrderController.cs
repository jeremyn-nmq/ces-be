using EIT.Netcompany.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EIT.Netcompany.Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShippingOrderController : ApiController
    {
        ShippingOrder shippingOrderResults = new ShippingOrder
        {
            Nr = "P0123",
            StartDate = DateTime.UtcNow,
            Status = "New"
        };

        public string GetShippingOrder(string shippingOrder)
        {
            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(shippingOrderResults);
            return jsonResult;
        }
    }
}
