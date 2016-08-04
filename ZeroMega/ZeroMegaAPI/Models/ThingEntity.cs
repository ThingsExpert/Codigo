using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroMegaAPI.Models
{
    public class ThingEntity : TableEntity
    {
        // Your entity type must expose a parameter-less constructor
        public ThingEntity() { }

        // Define the PK and RK
        public ThingEntity(string id_thing, string id_event)
        {
            this.PartitionKey = id_thing;
            this.RowKey = id_event;
        }

        //public DateTime TimeStamp { get; set; }

        public int account { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
    }
}