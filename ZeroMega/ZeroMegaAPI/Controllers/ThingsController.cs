using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ZeroMegaAPI.Controllers
{
    [Authorize]
    public class ThingsController : ApiController
    {
        private string _account = string.Empty;
        //private PositionRepository _repository = new PositionRepository();

        public ThingsController()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            _account = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
        }

    }
}