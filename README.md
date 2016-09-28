# Zero Mega

Zero Mega is a project from Things Expert implemented in partner with Microsoft. In this repository you can find all solutions and scripts related to the project.

There are 3 folders:
- **Bash Scripts:** Here are the scripts in charge to send data from Linux Servers to Azure Event Hub.
- **Stream Analytics Scripts:** Here are the scripts for processing incoming data in Azure Stream Analytics.
- **ZeroMega**: The Visual Studio solution with the Zero Mega ASP.NET Web API.

## Architecture
### Part 1: Data Insertion 
![](/Images/architecture_1.png)

### Part 2: Data Query
![](/Images/architecture_2.png)

## Running the solution:

### Prerequisites:
- Azure Account
- Azure Active Directory Tenant
- Microsoft Azure Storage Emulator

### Step 1.	Use Azure AD to store the account number as an Extension Property
1. Create an Application in Azure AD and called it ZeroGraph.
2. Get the Tenand Name, Tenant ID, Client ID and Client Secret.
3. Insert your values in Constants.cs file:
 ```cs
 public const string TenantName = "<Your Tenant Name>.onmicrosoft.com";
 public const string TenantId = "<Your Tenant ID>";
 public const string ClientId = "<Your Client ID>";
 public const string ClientSecret = "<Your Client Secret>";
 public const string AuthString = "https://login.windows.net/" + TenantName;
 public const string ResourceUrl = "https://graph.windows.net";

 ```
4. Open GraphConsoleApp project and Run with F5.
5. Type and set the desired users’ account.

### Step 2.	Setup the IoT/Data Insertion Enviroment
1.	Create a Service Bus Namespace
2.	Create an Event Hub
3.	Create a Storage Account with General Purpose
4.	Use Microsoft Azure Storage Emulator and create:
  * BLOB Container called idthingsrd
  * Tables xDRTable and xDR2LogsTable
5. Upload the file Stream Analytics Scripts\2016-08-09-03-22-ref.csv into the idthingsrd container.
6. Create a Stream Analytics and set it this way:
  * Input 1: Event Hub
  * Input 2: Reference data idthingsrd
  * Output 1: Table xDRTable
  * Output 2: Table xDR2LogsTable
  * Query: Copy and paste the content of Stream Analytics Scripts\query.txt

### Step 3.	Setup the Data Query Enviroment
1.	
