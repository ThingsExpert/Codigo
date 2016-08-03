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
        public ThingEntity(string thingId, string eventId)
        {
            this.PartitionKey = thingId;
            this.RowKey = eventId;
        }

        //public DateTime TimeStamp { get; set; }

        public string Account { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }
    }
}