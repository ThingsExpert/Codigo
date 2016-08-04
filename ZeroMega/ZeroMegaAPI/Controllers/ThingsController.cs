using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using ZeroMegaAPI.Models;
using ZeroMegaAPI.Repositories;

namespace ZeroMegaAPI.Controllers
{
    [Authorize]
    public class ThingsController : ApiController
    {
        private int _account;
        private PositionRepository _repository = new PositionRepository();

        public ThingsController()
        {
            

        }

        public async Task<IEnumerable<ThingPosition>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var user = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;

            _account = GetAccountId(user);

            var data = await _repository.GetAllThingsPositions(_account);

            return data;
        }

        string extensionName = "extension_091c133eb3934bf9900259a2814b1cad_AccountNumber";

        private int GetAccountId(string UserPrincipalName)
        {
            ActiveDirectoryClient activeDirectoryClient;
            activeDirectoryClient = AuthenticationHelper.GetActiveDirectoryClientAsApplication();

            string searchString = UserPrincipalName;
            User retrievedUser = new User();
            List<IUser> retrievedUsers = null;
            try
            {
                retrievedUsers = activeDirectoryClient.Users
                    .Where(user => user.UserPrincipalName.Equals(searchString))
                    .ExecuteAsync().Result.CurrentPage.ToList();
            }
            catch (Exception)
            {
                throw;
            }

            retrievedUser = (User)retrievedUsers.First();


            Application appObject = new Application();
            List<IApplication> retrievedApps = null;

            retrievedApps = activeDirectoryClient.Applications
                .Where(app => app.AppId.Equals(Constants.ClientId))
                .ExecuteAsync().Result.CurrentPage.ToList();

            appObject = (Application)retrievedApps.First();


            object value = null;
            try
            {
                if (retrievedUser != null && retrievedUser.ObjectId != null)
                {
                    IReadOnlyDictionary<string, object> extendedProperties = retrievedUser.GetExtendedProperties();
                    value = extendedProperties[extensionName];
                }
            }
            catch (Exception)
            {
                throw;
            }

            return int.Parse(value.ToString());
        }
    }
}