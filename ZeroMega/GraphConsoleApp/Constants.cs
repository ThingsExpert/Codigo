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

namespace GraphConsoleApp
{
    internal class Constants
    {
        public const string TenantName = "ip101cloud.onmicrosoft.com";
        public const string TenantId = "15a1913a-d95c-47d8-aaf4-6f9d77cfd1f1";
        public const string ClientId = "46182689-17f0-4b6d-96b3-5d2c73ce50e5";
        public const string ClientSecret = "BQnZF81bQNk1MAi3JZpIM/UXyJJWSVZQ2M0Ifeg8QZs=";
        //public const string ClientIdForUserAuthn = "66133929-66a4-4edc-aaee-13b04b03207d";
        public const string AuthString = "https://login.windows.net/" + TenantName;
        public const string ResourceUrl = "https://graph.windows.net";
    }
}