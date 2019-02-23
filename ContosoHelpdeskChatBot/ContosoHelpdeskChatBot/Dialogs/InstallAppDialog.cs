using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using Microsoft.Bot.Builder;
using ContosoHelpdeskChatBot.Models;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class InstallAppDialog : WaterfallDialog
    {
        private App install = new App();
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
                install.AppName = names.First();
                return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text($"Found {install.AppName}. What is the name of the machine to install application?") }, cancellationToken);
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
                    //minus becoz index zero base
                    stepContext.Values["AppSelected"] = true;
                    stepContext.Values["AppName"] = stepContext.Values["AppName"].ToString() + choice;
                    return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text($"What is the name of the machine to install?") }, cancellationToken);
                }
                else
                {
                    return await stepContext.PromptAsync("promptNumber", new PromptOptions { Prompt = MessageFactory.Text($"Invalid response. Please reply 1 - {names.Count()} to indicate your choice.") }, cancellationToken);
                }
            }
            else
            {
                var machineName = (string)stepContext.Result;

                App install = new App();
                install.AppName = (string)stepContext.Values["AppName"];
                install.MachineName = machineName;
                stepContext.Values["MachineName"] = machineName;

                //we don't need to do much here for now but in the next PBI we will add saving to database

                await stepContext.Context.SendActivityAsync($"Great, your request to install {install.AppName} on {install.MachineName} has been scheduled.");
                return await stepContext.EndDialogAsync();
            }
        }

        private async Task<List<string>> getAppsAsync(string Name)
        {
            //TODO: Add list of dummy app names
            //we will switch to using Entity Framework in the next PBI to lookup list from a table
            var names = new List<string>();
            names.Add(Name + " 1");
            names.Add(Name + " 2");

            return names;
        }
    }
}