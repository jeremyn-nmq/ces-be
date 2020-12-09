using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIT.Netcompany.Service.Models
{
    public class ShippingOrder
    {
        public string Nr { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
    }
}