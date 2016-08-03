using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ZeroMegaAPI.Models;
using ZeroMegaAPI.Repositories;

namespace ZeroMegaAPI.Controllers
{
    public class ThingsController : Controller
    {
        private string _account = string.Empty;
        private PositionRepository _repository = new PositionRepository();

        public ThingsController()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            _account = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
        }


        // GET: Things/Position
        public IEnumerable<ThingPosition> Position()
        {
            return null;
            //return _repository.GetAllThingsPositions(_account);
        }

        // GET: Things/Position/1293801283091280938
        public IEnumerable<ThingPosition> Position(string thingId)
        {
            return _repository.GetThingPositions(_account, thingId);
        }

        // GET: Things/Position/1293801283091280938/344131B1-7B61-43B4-BC7B-19E97C002EC8
        public ThingPosition Position(string thingId, Guid eventId)
        {
            return _repository.GetThingPosition(_account, thingId, eventId);
        }

        // GET: Things/Position/1293801283091280938&lowerLimit=11/08/2016
        public IEnumerable<ThingPosition> Position(string thingId, DateTime lowerLimit)
        {
            return _repository.GetThingPositions(_account, thingId, lowerLimit);
        }

        // GET: Things/Position/1293801283091280938&lowerLimit=11/08/2016&upperLimit=14/08/2016
        public IEnumerable<ThingPosition> Position(string thingId, DateTime lowerLimit, DateTime upperLimit)
        {
            return _repository.GetThingPositions(_account, thingId, lowerLimit, upperLimit);
        }
    }
}