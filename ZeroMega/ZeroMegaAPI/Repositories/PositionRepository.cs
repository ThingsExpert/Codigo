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


        public async Task<IEnumerable<ThingPosition>> GetAllThingsPositions(int accountId)
        {
            //TableOperation retrieveOperation = TableOperation.Retrieve<ThingEntity>("8000123456789012345", "3E136D80-7096-40C7-8C51-B1C203F97D74");
            //TableResult result = _table.ExecuteAsync(retrieveOperation).Result;
            //ThingEntity thing = result.Result as ThingEntity;




            var thingsPositions = new List<ThingPosition>();

            //TableQuery<ThingEntity> partitionScanQuery = new TableQuery<ThingEntity>().Where
            //    (TableQuery.GenerateFilterCondition("account", QueryComparisons.Equal, accountId));

            TableQuery<ThingEntity> partitionScanQuery = new TableQuery<ThingEntity>().Where
    (TableQuery.GenerateFilterCondition("account", QueryComparisons.Equal, accountId.ToString()));

            TableContinuationToken token = null;
            // Page through the results

            try
            {
                do
                {
                    TableQuerySegment<ThingEntity> segment = await _table.ExecuteQuerySegmentedAsync(new TableQuery<ThingEntity>()
                    {
                        FilterString = "account eq 2"
                    }, token);

                    //TableQuerySegment<ThingEntity> segment = await _table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                    token = segment.ContinuationToken;
                    var result = from thing
                                 in segment
                                 select new ThingPosition()
                                 {
                                     ThingID = thing.PartitionKey,
                                     EventID = Guid.Parse(thing.RowKey),
                                     Latitude = thing.latitude,
                                     Longitude = thing.longitude,
                                     TimeStamp = DateTime.Parse(thing.Timestamp.ToString())
                                 };
                    thingsPositions.AddRange(result);
                }
                while (token != null);
            }
            catch (Exception ex)
            {

                throw ex;
            }

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