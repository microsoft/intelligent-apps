### February 2019 Updates
**Contoso Helpdesk Chat Bot**
* Bot framework updated from version 3 to version 4.
	- updated packages: Microsoft.Bot.Builder to v4.2.2
	- added packages: Microsoft.Bot.Builder.Dialogs, Microsoft.Bot.Builder.Integration.AspNet.WebApi, Microsoft.Bot.Configuration

**Fabrikam Investment Bank Customer Service**
* Bot framework updated from version 3 to version 4.
	- updated packages: Microsoft.Bot.Builder to v4.2.2
	- added packages: Microsoft.Bot.Builder.AI.Luis, Microsoft.Bot.Builder.Dialogs, Microsoft.Bot.Builder.Integration.AspNet.WebApi, Microsoft.Bot.Configuration
* Speech API updated from version 0.6 to version 1.2.

# Activate Azure with Intelligent Apps
There are five proof of concept applications in this repo written to illustrate how to augment existing applications with Microsoft Cognitive Services and Bot Framework to add intelligence as well as using other services in the Azure platform.

**Adatum Corporation Knowledge Service**
* Demonstrate how to enhance website to answer questions with natural language based on existing list of FAQ questions & answers. 
* Uses QnA Maker API & Azure SQL

**Alpine Ski House Happiness Meter**
* Simple greetings app to evaluate the satisfaction of arriving visitors by analyzing their emotions.
* Uses Emotion API

**Contoso Helpdesk Chat Bot**
* Employee helpdesk solution to enable self-service app install, password reset and local admin elevation with a bot. 
* Uses Bot Framework (Email Channel) & Azure SQL

**Fabrikam Investment Bank Customer Service**
* Customer facing bot for the account balance information inquiry through simulated Interactive Voice Response.
* Uses Bot Framework (DirectLine Channel), LUIS & Speech API

**Woodgrove Bank Enhanced ATM Security**
* Demonstration of an enhanced ATM authentication through face recognition API.
* Uses Face API (REST) & Azure Blob Storage


# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
