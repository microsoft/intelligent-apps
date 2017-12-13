using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FabrikamCustomerServiceBot.Dialogs
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            
            // return our reply to the user
            await context.PostAsync($"You said {activity.Text}");

            context.Wait(MessageReceivedAsync);
        }
    }
}