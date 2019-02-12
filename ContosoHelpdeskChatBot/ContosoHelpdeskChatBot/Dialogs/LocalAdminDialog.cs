using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ContosoHelpdeskChatBot.Models;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using System.Threading;

namespace ContosoHelpdeskChatBot.Dialogs
{
    [Serializable]
    public class LocalAdminDialog : WaterfallDialog
    {
        private LocalAdmin admin = new LocalAdmin();
        public static string dialogId = "LocalAdminDialog";

        public LocalAdminDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(GreetingStepAsync);
            AddStep(ResponseConfirmStepAsync);
            AddStep(finalStepAsync);
        }


        private static async Task<DialogTurnResult> GreetingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"Great! I will help you request local machine admin.");
            return await stepContext.PromptAsync("promptText", new PromptOptions { Prompt = MessageFactory.Text("What is the machine name to add you to local admin group?") }, cancellationToken);
        }

        private async Task<DialogTurnResult> ResponseConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var MachineName = (string)stepContext.Result;
            stepContext.Values["MachineName"] = MachineName;

            return await stepContext.PromptAsync("promptNumber", new PromptOptions { Prompt = MessageFactory.Text("How many days do you need the admin access?") }, cancellationToken);
        }

        private async Task<DialogTurnResult> finalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int days = (int)stepContext.Result;

            if (days >= 0)
            {
                stepContext.Values["AdminDuration"] = days;
                admin.AdminDuration = (int)stepContext.Values["AdminDuration"];
                admin.MachineName = (string)stepContext.Values["MachineName"];

                using (var db = new ContosoHelpdeskContext())
                {
                    db.LocalAdmins.Add(admin);
                    db.SaveChanges();
                }

                var ticketNumber = new Random().Next(0, 20000);
                await stepContext.Context.SendActivityAsync($"Thank you for using the Helpdesk Bot. Your ticket number is {ticketNumber}.");
                return await stepContext.EndDialogAsync();
            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Invalid response provided.  Ending dialog.");
                return await stepContext.EndDialogAsync();
            }
        }
    }
}