![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Cosmos DB real-time advanced analytics
</div>

<div class="MCWHeader2">
Before the hands-on lab setup guide
</div>

<div class="MCWHeader3">
May 2019
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
    - [Task 8: Deploy Environment](#task-8-deploy-environment)

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

1. Create a virtual machine (VM) in Azure using the **Visual Studio Community 2017 (latest release) on Windows Server 2016 (x64)** image. A Windows 10 image will work as well.

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

4. Unzip the contents to the folder `C:\CosmosMCW\`.

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

### Task 8: Deploy Environment

1. In the Azure Portal, navigate to **Azure Active Directory**, then select **Users** under Manage.

    ![Azure Active Directory blade with Users highlighted](media/deploy-azure-ad-users-link.png "Azure Active Directory blade with Users highlighted")

2. Select the user your currently logged into the Azure Portal with.

    ![All users list with user highlighted](media/deploy-azure-ad-user-list.png "All users list with user highlighted")

3. On the **User** blade, copy the **Object ID** for this user.

    ![User blade is shown with Object ID highlighted](media/deploy-azure-ad-user-object-id.png "User blade is shown with Object ID highlighted")

4. Select **Create a resource**, then search for and select **Template Deployment**.

    ![Create a resource is highlighted as step 1, and the search box is highlighted a step 2 with template deployment entered](media/deploy-create-resource-search-template-deployment.png "Create a resource is highlighted as step 1, and the search box is highlighted a step 2 with template deployment entered")

5. On the **Custom deployment** blade, select the **Build your own template in the editor** link.

    ![](media/deploy-deployment-build-your-own-link.png)

6. On the **Edit template** blade, click the **Load file** button to select the ARM Template to use.

    ![The Load file button is highlighted](media/deploy-deployment-load-file-button.png "The Load file button is highlighted")

7. Select the `C:\CosmosMCW\Deployment\environment-template.json` ARM Template.

8. Click **Save**.

    ![The Save button is highlighted](media/deploy-deployment-arm-template-save.png "The Save button is highlighted")

9. Enter the following values:

    - Resource group: **select the Resource Group created previously for the lab**
    - Key Vault Access Policy User Object Id: **paste in the User Object ID that was copied previously from Azure Active Directory**

    ![The parameters specified are highlighted](media/deploy-deployment-parameter-fields.png "The parameters specified are highlighted")

10. Check the **I agree...** check box, then click **Purchase**.

    ![The I agree checkbox and the purchase button are highlighted](media/deploy-deployment-purchase-button.png "The I agree checkbox and the purchase button are highlighted")

11. The deployment will likely take 5 - 10 minutes to deploy.

    ![The deployment progress is shown](media/deploy-deployment-progress.png "The deployment progress is shown")

You should follow all steps provided _before_ performing the Hands-on lab.
