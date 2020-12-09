using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIT.Netcompany.Service.Models
{
    public class RouteData
    {
        public string Point1 { get; set; }
        public string Point2 { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
        public List<RouteData> NextRoutes { get; set; }
    }

    public class RouteHistory
    { 
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
    }

    public class CloseRoute
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
    }

    public class DBRoute
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Segmets { get; set; }
    }
}