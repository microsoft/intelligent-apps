using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using Microsoft.Bot.Builder;
//TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
//using ContosoHelpdeskChatBot.Models;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class InstallAppDialog : WaterfallDialog
    {
        //TODO: Uncomment after adding log4net Nuget package
        //private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
        //private Models.InstallApp install = new InstallApp();
        List<string> names = new List<string>();
        public static string dialogId = "InstallAppDialog";

        public InstallAppDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(greetingStepAsync);
            AddStep(responseConfirmStepAsync);
            AddStep(multipleAppsStepAsync);
            AddStep(multipleAppsStepAsync); // Added twice for dialog path where multiple applications are returned to user, and user must select one
        }

        private static async Task<DialogTurnResult> greetingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text("Ok let's get started. What is the name of the application?") }, cancellationToken);
        }

        private async Task<DialogTurnResult> responseConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var appName = (string)stepContext.Result;
            stepContext.Values["AppName"] = appName;

            names = await this.getAppsAsync(appName);

            if (names.Count == 1)
            {
                //TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
                //install.AppName = names.First();
                //return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text($"Found {install.AppName}. What is the name of the machine to install application?") }, cancellationToken);
                return null;
            }
            else if (names.Count > 1)
            {
                string appnames = "";
                for (int i = 0; i < names.Count; i++)
                {
                    appnames += $"<br/>&nbsp;&nbsp;&nbsp;{i + 1}.&nbsp;" + names[i];
                }
                return await stepContext.PromptAsync("promptNumber", new PromptOptions { Prompt = MessageFactory.Text($"I found {names.Count()} applications.<br/> {appnames}<br/> Please reply 1 - {names.Count()} to indicate your choice.") }, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Sorry, I did not find any application with the name \"{appName}\".");
                return await stepContext.EndDialogAsync();
            }
        }


        private async Task<DialogTurnResult> multipleAppsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Check to see if user selected app when there are multiple applications to pick from
            if (stepContext.Values.ContainsKey("AppSelected"))
                names.Clear();
            else
                names = await this.getAppsAsync(stepContext.Values["AppName"].ToString());

            // If multiple apps returned then need to prompt user for which one to select
            if (names.Count > 1)
            {
                int choice = (int)stepContext.Result;

                if (choice <= names.Count && choice > 0)
                {
                    //TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
                    //minus becoz index zero base
                    //stepContext.Values["AppName"] = names[choice - 1];
                    stepContext.Values["AppSelected"] = true;
                    return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text($"What is the name of the machine to install?") }, cancellationToken);
                }
                else
                {
                    return await stepContext.PromptAsync("promptNumber", new PromptOptions { Prompt = MessageFactory.Text($"Invalid response. Please reply 1 - {names.Count()} to indicate your choice.") }, cancellationToken);
                }
            }
            else
            {
                // Proceed with saving entry into database
                var machineName = (string)stepContext.Result;

                //TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
                //Models.InstallApp install = new InstallApp();
                //install.AppName = (string)stepContext.Values["AppName"];
                //install.MachineName = machineName;

                //TODO: Save to database

                //TODO: Uncomment after adding log4net Nuget package
                //logger.Info("InstallAppDialog: AppName = " + install.AppName + "; MachineName = " + install.MachineName);

                //TODO: Uncomment after adding Entity Framework entities from ContosoHelpdeskData database
                //await stepContext.Context.SendActivityAsync($"Great, your request to install {install.AppName} on {install.MachineName} has been scheduled.");
                return await stepContext.EndDialogAsync();
            }
        }

        private async Task<List<string>> getAppsAsync(string Name)
        {
            //TODO: Add EF to lookup database
            var names = new List<string>();

            return names;
        }
    }
}