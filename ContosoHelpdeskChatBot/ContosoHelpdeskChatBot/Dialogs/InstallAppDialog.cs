using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using Microsoft.Bot.Builder;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class InstallAppDialog : WaterfallDialog
    {
        private Models.App install = new Models.App();
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
            names = await this.getAppsAsync(appName);

            //TODO: store appName


            //TODO: implement logic based of number of names
            return null;
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
                //TODO: validate user input corresponds to a valid choice and prompt user for info
                return null;
            }
            else
            {
                //TODO:get user's response and initialize a new InstallApp object

                //we don't need to do much here for now but in the next PBI we will add saving to database

                //TODO: uncomment to notify user it was successfull
                //await stepContext.Context.SendActivityAsync($"Great, your request to install {install.AppName} on {install.MachineName} has been scheduled.");
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