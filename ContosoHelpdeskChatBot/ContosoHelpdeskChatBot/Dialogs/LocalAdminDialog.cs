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

            //TODO: prompt the user
            return null;
        }

        private async Task<DialogTurnResult> ResponseConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //TODO: get user response and store it

            //TODO: prompt user
            return null;
        }

        private async Task<DialogTurnResult> finalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //TODO: get user response and validate

            //TODO: save data to localAdmin table
            return null;
        }
    }
}