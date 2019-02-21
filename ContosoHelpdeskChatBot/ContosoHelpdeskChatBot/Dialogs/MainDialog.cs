using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class MainDialog : WaterfallDialog
    {
        //TODO: add in variables
        public static string dialogId = "MainDialog";


        public MainDialog(string dialogId) : base(dialogId)
        {
            AddStep(GreetingStepAsync);
            AddStep(ChoiceSelectedStepAsync);
        }

        private static async Task<DialogTurnResult> GreetingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //TODO: uncomment and pass in the needed arguments to the function
            //return await stepContext.PromptAsync(/*Fill In*/);
            return null;
        }

        private static async Task<DialogTurnResult> ChoiceSelectedStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var optionSelected = (stepContext.Result as FoundChoice)?.Value;

            //TODO: handle option selected

            return await stepContext.NextAsync();
        }
    }
}