using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIT.Netcompany.Service.Models
{
    public class CityInfo
    {
        public string CityLabel { get; set; }
        public string CityCodeName { get; set; }
        public bool SeaSupported { get; set; }
        public bool LandSupported { get; set; }
        public bool SkySupported { get; set; }
    }
}