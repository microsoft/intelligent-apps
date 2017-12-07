using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace FabrikamCustomerServiceBot.Dialogs
{
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        public RootLuisDialog(ILuisService luis) : base(luis)
        { }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I didn't understand. Try again.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Welcome to Fabrikam Investment Customer Service. I can understand phrases and full sentences. Now how can I help you?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("CheckingAccountBalance")]
        public async Task CheckingAccountBalance(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var balance = new Random().Next(0, 1000000);
            await context.PostAsync($"Your checking account balance is {balance} dollars");
            context.Wait(MessageReceived);
        }
    }
}