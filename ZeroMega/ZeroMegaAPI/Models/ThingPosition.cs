using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroMegaAPI.Models
{
    public class ThingPosition
    {
        public string ThingID { get; set; }

        public Guid EventID { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}