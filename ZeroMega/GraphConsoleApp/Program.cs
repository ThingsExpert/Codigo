//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

#endregion

namespace GraphConsoleApp
{
    public class Program
    {

        //****************************************************************************************
        //
        // This console application is a .Net sample, using the Graph API Client library (Version 2.0) 
        // - it demonstrates common Read calls to the Graph API including Getting Users, Groups, Group 
        // Membership, Roles, Tenant information, Service Principals, Applications. The second part of 
        // the sample app demonstrates common Write/Update/Delete options on Users, Groups, and shows 
        // how to execute User License Assignment, updating a User's thumbnailPhoto and links, etc. It 
        // also can read the contents from the signed-on user's mailbox.
        //
        // TODO: 
        //   1.  Run the sample using the demo tenant. The sample app is preconfigured to read data 
        //       from a Demonstration company (GraphDir1.onMicrosoft.com) in Azure AD. Run the sample 
        //       application by selecting F5. The second part of the app will require Admin credentials, 
        //       you can simulate authentication using this demo user account: 
        //              userName = demoUser@graphDir1.onMicrosoft.com, password = graphDem0 
        //       However, this is only a user account and does not have administrative permissions to 
        //       execute updates - therefore, you will see "..unauthorized.." response errors when attempting 
        //       any requests requiring admin permissions. To see how updates work, you will need to 
        //       configure and use this sample with your own tenant - see the next step.
        //
        //   2.  Running this application with your own Azure Active Directory tenant:
        //       Update the values in the Constants.cs file as described in the instructions at 
        //       https://github.com/AzureADSamples/ConsoleApp-GraphAPI-DotNet/blob/master/README.md
        //
        //   Note: For more Azure AD samples, go to https://github.com/AzureADSamples
        //****************************************************************************************

        // Single-Threaded Apartment required for OAuth2 Authz Code flow (User Authn) to execute for this demo app
        [STAThread]
        private static void Main()
        {
            string extension = "AccountNumber";
            string extensionName = "extension_091c133eb3934bf9900259a2814b1cad_AccountNumber";

            Console.WriteLine("Enter the user mail (without @tenant.onmicrosoft.com): ");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter the value for {0}: ", extension);
            string value = Console.ReadLine();


            #region Setup Active Directory Client

            //*********************************************************************
            // setup Active Directory Client
            //*********************************************************************
            ActiveDirectoryClient activeDirectoryClient;
            try
            {
                activeDirectoryClient = AuthenticationHelper.GetActiveDirectoryClientAsApplication();
            }
            catch (AuthenticationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Acquiring a token failed with the following error: {0}", ex.Message);
                if (ex.InnerException != null)
                {
                    //You should implement retry and back-off logic per the guidance given here:http://msdn.microsoft.com/en-us/library/dn168916.aspx
                    //InnerException Message will contain the HTTP error status codes mentioned in the link above
                    Console.WriteLine("Error detail: {0}", ex.InnerException.Message);
                }
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            #endregion

            #region TenantDetails

            //*********************************************************************
            // Get Tenant Details
            // Note: update the string TenantId with your TenantId.
            // This can be retrieved from the login Federation Metadata end point:             
            // https://login.windows.net/GraphDir1.onmicrosoft.com/FederationMetadata/2007-06/FederationMetadata.xml
            //  Replace "GraphDir1.onMicrosoft.com" with any domain owned by your organization
            // The returned value from the first xml node "EntityDescriptor", will have a STS URL
            // containing your TenantId e.g. "https://sts.windows.net/4fd2b2f2-ea27-4fe5-a8f3-7b1a7c975f34/" is returned for GraphDir1.onMicrosoft.com
            //*********************************************************************
            VerifiedDomain initialDomain = new VerifiedDomain();
            VerifiedDomain defaultDomain = new VerifiedDomain();
            ITenantDetail tenant = null;
            Console.WriteLine("\n Retrieving Tenant Details");
            try
            {
                List<ITenantDetail> tenantsList = activeDirectoryClient.TenantDetails
                    .Where(tenantDetail => tenantDetail.ObjectId.Equals(Constants.TenantId))
                    .ExecuteAsync().Result.CurrentPage.ToList();
                if (tenantsList.Count > 0)
                {
                    tenant = tenantsList.First();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError getting TenantDetails {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }

            if (tenant == null)
            {
                Console.WriteLine("Tenant not found");
            }
            else
            {
                TenantDetail tenantDetail = (TenantDetail)tenant;
                Console.WriteLine("Tenant Display Name: " + tenantDetail.DisplayName);

                // Get the Tenant's Verified Domains 
                initialDomain = tenantDetail.VerifiedDomains.First(x => x.Initial.HasValue && x.Initial.Value);
                Console.WriteLine("Initial Domain Name: " + initialDomain.Name);
                defaultDomain = tenantDetail.VerifiedDomains.First(x => x.@default.HasValue && x.@default.Value);
                Console.WriteLine("Default Domain Name: " + defaultDomain.Name);

                // Get Tenant's Tech Contacts
                foreach (string techContact in tenantDetail.TechnicalNotificationMails)
                {
                    Console.WriteLine("Tenant Tech Contact: " + techContact);
                }
            }

            #endregion

            #region Search User by UPN

            // search for a single user by UPN
            string searchString = userName + "@" + initialDomain.Name;
            Console.WriteLine("\n Retrieving user with UPN {0}", searchString);
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
                Console.WriteLine("\nError getting new user {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }
            // should only find one user with the specified UPN
            if (retrievedUsers != null && retrievedUsers.Count == 1)
            {
                retrievedUser = (User)retrievedUsers.First();
            }
            else
            {
                Console.WriteLine("User not found {0}", searchString);
            }

            #endregion

            #region Search For Application

            Application appObject = new Application();
            List<IApplication> retrievedApps = null;
            try
            {
                retrievedApps = activeDirectoryClient.Applications
                    .Where(app => app.AppId.Equals(Constants.ClientId))
                    .ExecuteAsync().Result.CurrentPage.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError getting new app {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }
            // should only find one app with the specified client id
            if (retrievedApps != null && retrievedApps.Count == 1)
            {
                appObject = (Application)retrievedApps.First();
            }
            else
            {
                Console.WriteLine("App not found {0}", searchString);
            }

            #endregion

            #region Create an Extension Property

            //ExtensionProperty accountId = new ExtensionProperty
            //{
            //    Name = extension,
            //    DataType = "String",
            //    TargetObjects = { "User" }
            //};
            //try
            //{
            //    appObject.ExtensionProperties.Add(accountId);
            //    appObject.UpdateAsync().Wait();
            //    Console.WriteLine("\nUser object extended successfully.");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("\nError extending the user object {0} {1}", e.Message,
            //        e.InnerException != null ? e.InnerException.Message : "");
            //}

            #endregion

            #region Manipulate an Extension Property

            try
            {
                if (retrievedUser != null && retrievedUser.ObjectId != null)
                {
                    retrievedUser.SetExtendedProperty(extensionName, value);
                    retrievedUser.UpdateAsync().Wait();
                    Console.WriteLine("\nUser {0}'s extended property set successully.", retrievedUser.DisplayName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError Updating the user object {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }

            #endregion

            #region Get an Extension Property

            try
            {
                if (retrievedUser != null && retrievedUser.ObjectId != null)
                {
                    IReadOnlyDictionary<string, object> extendedProperties = retrievedUser.GetExtendedProperties();
                    object extendedProperty = extendedProperties[extensionName];
                    Console.WriteLine("\n Retrieved User {0}'s extended property value is: {1}.", retrievedUser.DisplayName,
                        extendedProperty);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError Updating the user object {0} {1}", e.Message,
                    e.InnerException != null ? e.InnerException.Message : "");
            }

            #endregion

            Console.WriteLine("\nCompleted. Press Any Key to Exit.");
            Console.ReadKey();
        }
    }
}