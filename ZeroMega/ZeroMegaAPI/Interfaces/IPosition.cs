using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMegaAPI.Models;

namespace ZeroMegaAPI.Interfaces
{
    interface IPosition
    {
        ThingPosition GetThingPosition(string thingId, string accountId, Guid eventId);

        IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId);

        IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId, DateTime lowerLimit);

        IEnumerable<ThingPosition> GetThingPositions(string accountId, string thingId, DateTime lowerLimit, DateTime upperLimit);

        IEnumerable<ThingPosition> GetAllThingsPositions(string accountId);
    }
}