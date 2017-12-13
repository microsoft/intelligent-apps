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

            var appname = message.Text;
            var names = await this.GetAppsAsync(appname);

            if (names.Count == 1)
            {
                app.Name = names.First();
                await context.PostAsync($"Found {app.Name}. What is the name of the machine to install application?");
                context.Wait(machineNameAsync);
            }
            else if (names.Count > 1)
            {
                string appnames = "";
                for (int i = 0; i < names.Count; i++)
                {
                    appnames += $"<br/>&nbsp;&nbsp;&nbsp;{i + 1}.&nbsp;" + names[i];
                }
                await context.PostAsync($"I found {names.Count()} applications.<br/> {appnames}<br/> Please reply 1 - {names.Count()} to indicate your choice.");

                //at a conversation scope, store state data in ConversationData
                context.ConversationData.SetValue("AppList", names);
                context.Wait(multipleAppsAsync);
            }
            else
            {
                await context.PostAsync($"Sorry, I did not find any application with the name \"{appname}\".");
                context.Done<object>(null);
            }
        }

        private async Task multipleAppsAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            //in this case we are waiting for an app name which will be used as keyword to search the AppMsi table
            var message = await userReply;

            int choice;
            var isNum = int.TryParse(message.Text, out choice);
            List<string> applist;
           
            context.ConversationData.TryGetValue("AppList", out applist);

            if (isNum && choice <= applist.Count && choice > 0)
            {
                //minus becoz index zero base
                this.app.Name = applist[choice - 1];
                await context.PostAsync($"What is the name of the machine to install application?");
                context.Wait(machineNameAsync);
            }
            else
            {
                await context.PostAsync($"Invalid response. Please reply 1 - {applist.Count()} to indicate your choice.");
                context.Wait(multipleAppsAsync);
            }
        }

        private async Task machineNameAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            //in this case we are waiting for an app name which will be used as keyword to search the AppMsi table
            var message = await userReply;

            var machinename = message.Text;

            this.app.Machine = machinename;

            //we don't need to do much here for now but in the next PBI we will add saving to database

            await context.PostAsync($"Great, your request to install {this.app.Name} on {this.app.Machine} has been scheduled.");
            context.Done<object>(null);
        }

        private async Task<List<string>> GetAppsAsync(string Name)
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