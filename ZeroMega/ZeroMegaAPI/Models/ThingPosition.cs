using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroMegaAPI.Models
{
    public class ThingPosition
    {
        public string IDThing { get; set; }

        public DateTime DateTimeEvent { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}