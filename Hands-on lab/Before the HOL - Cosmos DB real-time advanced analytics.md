![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Cosmos DB real-time advanced analytics
</div>

<div class="MCWHeader2">
Before the hands-on lab setup guide
</div>

<div class="MCWHeader3">
April 2019
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2019 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Cosmos DB real-time advanced analytics before the hands-on lab setup guide](#cosmos-db-real-time-advanced-analytics-before-the-hands-on-lab-setup-guide)
  - [Requirements](#requirements)
  - [Before the hands-on lab](#before-the-hands-on-lab)
    - [Task 1: Set up a development environment](#task-1-set-up-a-development-environment)
    - [Task 2: Disable IE Enhanced Security](#task-2-disable-ie-enhanced-security)
    - [Task 3: Install Google Chrome](#task-3-install-google-chrome)
    - [Task 4: Validate connectivity to Azure](#task-4-validate-connectivity-to-azure)
    - [Task 5: Download the starter files](#task-5-download-the-starter-files)
    - [Task 6: Download and install Power BI Desktop](#task-6-download-and-install-power-bi-desktop)
    - [Task 7: Provision a resource group](#task-7-provision-a-resource-group)
    - [Task 8: Create a Cosmos DB account](#task-8-create-a-cosmos-db-account)
    - [Task 9: Create an Azure Databricks workspace](#task-9-create-an-azure-databricks-workspace)
    - [Task 10: Set up Azure Key Vault](#task-10-set-up-azure-key-vault)
    - [Task 11: Configure Cosmos DB](#task-11-configure-cosmos-db)
    - [Task 12: Configure Azure Databricks Key Vault-backed secrets](#task-12-configure-azure-databricks-key-vault-backed-secrets)
    - [Task 13: Provision Event Hubs](#task-13-provision-event-hubs)
    - [Task 14: Create an Azure Data Lake Storage Gen2 account](#task-14-create-an-azure-data-lake-storage-gen2-account)
    - [Task 15: Create an Azure Databricks cluster](#task-15-create-an-azure-databricks-cluster)
    - [Task 16: Open Azure Databricks and load lab notebooks](#task-16-open-azure-databricks-and-load-lab-notebooks)

<!-- /TOC -->

# Cosmos DB real-time advanced analytics before the hands-on lab setup guide

## Requirements

1. Microsoft Azure subscription (non-Microsoft subscription, must be a pay-as-you subscription).
2. **IMPORTANT**: To complete the OAuth 2.0 access components of this hands-on lab you must have permissions within your Azure subscription to create an App Registration and service principal within Azure Active Directory.

## Before the hands-on lab

Duration: 60 minutes

In the Before the hands-on lab exercise, you will set up your environment for use in the rest of the hands-on lab. You should follow all the steps provided in the Before the hands-on lab section to prepare your environment **before attending** the hands-on lab. Failure to do so will significantly impact your ability to complete the lab within the time allowed.

> **Important**: Most Azure resources require unique names. Throughout this lab you will see the word “SUFFIX” as part of resource names. You should replace this with your Microsoft alias, initials, or another value to ensure the resource is uniquely named.

### Task 1: Set up a development environment

If you do not have a machine with Visual Studio Community 2017 (or greater) and the Azure development workload, complete this task.

1. Create a virtual machine (VM) in Azure using the Visual Studio Community 2017 (latest release) on Windows Server 2016 (x64) image. A Windows 10 image will work as well.

   ![In Azure Portal, in the search field, Visual Studio Community 2017 on Windows Server 2016 (x64) is typed. Under Results, Visual Studio Community 2017 on Windows Server 2016 (x64) is selected.](media/create-resource-visual-studio.png 'Azure Portal')

   a. In the Azure portal, select **+ Create a resource** from the left-hand menu.

   b. Type **Visual Studio**.

   c. Select the **Visual Studio Community 2017 (latest release) on Windows Server 2016 (x64)**.

   d. Click **Create**.

   e. Select your subscription and recently created resource group.

   f. For the name, type **MainVM**, or some other globally unique name (as indicated by the checkmark).

   g. Leave the availability option as **No infrastructure redundancy required**.

   h. Ensure the image is **Visual Studio Community 2017 (latest release) on Windows Server 2016 (x64)**.

   i. Select your VM size.

   > **Note**: It is highly recommended to use a DS2 or D2 instance size for this VM.

   j. For username, type **demouser**.

   k. For password, type **Password.1!!**

   l. Click **Next : Disks**.

   m. Click **Next : Networking**.

   n. Click **Allow selected ports**.

   o. For the inbound ports, select **RDP (3389)**.

   p. Click **Review + create**.

   q. Click **Create**.

### Task 2: Disable IE Enhanced Security

> **Note**: Sometimes this image has IE ESC disabled. Sometimes it does not.

1. Login to the newly created VM using RDP and the username and password you supplied earlier.

2. After the VM loads, the Server Manager should open.

3. Select **Local Server**.

   ![Local Server is selected from the Server Manager menu.](media/local-server.png 'Server Manager menu')

4. On the side of the pane, for **IE Enhanced Security Configuration**, if it displays **On** select it.

   ![Screenshot of IE Enhanced Security Configuration, which is set to On.](media/ie-enhanced-security.png 'IE Enhanced Security Configuration')

   - Change to **Off** for Administrators and select **OK**.

   ![In the Internet Explorer Enhanced Security Configuration dialog box, under Administrators, the Off button is selected.](media/ie-enhanced-security-configuration.png 'Internet Explorer Enhanced Security Configuration dialog box')

### Task 3: Install Google Chrome

> **Note**: Some aspects of this lab require the use of Google Chrome. You may find yourself blocked if using Internet Explorer later in the lab.

1. Launch Internet Explorer and download [Google Chrome](https://www.google.com/chrome/).

2. Follow the setup instructions and make sure you can run Chrome to navigate to any webpage.

> **Note**: Chrome is needed for one of the labs as Internet Explorer is not supported for some specific activities.

### Task 4: Validate connectivity to Azure

1. From within the virtual machine, launch Visual Studio and validate that you can log in with your Microsoft Account when prompted.

2. To validate connectivity to your Azure subscription, open **Server Explorer** from the **View** menu, and ensure that you can connect to your Azure subscription.

   ![In Server Explorer, Azure is selected, and its right-click menu displays with options to Refresh, Connect to Microsoft Azure Subscription, Manage and Filter Subscriptions, or Open the Getting Started Page.](media/vs-server-explorer.png 'Server Explorer')

### Task 5: Download the starter files

Download a starter project that includes a payment data generator that sends real-time payment data for processing by your lab solution, as well as data files used in the lab.

1. From your LabVM, download the starter files by downloading a .zip copy of the Cosmos DB real-time advanced analytics GitHub repo.

2. In a web browser, navigate to the [Cosmos DB real-time advanced analytics MCW repo](https://github.com/Microsoft/MCW-Cosmos-DB-Real-Time-Advanced-Analytics).

3. On the repo page, select **Clone or download**, then select **Download ZIP**.

   ![Download .zip containing the repository](media/git-hub-download-repo.png 'Download ZIP')

4. Unzip the contents to the folder **C:\\CosmosMCW\\**.

### Task 6: Download and install Power BI Desktop

Power BI desktop is required to make a connection to your Azure Databricks environment when creating the Power BI dashboard.

1. From your LabVM, download and install [Power BI Desktop](https://powerbi.microsoft.com/desktop/).

### Task 7: Provision a resource group

In this task, you will create an Azure resource group for the resources used throughout this lab.

1. In the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand navigation menu, select **+Add**, and then enter the following in the Create a resource group blade:

   - **Subscription**: Select the subscription you are using for this hands-on lab.

   - **Resource group name**: Enter hands-on-lab-SUFFIX.

   - **Region**: Select the region you would like to use for resources in this hands-on lab. Remember this location so you can use it for the other resources you'll provision throughout this lab.

     ![Add Resource group Resource groups is highlighted in the navigation pane of the Azure portal, +Add is highlighted in the Resource groups blade, and "hands-on-labs" is entered into the Resource group name box on the Create an empty resource group blade.](./media/create-resource-group.png 'Create resource group')

2. Select **Review + create**.

3. On the Summary blade, select **Create** to provision your resource group.

### Task 8: Create a Cosmos DB account

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "cosmos db" into the Search the Marketplace box, select **Azure Cosmos DB** from the results, and then select **Create**.

   ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, cosmos db is entered into the Search the Marketplace box, and Azure Cosmos DB is highlighted in the results.](media/create-resource-cosmos-db.png 'Create an Azure Cosmos DB account')

2. On the Create Cosmos DB blade's basics tab, enter the following:

   - **Subscription**: Select the subscription you are using for this hands-on lab.
   - **Resource Group**: Choose the hands-on-lab-SUFFIX resource group.
   - **Account Name**: Enter a globally unique name (indicated by a green check mark).
   - **API**: Select Core (SQL).
   - **Location**: Select the region you are using for resources in this hands-on lab.
   - **Geo-Redundancy**: Enable.
   - **Multi-region Writes**: Enable.

   ![The Create Cosmos DB blade's basics tab is displayed, with the previously mentioned settings entered into the appropriate fields.](media/create-cosmos-db-blade.png 'Create Cosmos DB')

3. On the Review blade, select **Create**. It can take 5 - 10 minutes to provision your Cosmos DB account. You can move on to the next task while provisioning completes.

### Task 9: Create an Azure Databricks workspace

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource** from the left-hand navigation menu, enter "databricks" into the Search the Marketplace box, select **Azure Databricks** from the results, and then select **Create**.

   ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, "databricks" is entered into the search box, and Azure Databricks is highlighted in the search results.](media/create-resource-azure-databricks.png 'Create Azure Databricks workspace')

2. In the Azure Databricks Service blade, enter the following:

   - **Workspace name**: Enter a unique name, such as cosmosdb-mcw.
   - **Subscription**: Select the subscription you are using for this hands-on lab.
   - **Resource group**: Choose Use existing, and select the hands-on-lab-SUFFIX resource group from the list.
   - **Location**: Select the region you are using for resources in this hands-on lab.
   - **Pricing tier**: Select Premium (+ Role-based access controls).
   - Select **No** under Deploy Azure Databricks workspace in your Virtual Network.

   ![The Azure Databricks Service blade is displayed, with the values specified above entered into the appropriate fields.](media/create-azure-databricks-service.png 'Create Azure Databricks Service')

3. Select **Create**. It can take 5 - 10 minutes to provision your Azure Databricks workspace. You can move on to the next task while provisioning completes.

### Task 10: Set up Azure Key Vault

In this task, you will create an Azure Key Vault account in which you will store secrets such as account keys and connection strings. Key Vault will be used as a backing-store for Azure Databricks secrets to securely access these values from notebooks and libraries.

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "key vault" into the Search the Marketplace box, select **Key Vault** from the results, and then select **Create**.

   ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, key vault is entered into the Search the Marketplace box, and Key Vault is highlighted in the results.](media/create-resource-key-vault.png 'Create an Azure Key Vault account')

2. On the Create storage account blade's **Basics** tab, enter the following:

   - **Name**: Enter a globally unique name (indicated by a green check mark).
   - **Subscription**: Select the subscription you are using for this hands-on lab.
   - **Resource group**: Choose the hands-on-lab-SUFFIX resource group.
   - **Location**: Select the region you are using for resources in this hands-on lab.
   - **Pricing tier**: Select Standard.

   ![The Create Key Vault blade is displayed, with the previously mentioned settings entered into the appropriate fields.](media/create-key-vault-blade.png 'Create Key Vault')

3. Select **Create**.

4. Navigate to your newly provisioned Key Vault account and select **Properties** on the left-hand menu.

5. Copy the **DNS Name** and **Resource ID** property values and paste them to Notepad or some other text application that you can reference later. These values will be used in the next section.

   ![Properties is selected on the left-hand menu, and DNS Name and Resource ID are highlighted to show where to copy the values from.](media/key-vault-properties.png 'Key Vault properties')

### Task 11: Configure Cosmos DB

In this task, you will create an Azure Cosmos DB database and container for ingesting streaming payment data and for serving batch processed data.

1. Navigate to the newly provisioned Azure Cosmos DB account in the Azure portal, then select **Data Explorer** on the left-hand menu.

   ![Data Explorer is selected within the left-hand menu](media/cosmos-db-data-explorer-link.png 'Select Data Explorer')

2. Select **New Collection** in the top toolbar.

   ![The New Collection button is highlighted on the top toolbar](media/new-collection-button.png 'New Collection')

3. In the **Add Collection** blade, configure the following:

   - **Database id**: Select **Create new**, then enter "Woodgrove" for the id.
   - **Provision database throughput**: Unchecked.
   - **Collection id**: Enter "transactions".
   - **Partition key**: Enter "/ipCountryCode".
   - **Throughput**: Enter 15000.
   
   >**Note**: The /ipCountryCode partition was selected because the data will most likely include this value, and it allows us to partition by location from which the transaction originated. This field also contains a wide range of values, which is preferable for partitions.

   ![The Add Collection blade is displayed, with the previously mentioned settings entered into the appropriate fields.](media/cosmos-db-add-collection-blade.png 'Add Collection blade')

4. Select **Firewall and virtual networks** from the left-hand menu and verify that Allow access from **All networks** is selected. If not, select the option then select **Save**. This will allow the payment generator application to send data to your Cosmos DB collection.

   ![The Firewall and virtual networks blade is displayed with the All networks radio button highlighted and selected.](media/cosmos-db-firewall.png 'Firewall and virtual networks blade')

5. Select **Keys** from the left-hand menu.

   ![Keys is selected within the left-hand menu](media/cosmos-db-keys-link.png 'Select Keys')

6. Copy both the **URI** and **Primary Key** values and save to Notepad or similar text editor for later use in the TransactionGenerator console application. You will also insert these values into Key Vault in the next steps.

   ![The Cosmos DB Read-write Keys blade is displayed with highlights around the copy buttons for both URI and Primary Key.](media/cosmos-db-keys.png 'Cosmos DB Read-write Keys')

7.  Open a new browser tab or window and navigate to your Azure Key Vault account in the Azure portal, then select **Secrets** under Settings on the left-hand menu. On the Secrets blade, select **+ Generate/Import** on the top toolbar.

    ![Secrets is highlighted on the left-hand menu, and Generate/Import is highlighted on the top toolbar of the Secrets blade.](media/key-vault-secrets.png 'Key Vault secrets blade')

8.  On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "Cosmos-DB-URI".
    - **Value**: Paste the Azure Cosmos DB URI value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-uri-secret.png 'Create a secret')

9.  Select **Create**.

10. Select **+ Generate/Import** again on the top toolbar to create another secret.

11. On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "Cosmos-DB-Key".
    - **Value**: Paste the Azure Cosmos DB Primary Key value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-key-secret.png 'Create a secret')

12. Select **Create**.

### Task 12: Configure Azure Databricks Key Vault-backed secrets

In this task, you will connect to your Azure Databricks workspace and configure Azure Databricks secrets to use your Azure Key Vault account as a backing store.

1. Return to the [Azure portal](https://portal.azure.com), navigate to the Azure Databricks workspace you provisioned above, and select **Launch Workspace** from the overview blade, signing into the workspace with your Azure credentials, if required.

   ![The Launch Workspace button is displayed on the Databricks Workspace Overview blade.](media/databricks-launch-workspace.png 'Launch Workspace')

2. In your browser's URL bar, append **#secrets/createScope** to your Azure Databricks base URL (for example, <https://eastus.azuredatabricks.net#secrets/createScope>).

3. Enter `key-vault-secrets` for the name of the secret scope.

4. Select **Creator** within the Manage Principal drop-down to specify only the creator (which is you) of the secret scope has the MANAGE permission.

   > MANAGE permission allows users to read and write to this secret scope, and, in the case of accounts on the Azure Databricks Premium Plan, to change permissions for the scope.

   > Your account must have the Azure Databricks Premium Plan for you to be able to select Creator. This is the recommended approach: grant MANAGE permission to the Creator when you create the secret scope, and then assign more granular access permissions after you have tested the scope.

5. Enter the **DNS Name** (for example, <https://woodgrove-vault.vault.azure.net/>) and **Resource ID** you copied earlier during the Key Vault creation step, for example: `/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/resourcegroups/hands-on-lab/providers/Microsoft.KeyVault/vaults/woodgrove-vault`.

   ![Create Secret Scope form](media/create-secret-scope.png 'Create Secret Scope')

6. Select **Create**.

After a moment, you will see a dialog verifying that the secret scope has been created.

### Task 13: Provision Event Hubs

In this task, you will create an Event Hubs namespace and add an Event Hub within for ingesting streaming payment data.

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "event hubs" into the Search the Marketplace box, select **Event Hubs** from the results, and then select **Create**.

   ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, event hubs is entered into the Search the Marketplace box, and Event Hubs is highlighted in the results.](media/create-resource-event-hubs.png 'Create an Event Hubs namespace')

2. On the Create Namespace blade, enter the following:

   - **Name**: Enter a globally unique name (indicated by a green check mark).
   - **Pricing tier**: Select Standard.
   - **Enable Kafka**: Unchecked
   - **Make this namespace zone redundant**: Unchecked
   - **Subscription**: Select the subscription you are using for this hands-on lab.
   - **Resource group**: Choose the hands-on-lab-SUFFIX resource group.
   - **Location**: Select the region you are using for resources in this hands-on lab.
   - **Throughput Units**: Set the slider all the way to the left, setting the value to 1.
   - **Enable Auto-Inflate**: Unchecked

   ![The Create Namespace blade is displayed, with the previously mentioned settings entered into the appropriate fields.](media/create-event-hubs-blade.png 'Create Namespace')

3. Select **Create**.

4. Navigate to the newly provisioned Event Hubs namespace in the Azure portal, then select **Event Hubs** under Entities on the left-hand menu.

   ![Event Hubs is selected within the left-hand menu](media/select-event-hubs.png 'Select Event Hubs')

5. Select **+ Event Hub** in the top toolbar.

   ![Select the + Event Hub button in the top toolbar](media/add-event-hub-button.png 'Add Event Hub')

6. In the **Create Event Hub** blade, configure the following:

   - **Name**: Enter "transactions".
   - **Partition Count**: Move the slider to set the value to 10.
   - **Message Retention**: Leave at 1.
   - **Capture**: Off

   ![The Create Event Hub blade is displayed, with the previously mentioned settings entered into the appropriate fields](media/create-event-hub-blade.png 'Create Event Hub')

7. Select **Create**.

8. After the new Event Hub is created, select it then select **Shared access policies** under Settings in the left-hand menu.

   ![Shared access policies is selected within the left-hand menu](media/select-shared-access-policies.png 'Select Shared access policies')

9. Select **+ Add** in the top toolbar.

   ![Select the + Add button in the top toolbar](media/add-shared-access-policy.png 'Add Shared Access Policy')

10. In the **Add SAS Policy** blade, configure the following:

    - **Policy name**: Enter "Sender".
    - **Manage**: Unchecked
    - **Send**: Checked
    - **Listen**: Unchecked

    ![The Add SAS Plicy is displayed, with the previously mentioned settings entered into the appropriate fields](media/add-sas-policy-sender.png 'Add SAS Policy')

11. Select **Create**.

12. Select **+ Add** in the top toolbar to add another policy.

    ![Select the + Add button in the top toolbar](media/add-shared-access-policy.png 'Add Shared Access Policy')

13. In the **Add SAS Policy** blade, configure the following:

    - **Policy name**: Enter "Listener".
    - **Manage**: Unchecked
    - **Send**: Unchecked
    - **Listen**: Checked

    ![The Add SAS Plicy is displayed, with the previously mentioned settings entered into the appropriate fields](media/add-sas-policy-listener.png 'Add SAS Policy')

14. Select **Create**.

15. Select the **Sender** access policy.

16. Copy the **Connection string-primary key** value. Save this value for the Sender policy in Notepad or similar for later.

    ![The button to copy the primary connection string for the sender policy is highlighted.](media/copy-sender-policy-key.png 'Sender policy primary connection string')

### Task 14: Create an Azure Data Lake Storage Gen2 account

In this task, you will create an Azure Data Lake Storage Gen2 (ADLS Gen2) account, which will be used as the repository for the Databricks Delta tables you will be creating in this hands-on lab.

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "storage account" into the Search the Marketplace box, select **Storage account - blob, file, table, queue** from the results, and then select **Create**.

   ![Create a resource is highlighted in the left-hand navigation menu of the Azure portal, storage account is entered into the Search the Marketplace box, and Storage account - blob, file, table, queue is highlighted in the results.](media/create-resource-adls-gen2.png 'Create an Azure Data Lake Storage Gen2 account')

2. On the Create storage account blade's **Basics** tab, enter the following:

   - **Subscription**: Select the subscription you are using for this hands-on lab.
   - **Resource group**: Choose the hands-on-lab-SUFFIX resource group.
   - **Storage account name**: Enter a globally unique name (indicated by a green check mark).
   - **Location**: Select the region you are using for resources in this hands-on lab.
   - **Performance**: Select Standard.
   - **Account kind**: Select StorageV2 (general purpose v2).
   - **Replication**: Choose Locally-redundant storage (LRS).
   - **Access tier (default)**: Select Hot.

   ![The Create storage account blade's Basics tab is displayed, with the previously mentioned settings entered into the appropriate fields.](media/create-storage-account-basics.png 'New Data Lake Storage Gen2')

3. Select **Next : Advanced >** to move on to the **Advanced** tab.

4. On the **Advanced** tab, set the Hierarchical namespace option to **Enabled** under Data Lake Storage Gen2, and then select **Review + create**

   ![The Create storage account blade's Advanced tab is displayed, with Enabled selected and highlighted next to Hierarchical namespace under Data Lake Storage Gen2.](media/create-storage-account-advanced.png 'Enable Hierarchical namespace')

5. On the **Review + create** tab, ensure the **Validation passed** message is displayed, and select **Create** to provision the new ADLS Gen2 instance.

   ![The Create storage account blade's Review + create tab is displayed, with the validation passed message present.](media/create-storage-account-review.png 'Review and create storage account')

6. Navigate to the newly provisioned ADLS Gen2 account in the Azure portal, then select **Access keys** under Settings on the left-hand menu. You are going to copy the **Storage account name** and **Key** values and add them as secrets in your Key Vault account.

   ![The storage account Access keys blade is displayed, with the storage account name highlighted.](media/storage-account-access-keys.png 'Storage account access keys')

7. Open a new browser tab or window and navigate to your Azure Key Vault account in the Azure portal, then select **Secrets** under Settings on the left-hand menu. On the Secrets blade, select **+ Generate/Import** on the top toolbar.

   ![Secrets is highlighted on the left-hand menu, and Generate/Import is highlighted on the top toolbar of the Secrets blade.](media/key-vault-secrets.png 'Key Vault secrets blade')

8. On the Create a secret blade, enter the following:

   - **Upload options**: Select Manual.
   - **Name**: Enter "ADLS-Gen2-Account-Name".
   - **Value**: Paste the Storage account name value you copied in an earlier step.

   ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-adls-gen2-account-name-secret.png 'Create a secret')

9. Select **Create**.

10. Select **+ Generate/Import** again on the top toolbar to create another secret.

11. On the Create a secret blade, enter the following:

    - **Upload options**: Select Manual.
    - **Name**: Enter "ADLS-Gen2-Account-Key".
    - **Value**: Paste the Storage account Key value you copied in an earlier step.

    ![The Create a secret blade is displayed, with the previously mentioned values entered into the appropriate fields.](media/key-vault-create-adls-gen2-account-key-secret.png 'Create a secret')

12. Select **Create**.

### Task 15: Create an Azure Databricks cluster

In this task, you will connect to your Azure Databricks workspace and create a cluster to use for this hands-on lab.

1. Return to the [Azure portal](https://portal.azure.com), navigate to the Azure Databricks workspace you provisioned above, and select **Launch Workspace** from the overview blade, signing into the workspace with your Azure credentials, if required.

   ![The Launch Workspace button is displayed on the Databricks Workspace Overview blade.](media/databricks-launch-workspace.png 'Launch Workspace')

2. Select **Clusters** from the left-hand navigation menu, and then select **+ Create Cluster**.

   ![The Clusters option in the left-hand menu is selected and highlighted, and the Create Cluster button is highlighted on the clusters page.](media/databricks-clusters.png 'Databricks Clusters')

3. On the Create Cluster screen, enter the following:

   - **Cluster Name**: Enter a name for your cluster, such as lab-cluster.
   - **Cluster Mode**: Select Standard.
   - **Databricks Runtime Version**: Select Runtime: 5.2 (Scala 2.11, Spark 2.4.0).
   - **Python Version**: Select 3.
   - **Enable autoscaling**: Ensure this is checked.
   - **Terminate after XX minutes of inactivity**: Leave this checked, and the number of minutes set to 120.
   - **Worker Type**: Select Standard_DS4_v2.
     - **Min Workers**: Leave set to 2.
     - **Max Workers**: Leave set to 8.
   - **Driver Type**: Set to Same as worker.
   - Expand Advanced Options and enter the following into the Spark Config box:

   ```bash
   spark.databricks.delta.preview.enabled true
   ```

   ![The Create Cluster screen is displayed, with the values specified above entered into the appropriate fields.](media/databricks-create-new-cluster.png 'Create a new Databricks cluster')

4. Select **Create Cluster**. It will take 3-5 minutes for the cluster to be created and started.

### Task 16: Open Azure Databricks and load lab notebooks

In this task, you will import the notebooks contained in the [Cosmos DB real-time advanced analytics MCW GitHub repo](https://github.com/Microsoft/MCW-Cosmos-DB-Real-Time-Advanced-Analytics) into your Azure Databricks workspace.

1. Navigate to your Azure Databricks workspace in the Azure portal, and select **Launch Workspace** from the overview blade, signing into the workspace with your Azure credentials, if required.

   ![The Launch Workspace button is displayed on the Databricks Workspace Overview blade.](media/databricks-launch-workspace.png 'Launch Workspace')

2. Select **Workspace** from the left-hand menu, then select **Users** and select your user account (email address), and then select the down arrow on top of your user workspace and select **Import** from the context menu.

   ![The Workspace menu is highlighted in the Azure Databricks workspace, and Users is selected with the current user's account selected and highlighted. Import is selected in the user's context menu.](media/databricks-workspace-import.png 'Import files into user workspace')

3. Within the Import Notebooks dialog, select **URL** for Import from, and then paste the following into the box: `https://github.com/Microsoft/MCW-Cosmos-DB-real-time-advanced-analytics/blob/master/Hands-on%20lab/lab-files/CosmosDbAdvancedAnalytics.dbc`

   ![The Import Notebooks dialog is displayed](media/databricks-import-notebooks.png 'Import Notebooks dialog')

4. Select **Import**.

5. You should now see a folder named **CosmosDbAdvancedAnalytics** in your user workspace. This folder contains all of the notebooks you will use throughout this hands-on lab.

You should follow all steps provided _before_ performing the Hands-on lab.
