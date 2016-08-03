using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ZeroMegaAPI.Interfaces;
using ZeroMegaAPI.Models;

namespace ZeroMegaAPI.Repositories
{
    //TODO: Implements IPosition
    public class PositionRepository
    {
        CloudTable _table = null;

        public PositionRepository()
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            _table = tableClient.GetTableReference("xDRTable");

            //try
            //{
            //    if (await _table.CreateIfNotExistsAsync())
            //    {
            //    }
            //    else
            //    {
            //    }
            //}
        }


        public async Task<IEnumerable<ThingPosition>> GetAllThingsPositions(string accountId)
        {
            var thingsPositions = new List<ThingPosition>();

            TableQuery<ThingEntity> partitionScanQuery = new TableQuery<ThingEntity>().Where
                (TableQuery.GenerateFilterCondition("account", QueryComparisons.Equal, accountId));

            TableContinuationToken token = null;
            // Page through the results
            do
            {
                TableQuerySegment<ThingEntity> segment = await _table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                token = segment.ContinuationToken;
                foreach (ThingEntity entity in segment)
                {
                    Console.WriteLine("Customer: {0},{1}\t{2}\t{3}\t{4}", entity.PartitionKey, entity.RowKey, entity.Timestamp, entity.Latitude, entity.Longitude);
                }
            }
            while (token != null);

            return thingsPositions;
        }

        public ThingPosition GetThingPosition(string thingId, string accountId, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId, DateTime lowerLimit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId, DateTime lowerLimit, DateTime upperLimit)
        {
            throw new NotImplementedException();
        }
    }
}