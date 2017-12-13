namespace ContosoHelpdeskChatBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using ContosoHelpdeskChatBot.Models;

    [Serializable]
    public class InstallAppDialog : IDialog<object>
    {
        private Models.App app = new App();

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Ok let's get started. What is the name of the application? ");

            context.Wait(appNameAsync);
        }

        private async Task appNameAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            //in this case we are waiting for an app name which will be used as keyword to search the AppMsi table
            var message = await userReply;
            
        }

        private async Task multipleAppsAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            //in this case we are waiting for an app name which will be used as keyword to search the AppMsi table
            var message = await userReply;

        }

        private async Task machineNameAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            //in this case we are waiting for an app name which will be used as keyword to search the AppMsi table
            var message = await userReply;

            //we don't need to do much here for now but in the next PBI we will add saving to database

            await context.PostAsync($"Great, your request to install {this.app.Name} on {this.app.Machine} has been scheduled.");
            context.Done<object>(null);
        }

        private async Task<List<string>> GetAppsAsync(string Name)
        {
            //TODO: Add list of dummy app names
            //we will switch to using Entity Framework in the next PBI to lookup list from a table
            var names = new List<string>();

            return names;
        }
    }
}