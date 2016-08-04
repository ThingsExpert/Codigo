using Microsoft.Azure.ActiveDirectory.GraphClient;
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


        }

        public string Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var user = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;

            _account = GetAccountId(user);

            return _account;
        }

        string extensionName = "extension_091c133eb3934bf9900259a2814b1cad_AccountNumber";

        private string GetAccountId(string UserPrincipalName)
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
            catch (Exception e)
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


            object extendedProperty = null;
            try
            {
                if (retrievedUser != null && retrievedUser.ObjectId != null)
                {
                    IReadOnlyDictionary<string, object> extendedProperties = retrievedUser.GetExtendedProperties();
                    extendedProperty = extendedProperties[extensionName];
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return (string)extendedProperty;
        }
    }
}