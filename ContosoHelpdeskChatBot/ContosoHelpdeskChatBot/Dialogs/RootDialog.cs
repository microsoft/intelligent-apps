namespace ContosoHelpdeskChatBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> userReply)
        {
            //this will trigger a wait for user's reply
            var message = await userReply;


        }

        private void ShowOptions(IDialogContext context)
        {
            
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> userReply)
        {
            try
            {
                //this will trigger a wait for user's reply
                string optionSelected = await userReply;

                
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> userReply)
        {
            try
            {
                //this will trigger a wait for user's reply
                var message = await userReply;

                var ticketNumber = new Random().Next(0, 20000);
                await context.PostAsync($"Thank you for using the Helpdesk Bot. Your ticket number is {ticketNumber}.");
                context.Done(ticketNumber);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
        }
    }
}