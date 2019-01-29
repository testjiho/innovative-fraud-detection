![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
Cosmos DB real-time advanced analytics
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step
</div>

<div class="MCWHeader3">
January 2019
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2019 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Cosmos DB real-time advanced analytics hands-on lab step-by-step](#cosmos-db-real-time-advanced-analytics-hands-on-lab-step-by-step)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Before the hands-on lab](#before-the-hands-on-lab)
  - [Exercise 1: Collecting streaming transaction data](#exercise-1-collecting-streaming-transaction-data)
    - [Task 1: Configure Event Hubs and the transaction generator](#task-1-configure-event-hubs-and-the-transaction-generator)
    - [Task 2: Ingesting streaming data into Cosmos DB](#task-2-ingesting-streaming-data-into-cosmos-db)
    - [Task 3: Choosing between Cosmos DB and Event Hubs for ingestion](#task-3-choosing-between-cosmos-db-and-events-hubs-for-ingestion)
  - [Exercise 2: Understanding and preparing the transaction data at scale](#exercise-2-understanding-and-preparing-the-transaction-data-at-scale)
    - [Task 1: Create a service principal for OAuth access to the ADLS Gen2 filesystem](#task-1-create-a-service-principal-for-oauth-access-to-the-adls-gen2-filesystem)
    - [Task 2: Grant ADLS Gen2 permissions to the service principal](#task-2-grant-adls-gen2-permissions-to-the-service-principal)
    - [Task 3: Retrieve your Azure AD tenant ID](#task-3-retrieve-your-azure-ad-tenant-id)
    - [Task 4: Install the Azure Cosmos DB Spark Connector in Databricks](#task-4-install-the-azure-cosmos-db-spark-connector-in-databricks)
    - [Task 5: Querying transactions directly from Cosmos DB with Azure Databricks and Spark](#task-5-querying-transactions-directly-from-cosmos-db-with-azure-databricks-and-spark)
    - [Task 6: Responding to streaming transactions using the Cosmos DB Change Feed and Spark Structured Streaming in Azure Databricks](#task-6-responding-to-streaming-transactions-using-the-cosmos-db-change-feed-and-spark-structured-streaming-in-azure-databricks)
  - [Exercise 3: Creating and evaluating fraud models](#exercise-3-creating-and-evaluating-fraud-models)
    - [Task 1: Task name](#task-1-task-name-2)
    - [Task 2: Task name](#task-2-task-name-2)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Task name](#task-1-task-name-3)
    - [Task 2: Task name](#task-2-task-name-3)

<!-- /TOC -->

# Cosmos DB real-time advanced analytics hands-on lab step-by-step

## Abstract and learning objectives

Woodgrove Bank, who provides payment processing services for commerce, is looking to design and implement a PoC of an innovative fraud detection solution. They want to provide new services to their merchant customers, helping them save costs by applying machine learning and advanced analytics to detect fraudulent transactions. Their customers are around the world, and the right solutions for them would minimize any latencies experienced using their service by distributing as much of the solution as possible, as closely as possible, to the regions in which their customers use the service.

In this hands-on lab session, you will implement a PoC of the data pipeline that could support the needs of Woodgrove Bank.

At the end of this workshop, you will be better able to implement solutions that leverage the strengths of Cosmos DB in support of advanced analytics solutions that require high throughput ingest, low latency serving and global scale in combination with scalable machine learning, big data and real-time processing capabilities.

## Overview

Woodgrove Bank, who provides payment processing services for commerce, is looking to design and implement a proof-of-concept (PoC) of an innovative fraud detection solution. They want to provide new services to their merchant customers, helping them save costs by applying machine learning and advanced analytics to detect fraudulent transactions. Their customers are around the world, and the right solutions for them would minimize any latencies experienced using their service by distributing as much of the solution as possible, as closely as possible, to the regions in which their customers use the service.

## Solution architecture

Below is a diagram of the solution architecture you will build in this lab. Please study this carefully, so you understand the whole of the solution as you are working on the various components.

![Preferred solution diagram outlining the ingestion of streaming payment transactions into Cosmos DB, and fed into Azure Databricks. Azure Databricks will process the data and insert it into Azure Data Lake Storage Gen2 for long-term storage. Databricks is also used for training machine learning models for batch and real-time fraud detection. Power BI is used for dashboards and reporting.](../media/outline-architecture.png "Preferred Solution diagram")

The solution begins with the payment transaction systems writing transactions to Azure Cosmos DB. With change feed enabled in Cosmos DB, the transactions can be read as a stream of incoming data within an Azure Databricks notebook, using the `azure-cosmosdb-spark` connector, and stored long-term within an Azure Databricks Delta table backed by Azure Data Lake Storage. The Delta tables efficiently manage inserts and updates (e.g., upserts) to the transaction data. Tables created in Databricks over this data can be accessed by business analysts using dashboards and reports in Power BI, by using Power BI's Spark connector. Data scientists and engineers can create their own reports against this data, using Azure Databricks notebooks. Azure Databricks also supports training and validating the machine learning model, using historical data stored in Azure Data Lake Storage. The model can be periodically re-trained using the data stored in Delta tables or other historical tables. The Azure Machine Learning service is used to deploy the trained model as a real-time scoring web service running on a highly available Azure Kubernetes Service cluster (AKS cluster). The trained model is also used in scheduled offline scoring through Databricks jobs, and the "suspicious activity" output is stored in Azure Cosmos DB so it is globally available in regions closest to Woodgrove Bank's customers through their web applications. Finally, Azure Key Vault is used to securely store secrets, such as account keys and connection strings, and serves as a backing for Azure Databricks secret scopes.

> **Note**: The preferred solution is only one of many possible, viable approaches.

## Requirements

1. Microsoft Azure subscription (non-Microsoft subscription, must be a pay-as-you subscription).
2. An Azure Databricks cluster running Databricks Runtime 5.1 or above. Azure Databricks integration with Azure Data Lake Storage Gen2 is **fully supported in Databricks Runtime 5.1**.
   - **IMPORTANT**: To complete the OAuth 2.0 access components of this hands-on lab you must:
     - Have a cluster running Databricks Runtime 5.1 and above.
     - Have permissions within your Azure subscription to create an App Registration and service principal within Azure Active Directory.

## Exercise 1: Collecting streaming transaction data

Duration: 30 minutes

In this exercise, you will configure a payment transaction generator to write real-time streaming online payments to both Event Hubs and Azure Cosmos DB. By the end, you will have selected the best ingest option before continuing to the following exercise where you will process the generated data.

### Task 1: Configuring Event Hubs and the transaction generator

In this task, you will configure the payment transaction data generator project by completing TODO items in the source code and adding connection information for your Event Hub.

1. Open File Explorer on your machine or VM and navigate to the location you extracted the MCW repo .zip file to (C:\\CosmosMCW\\).

2. Open **TransactionGenerator.sln** in the `Hands-on lab\lab-files\TransactionGenerator` directory. This will open the solution in Visual Studio.

    ![Screenshot of the solution folder with the TransactionGenerator.sln file selected.](media/solution-folder.png "File Explorer")

3. Double-click `appsettings.json` in the Solution Explorer to open it. This file contains the settings used by the console app to connect to your Azure services and to configure application behavior settings. The console app is programmed to either use values stored in this file, or within the machine's environment variables. This makes you capable of distributing the executable or containerizing it and passing in environment variables via the command line.

    ![appsettings.json is highlighted in the Visual Studio Solution Explorer.](media/solution-explorer.png "Solution Explorer")

    The `appsettings.json` file contains the following:

    ```javascript
    {
        "EVENT_HUB_1_CONNECTION_STRING": "",
        "EVENT_HUB_2_CONNECTION_STRING": "",
        "EVENT_HUB_3_CONNECTION_STRING": "",

        "COSMOS_DB_ENDPOINT": "",
        "COSMOS_DB_AUTH_KEY": "",

        "SECONDS_TO_LEAD": "0",
        "SECONDS_TO_RUN": "600"
    }
    ```

    `SECONDS_TO_LEAD` is the amount of time to wait before sending payment transaction data. Default value is `0`. `SECONDS_TO_RUN` is the maximum amount of time to allow the generator to run before stopping transmission of data. The default value is `600`. Data will also stop transmitting after the included `Untagged_Transactions.csv` file's data has been sent.

4. Copy your Event Hub connection string value you saved during the steps you completed in the before the hands-on lab setup guide. Paste this value into the double-quotes located next to `EVENT_HUB_1_CONNECTION_STRING`.

    ![The Event Hub connection string is pasted in appsettings.json.](media/event-hub-connection-string.png "appsettings.json")

5. Save the file.

6. Open `Program.cs` in the Visual Studio Solution Explorer.

7. In Visual Studio, select **View**, then **Task List** from the dropdown menu.

    ![In Visual Studio, select View then Task List.](media/select-task-list.png "Select task list")

    This will display the TODO items within the code comments as a list of tasks you can double-click to jump to its location.

    ![Screenshot of the task list.](media/task-list.png "Visual Studio Task List")

8. Go to **TODO 1** located in `Program.cs` by double-clicking the item in the Task List. Paste the following code under TODO 1, which uses the Event Hub client to send the event data, setting the partition key to `IpCountryCode`:

    ```csharp
    await eventHubClient.SendAsync(eventData: eventData,
        partitionKey: transaction.IpCountryCode).ConfigureAwait(false);
    ```

    Your completed code should look like the following:

    ![Screenshot of completed TODO 1 code.](media/completed-todo-1-code.png "Completed code")

9. Paste the code below under **TODO 2** to increment the count of the number of Event Hub requests that succeeded:

    ```csharp
    _eventHubRequestsSucceededInBatch++;
    ```

10. Paste the code below under **TODO 3** to instantiate a new Event Hub client and add it to the `eventHubClients` collection:

    ```csharp
    EventHubClient.CreateFromConnectionString(
        arguments.EventHubConnectionString
    ),
    ```

11. Save your changes.

### Task 2: Ingesting streaming data into Cosmos DB

In this task, you will configure Cosmos DB's time-to-live (TTL) settings to On with no default. This will allow the data generator to expire, or delete, the ingested messages after any desired period of time by setting the TTL value (object property of `ttl`) on individual messages as they are sent.

Next you will pass in the Azure Cosmos DB URI and Key values to the data generator so it can connect to and send events to your collection.

1. Navigate to your Azure Cosmos DB account in the Azure portal, then select **Data Explorer** on the left-hand menu.

    ![Data Explorer is selected within the left-hand menu](media/cosmos-db-data-explorer-link.png "Select Data Explorer")

2. Expand your **Woodgrove** database and your **transactions** collection, then select **Scale & Settings**.

3. Under Settings within the Scale & Settings blade, select the **On (no default)** option for Time to Live. This setting is required to allow documents added to the collection to be configured with their own TTL values.

    ![The Scale & Settings blade is shown with the On (no default) Time to Live option selected.](media/cosmos-db-ttl-settings.png "Scale & Settings blade")

4. Select **Save** to apply your settings.

5. Open Visual Studio to go back to the TransactionGenerator project.

6. Open the `appsettings.json` file once more. Paste your Cosmos DB endpoint value next to `COSMOS_DB_ENDPOINT`, and the Cosmos DB authorization key next to `COSMOS_DB_AUTH_KEY`. You recorded these values during the Cosmos DB provisioning steps in the before the hands-on lab setup guide. For example:

    ![The Cosmos DB values have been added to appsettings.json](media/cosmos-db-values.png "appsettings.json")

7. Open `Program.cs` and paste the code below under **TODO 4** to send the generated transaction data to Cosmos DB and store the returned `ResourceResponse` object into a new variable for statistics about RU/s used:

    ```csharp
    var response = await _cosmosDbClient.CreateDocumentAsync(collectionUri, transaction)
        .ConfigureAwait(false);
    ```

8. Paste the code below under **TODO 5** to append the number of RU/s consumed to the `_cosmosRUsPerBatch` variable:

    ```csharp
    _cosmosRUsPerBatch += response.RequestCharge;
    ```

9. Paste the code below under **TODO 6** to set the Cosmos DB connection policy:

    ```csharp
    var connectionPolicy = new ConnectionPolicy
    {
        ConnectionMode = ConnectionMode.Direct,
        ConnectionProtocol = Protocol.Tcp
    };
    ```

10. Save your changes.

11. Run the console app by clicking **Debug**, then **Start Debugging** in the top menu in Visual Studio, or press _F-5_ on your keyboard.

    ![Screenshot showing the Debug menu expanded in Visual Studio with the Start Debugging menu option highlighted.](media/debug-in-vs.png "Debug")

12. The PaymentGenerator console window will open, and you should see it start to send data after a few seconds. You may close the window or press `Ctrl+C` or `Ctrl+Break` at any time to stop sending data to Event Hubs and Cosmos DB.

    ![Screenshot of the PaymentGenerator console window running.](media/payment-generator-console.png "Payment Generator console window")

    The top of the output displays information about the Cosmos DB collection you created (transactions), the requested RU/s as well as estimated hourly and monthly cost. After every 1,000 records are requested to be sent, you will see output statistics so you can compare Event Hubs to Cosmos DB. Be on the lookout for the following:

    - Compare Event Hub to Cosmos DB statistics. They should have similar processing times and successful calls.
    - Inserted line shows successful inserts in this batch and throughput for writes/second with RU/s usage and estimated monthly ingestion rate added to Cosmos DB statistics.
    - Processing time: Shows whether the processing time for the past 1,000 requested inserts is faster or slower than the other service.
    - Total elapsed time: Running total of time taken to process all documents.
    - If this value continues to be grow higher for Cosmos DB vs. Event Hubs, that is a good indicator that the Cosmos DB requests are being throttled. Consider increasing the RU/s for the container.
    - Succeeded shows number of accumulative successful inserts to the service.
    - Pending are items in the bulkhead queue. This amount will continue to grow if the service is unable to keep up with demand.
    - Accumulative failed requests that encountered an exception.

    > The obvious and recommended method for sending a lot of data is to do so in batches. This method can multiply the amount of data sent with each request by hundreds or thousands. However, the point of our exercise is not to maximize throughput and send as much data as possible, but to compare the relative performance between Event Hubs and Cosmos DB.

13. As an experiment, scale the number of requested RU/s for your Cosmos DB collection down to 750. After doing so, you should see increasingly slower transfer rates to Cosmos DB due to throttling. You will also see the pending queue growing at a higher rate. The reason for this is because when the number of writes (remember, writes use 5 RU/s vs. just 1 RU/s for reads) exceeds the allotted amount of RU/s, Cosmos DB sends a 429 response with a _retry_after_ header value to tell the consumer that it is resource-constrained. The SDK automatically handles this by waiting for the specified amount of time, then retrying. After you are done experimenting, set the RU/s back to 15,000.

### Task 3: Choosing between Cosmos DB and Event Hubs for ingestion

Woodgrove Bank has a number of requirements around ingesting payment data, including data retention of the hot data and geographic locations to which the data is replicated for high availability and global distribution of the data for processing. There are many similarities between Event Hubs and Cosmos DB that allow both to work well for data ingestion. However, these services have some significant differences in their overall feature set that you need to evaluate to choose the best option for this customer situation.

In this exercise, you will use the data generator to send data to both Event Hubs and Cosmos DB and compare the performance of the two. You will also configure the generator and the services to set message retention and to send data to various global regions.

1. Open Visual Studio and paste the code below under **TODO 7** (located within `Transaction.cs`) to set the time to live (TTL) value to 60 days (in seconds):

    ```csharp
    tx.TimeToLive = 60 * 60 * 24 * 60;
    ```

    This configures Cosmos DB to automatically delete the ingested messages after 60 days by setting the TTL value (`ttl` property) on individual messages as they are sent. This optimization helps save in storage costs while meeting Woodgrove Bank's requirement to keep the streaming data available for that amount of time so they can reprocess in Azure Databricks, or query the raw data within the collection as needed.

    > Setting the TTL for documents saved to Cosmos DB individually for any length of time desired (even beyond 7 days) is and advantage Cosmos DB has over Event Hubs when used for ingesting streaming data.

2. Save your changes.

3. Navigate to your Event Hubs namespace for this lab in the Azure portal. Select **Event Hubs** from the left-hand menu.

    ![Event Hubs is selected within the left-hand menu](media/select-event-hubs.png "Select Event Hubs")

4. Select your **transactions** event hub.

5. Select **Properties** on the left-hand menu.

6. Drag the **Message Retention** slider all the way to the right to set the value to 7. Unfortunately, this is as long as you can set message retention for Event Hubs messages using the portal UI. It is possible to contact Microsoft to set the value to as many as 4 weeks. Unfortunately, this does not meet Woodgrove Bank's requirements for retaining this hot data for 60 days.

    ![Screenshot displaying the event hub properties and the Message Retention value being set to 7.](media/event-hub-message-retention.png "Properties")

7. Select **Save Changes**.

    > Woodgrove Bank wants to write all transaction data simultaneously two three different geographic locations: United States, Great Britain, and East Asia. All data should be able to be read from these locations with as little latency as possible. They require this for redundancy purposes as well as being able to better process the data in those regions.

8. Create two more Event Hubs namespaces and event hubs within to ingest transaction data. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "event hubs" into the Search the Marketplace box, select **Event Hubs** from the results, and then select **Create**.

    ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, event hubs is entered into the Search the Marketplace box, and Event Hubs is highlighted in the results.](media/create-resource-event-hubs.png "Create an Event Hubs namespace")

9. On the Create Namespace blade, enter the following:

    - **Name**: Enter a globally unique name (indicated by a green check mark).
    - **Pricing tier**: Select Standard.
    - **Enable Kafka**: Unchecked.
    - **Make this namespace zone redundant**: Unchecked.
    - **Subscription**: Select the subscription you are using for this hands-on lab.
    - **Resource group**: Choose the hands-on-lab-SUFFIX resource group.
    - **Location**: Select **UK South**. (If you already selected this for the first Event Hub namespace, select a US region)
    - **Throughput Units**: Set the slider all the way to the left, setting the value to 1.
    - **Enable Auto-Inflate**: Unchecked.

    ![The Create Namespace blade is displayed, with the previously mentioned settings entered into the appropriate fields.](media/create-event-hubs-blade-uk.png "Create Namespace")

10. Select **Create**.

11. Navigate to the newly provisioned Event Hubs namespace in the Azure portal, then select **Event Hubs** under Entities on the left-hand menu.

    ![Event Hubs is selected within the left-hand menu](media/select-event-hubs.png "Select Event Hubs")

12. Select **+ Event Hub** in the top toolbar.

    ![Select the + Event Hub button in the top toolbar](media/add-event-hub-button.png "Add Event Hub")

13. In the **Create Event Hub** blade, configure the following:

    - **Name**: Enter "transactions".
    - **Partition Count**: Move the slider to set the value to 10.
    - **Message Retention**: Set to 7.
    - **Capture**: Off.

    ![The Create Event Hub blade is displayed, with the previously mentioned settings entered into the appropriate fields](media/create-event-hub-blade-7-day-retention.png "Create Event Hub")

14. Select **Create**.

15. After the new Event Hub is created, select it then select **Shared access policies** under Settings in the left-hand menu.

    ![Shared access policies is selected within the left-hand menu](media/select-shared-access-policies.png "Select Shared access policies")

16. Select **+ Add** in the top toolbar.

    ![Select the + Add button in the top toolbar](media/add-shared-access-policy.png "Add Shared Access Policy")

17. In the **Add SAS Policy** blade, configure the following:

    - **Policy name**: Enter "Sender".
    - **Manage**: Unchecked.
    - **Send**: Checked.
    - **Listen**: Unchecked.

    ![The Add SAS Plicy is displayed, with the previously mentioned settings entered into the appropriate fields](media/add-sas-policy-sender.png "Add SAS Policy")

18. Select **Create**.

19. Select **+ Add** in the top toolbar to add another policy.

    ![Select the + Add button in the top toolbar](media/add-shared-access-policy.png "Add Shared Access Policy")

20. In the **Add SAS Policy** blade, configure the following:

    - **Policy name**: Enter "Listener".
    - **Manage**: Unchecked.
    - **Send**: Unchecked.
    - **Listen**: Checked.

    ![The Add SAS Plicy is displayed, with the previously mentioned settings entered into the appropriate fields](media/add-sas-policy-listener.png "Add SAS Policy")

21. Select **Create**.

22. Select the **Sender** access policy.

23. Copy the **Connection string-primary key** value. Save this value for the Sender policy in Notepad or similar for later.

24. Follow the step above to copy the **Connection string-primary key** value for the Listener policy and save for later.

25. **Repeat steps 8 through 24 above** to create a new Event Hub namespace in the **East Asia** region.

    ![Create Event Hubs Namespace blade with the East Asia Location selection highlighted.](media/create-event-hubs-blade-asia.png "Create Namespace")

26. Open Visual Studio to go back go the TransactionGenerator project.

27. Open the `appsettings.json` file once more. Paste your two new Event Hub connection strings into the values for `EVENT_HUB_2_CONNECTION_STRING` and `EVENT_HUB_3_CONNECTION_STRING`, respectively.

    ![The two new Event Hub connection strings are pasted into the appsettings.json file.](media/event-hub-connection-strings.png "appsettings.json")

28. Save your changes.

29. Open `Program.cs` and paste the below code underneath **TODO 8** to add two new Event Hub clients to the `eventHubClients` collection, using the two new connection string values:

    ```csharp
    EventHubClient.CreateFromConnectionString(
        arguments.EventHub2ConnectionString
    ),
    EventHubClient.CreateFromConnectionString(
        arguments.EventHub3ConnectionString
    )
    ```

    ![Screenshot displaying completed code.](media/event-hub-clients-added.png "Program.cs")

30. Now, we will add the two additional regions to Cosmos DB. Navigate to the Azure portal and select your Cosmos DB account you created for this lab.

31. Select **Replicate data globally** underneath Settings in the left-hand menu.

    ![Left-hand menu with the Replicate data globally link highlighted.](media/replicate-data-globally-link.png "Replicate data globally link")

32. Within the Replicate data globally blade, select **+ Add region** above the listed regions in the Configure regions section.

    ![Screenshot with the Add region link highlighted.](media/add-region-link.png "Add region link")

    > Notice that there are already two regions and they each have both reads and writes enabled. This is because you enabled the geo-redundancy and multi-region writes options when you provisioned Cosmos DB.

33. Select **East Asia** in the dropdown list, then select **OK** to add the region.

    ![East Asia is selected and highlighted in the location dropdown list.](media/add-region-east-asia.png "East Asia")

34. Select **+ Add region** again, this time selecting **UK South** in the dropdown list. Select **OK** to add the new region.

    ![UK South is selected and highlighted in the location dropdown list.](media/add-region-uk-south.png "UK South")

35. Notice that the two new regions are highlighted on the world map, and each have both reads and writes enabled. Congratulations! You completed all the steps to write to and read from multiple regions around the world with Cosmos DB! Finally, select **Save** to save your changes.

    ![Map showing newly added regions for Cosmos DB.](media/replicate-data-globally-map.png "Cosmos DB region map")

    > You may have to wait several minutes for the change to take effect. In the meantime, you can feel free to continue and run the transaction generator. Cosmos DB can still ingest data as regions are being added. There should be no performance impact during this time or after the provisioning is complete.

36. Open Visual Studio and debug the TransactionGenerator project. Let it run for at least 1 minute, or long enough to send 5,000 messages.

    ![The TransactionGenerator console shows event hubs overall running slower than Cosmos DB.](media/console-output-event-hubs-slower.png "Console output")

    Results will vary depending on machine specifications and network speeds, but overall, it will likely take longer to send the data to the three Event Hub instances than to Cosmos DB. You may also notice the Event Hubs pending queue filling up quite a bit more. Also notice that you did not have to make any code changes to write to the additional Cosmos DB regions.

37. Open each of the three Event Hubs namespaces you have created for this lab. You should see an equal number of messages that were sent to each. The graph is shown on the bottom of the Overview blade. Select the **Messages** metric above the graph to view the number of messages received. The screenshot below is of the UK South Event Hub:

    ![The Messages metric is selected for the UK South Event Hubs namespace.](media/uk-south-metrics.png "Event Hubs Overview blade")

38. View the data that was saved to Cosmos DB. Navigate to the Cosmos DB account for this lab in the Azure portal. Select **Data Explorer** on the left-hand menu. Expand the **Woodgrove** database and **transactions** collection, then select **Documents**. Select one of the documents from the list to view it. If you selected a more recently added document, notice that it contains a `ttl` value of 5,184,000 seconds, or 60 days. Also, there is a `collectionType` value of "Transaction". This allows consumers to query documents stored within the collection by the type. This is needed because a collection can contain any number of document types within, since it does not enforce any type of schema.

    ![Screenshot shows a document displayed within the Cosmos DB Data Explorer.](media/cosmos-db-document.png "Cosmos DB Data Exporer")

Given the requirements provided by the customer, Cosmos DB is the best choice for ingesting data for this PoC. Cosmos DB allows for more flexible, and longer, TTL (message retention) than Event Hubs, which is capped at 7 days, or 4 weeks when you contact Microsoft to request the extra capacity. Another option for Event Hubs is to use Event Hubs Capture to simultaneously save ingested data to Blob Storage or Azure Data Lake Store for longer retention and cold storage. However, this will require additional development, including automatic clearing of the data after a period of time. In addition, Woodgrove Bank wanted to be able to easily query this data during the 60-day message retention period, from a database. This could also be accomplished through Azure Data Warehouse using Polybase to query the files, but that requires yet another service they may otherwise not need, as well as additional development, administration, and cost.

Finally, the requirement to synchronize/write the ingested data to multiple regions, which could grow at any time, makes Cosmos DB a more favorable choice. As you can see, there are more steps required to send data to additional regions using Event Hubs, since you have to provision new namespaces and Event Hub instances in each region. You would also have to account for all those instances on the consuming side, which we will not cover in this lab for sake of time. The ability to read/write to multiple regions by adding and removing them at will with no code or changes required is a great value that Cosmos DB adds. Plus, the fact that Cosmos DB will be used in this solution for serving batch-processed fraudulent data on a global scale means that Cosmos DB can be used to meet both the data ingest and delivery needs with no additional services, like Event Hubs, required.

We will continue the lab using Cosmos DB for data ingestion.

## Exercise 2: Understanding and preparing the transaction data at scale

Duration: X minutes

In this exercise, you will use Azure Databricks to explore the transaction data stored in Cosmos DB and respond to transactions directly from the Cosmos DB Change Feed, writing the incoming streaming transaction data into an Azure Databricks Delta table stored in your data lake.

### Task 1: Create a service principal for OAuth access to the ADLS Gen2 filesystem

As an added layer of security when accessing an ADLS Gen2 filesystem using Databricks you can use OAuth 2.0 for authentication. In this task, you will create an identity in Azure Active Directory (Azure AD) known as a service principal to facilitate the use of OAuth authentication.

> **IMPORTANT**: You must have permissions within your Azure subscription to create an App registration and service principal within Azure Active Directory to complete this task.

1. In the [Azure portal](https://portal.azure.com), select **Azure Active Directory** from the left-hand navigation menu, select **App registrations**, and then select **+ New application registration**.

   ![Register new app in Azure Active Directory](media/aad-app-registration.png "Register new app in Azure Active Directory")

2. On the Create blade, enter the following:

   - **Name**: Enter a unique name, such as woodgrove-sp (this name must be unique within your Azure AD subscription, as indicated by a green check mark).
   - **Application type**: Select Web app / API.
   - **Sign-on URL**: Enter <https://woodgrove.com>.

   ![Create a new app registration](media/aad-app-create.png "Create a new app registration")

3. Select **Create**.

4. To provide access your ADLS Gen2 account from Azure Databricks you will use secrets stored in your Azure Key Vault account to provide the credentials of your newly created service principal within Databricks. On the Registered app blade that appears, copy the **Application ID** and paste it into a text editor, such as Notepad, for use in an upcoming step to create a new secret in Key Vault.

   ![Copy the Registered App Application ID](media/registered-app-id.png "Copy the Registered App Application ID")

5. Next, select **Settings** on the Registered app blade, and then select **Keys**.

   ![Open Keys blade for the Registered App](media/registered-app-settings-keys.png "Open Keys blade for the Registered App")

6. On the Keys blade, you will create a new password by doing the following under Passwords:

   - **Description**: Enter a description, such as Databricks-ADLS.
   - **Expires**: Select a duration, such as **In 1 year**.

   ![Create new password](media/registered-app-create-key.png "Create new password")

7. Select **Save**, and then copy the key displayed under **Value**, and paste it into a text editor, such as Notepad, for use in an upcoming step to create a new secret in Key Vault. **Note**: This value will not be accessible once you navigate away from this screen, so make sure you copy it before leaving the Keys blade.

   ![Copy key value](media/registered-app-key-value.png "Copy key value")

8. Navigate to your Azure Key Vault account in the Azure portal, then select **Secrets** under Settings on the left-hand menu. On the Secrets blade, select **+ Generate/Import** on the top toolbar.

    ![Secrets is highlighted on the left-hand menu, and Generate/Import is highlighted on the top toolbar of the Secrets blade.](media/key-vault-secrets.png "Key Vault secrets blade")

9. On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "Woodgrove-SP-Client-ID".
    - **Value**: Paste the Application ID value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-woodgrove-sp-client-id-secret.png "Create a secret")

10. Select **Create**.

11. Select **+ Generate/Import** again on the top toolbar to create another secret.

12. On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "Woodgrove-SP-Client-Key".
    - **Value**: Paste the Password value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-woodgrove-sp-client-key-secret.png "Create a secret")

13. Select **Create**.

### Task 2: Grant ADLS Gen2 permissions to the service principal

In this task, you will assign the required permissions to the service principal in your ADLS Gen2 account.

1. In the [Azure portal](https://portal.azure.com), navigate to the ADLS Gen2 account you created above, select **Access control (IAM)** from the left-hand menu, and then select **+ Add role assignment**.

   ![ADLS Gen2 Access Control blade](media/access-control.png "ADLS Gen2 Access Control blade")

2. On the Add role assignment blade, set the following:

   - **Role**: Select **Storage Blob Data Contributor (Preview)** from the list.
   - **Assign access to**: Choose **Azure AD user, group, or service principal**.
   - **Select**: Enter the name of the service principal you created above, and then select it from the list.

   ![ADLS Gen2 Add role assignment](media/add-role-assignment.png "ADLS Gen2 Add role assignment")

3. Select **Save**

4. You will now see the service principal listed under **Role assignments** on the Access control (IAM) blade.

### Task 3: Retrieve your Azure AD tenant ID

To perform authentication using the service principal account in Databricks you will also need to provide your Azure AD Tenant ID.

1. To retrieve your tenant ID, select **Azure Active Directory** from the left-hand navigation menu in the Azure portal, then select **Properties**, and select the copy button next to **Directory ID** on the Directory Properties blade.

   ![Azure Active Directory is selected in the left-hand menu on the Azure portal, and the properties menu is selected. The Directory ID field is highlighted.](media/aad-tenant-id.png "Retrieve Tenant ID")

2. Navigate to your Azure Key Vault account in the Azure portal, then select **Secrets** under Settings on the left-hand menu. On the Secrets blade, select **+ Generate/Import** on the top toolbar.

    ![Secrets is highlighted on the left-hand menu, and Generate/Import is highlighted on the top toolbar of the Secrets blade.](media/key-vault-secrets.png "Key Vault secrets blade")

3. On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "Azure-Tenant-ID".
    - **Value**: Paste the Directory ID value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-azure-tenant-id-secret.png "Create a secret")

4. Select **Create**.

### Task 4: Install the Azure Cosmos DB Spark Connector in Databricks

In this task, you will install the [Azure Cosmos DB Spark Connector](https://github.com/Azure/azure-cosmosdb-spark) on your Databricks cluster. The connector allows you to easily read to and write from Azure Cosmos DB via Apache Spark DataFrames in `python` and `scala`.

1. In a web browser, download the latest [azure-cosmosdb-spark library](https://github.com/Azure/azure-cosmosdb-spark) from here: <https://search.maven.org/remotecontent?filepath=com/microsoft/azure/azure-cosmosdb-spark_2.3.0_2.11/1.2.2/azure-cosmosdb-spark_2.3.0_2.11-1.2.2-uber.jar>.

2. Navigate to your Azure Databricks workspace in the [Azure portal](https://portal.azure.com/), and select **Launch Workspace** from the overview blade, signing into the workspace with your Azure credentials, if required.

    ![The Launch Workspace button is displayed on the Databricks Workspace Overview blade.](media/databricks-launch-workspace.png "Launch Workspace")

3. Select **Workspace** from the left-hand menu, then select the drop down arrow next to **Shared** and select **Create** and **Library** from the context menus.

    ![The Workspace items is selected in the left-hand menu, and the shared workspace is highlighted. In the Shared workspace context menu, Create and Library are selected.](media/databrick-create-shared-library.png "Create Shared Library")

4. Drag and drop the downloaded Cosmos DB connector JAR file into the box on the Create Library dialog to install the uploaded library into your Databricks cluster.

    ![The Databricks Create Library dialog is displayed, with the azure-cosmosdb-spark library downloaded above added to the upload box.](media/databricks-create-library.png "Create Library")

5. Select **Create** to finish installing the library.

6. On the following screen, check the box for **Install automatically on all clusters**, and select **Confirm** when prompted.

    ![The Install automatically on all clusters box is checked and highlighted on the library dialog.](media/datbricks-install-library-on-all-clusters.png "Install library on all clusters")

### Task 5: Querying transactions directly from Cosmos DB with Azure Databricks and Spark

In this task, you will use an Azure Databricks notebook to create a connection to your Cosmos DB instance from an Azure Databricks notebook, and write queries to explore transaction data retrieved directly from Cosmos DB and Spark SQL.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Users** and your user account.

    ![In the Databricks workspace, Workspace is selected in the left-hand menu, Users is selected, and the user account is selected and highlighted.](media/databricks-user-workspace.png)

2. In your user workspace, select the **CosmosDbAdvancedAnalytics** folder, then select the **Exercise 2** folder, and select the notebook named **1-Querying-Cosmos-DB**.

    ![In the user's workspace, the 2-Querying-Cosmos-DB notebook is selected under the Exercise 2 folder.](media/databricks-user-workspace-ex2-notebook1.png "Notebooks in the user workspace")

3. In the **1-Querying-Cosmos-DB** notebook, follow the instructions to complete the remaining steps of this task.

> **NOTE**: There will be a link at the bottom of each notebook in this exercise to move on to the notebook for the next task, so you will not need to jump back and forth between this document and the Databricks notebooks for this exercise.

### Task 6: Responding to streaming transactions using the Cosmos DB Change Feed and Spark Structured Streaming in Azure Databricks

In this task, you will use an Azure Databricks notebook to create a connection to your Cosmos DB instance from an Azure Databricks notebook, and query streaming data from the Cosmos DB Change Feed.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Users** and your user account.

2. In your user workspace, select the **CosmosDbAdvancedAnalytics** folder, then select the **Exercise 2** folder, and select the notebook named **2-Cosmos-DB-Change-Feed**.

    ![In the user's workspace, the 3-Cosmos-DB-Change-Feed notebook is selected under the Exercise 2 folder.](media/databricks-user-workspace-ex2-notebook2.png "Notebooks in the user workspace")

3. In the **2-Cosmos-DB-Change-Feed** notebook, follow the instructions to complete the remaining steps of this task.

## Exercise 3: Creating and evaluating fraud models

Duration: X minutes

\[insert your custom Hands-on lab content here . . .\]

### Task 1: Task name

1. Number and insert your custom workshop content here . . .

    a. Insert content here

        i.

### Task 2: Task name

1. Number and insert your custom workshop content here . . .

    a. Insert content here

        i.

## After the hands-on lab

Duration: 10 mins

In this exercise, you will delete any Azure resources that were created in support of the lab. You should follow all steps provided after attending the Hands-on lab to ensure your account does not continue to be charged for lab resources.

### Task 1: Delete the resource group

1. Using the [Azure portal](https://portal.azure.com), navigate to the Resource group you used throughout this hands-on lab by selecting Resource groups in the left menu.
2. Search for the name of your research group, and select it from the list.
3. Select Delete in the command bar, and confirm the deletion by re-typing the Resource group name, and selecting Delete.

You should follow all steps provided _after_ attending the Hands-on lab.
