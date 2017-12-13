using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using ContosoHelpdeskChatBot.Models;
using ContosoHelpdeskSms;

namespace ContosoHelpdeskChatBot.Dialogs
{
    [Serializable]
    public class ResetPasswordDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Alright I will help you create a temp password");

            //TODO: Use form builder to prompt for pass code
            if (sendPassCode(context))
            {
                //call form builder & resume ResumeAfterResetPasswordFormDialog

            }
            else
            {
                //here we can simply fail the current dialog because we have root dialog handling all exceptions
                context.Fail(new Exception("\n\nCannot send SMS. This feature only works in email channel.\n\nAlso, make sure email & phone number has been added to database."));
            }
        }

        private bool sendPassCode(IDialogContext context)
        {
            bool result = false;

            //TODO: Use context.Activity.From.Id to match email stored in database
            //From Id varies depending on channel
            //refer ChannelAccount class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dd/def/class_microsoft_1_1_bot_1_1_connector_1_1_channel_account.html#a0b89cf01fdd73cbc00a524dce9e2ad1a
            //as well as Activity class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dc/d2f/class_microsoft_1_1_bot_1_1_connector_1_1_activity.html


            //TODO: Save PassCode to database


            //TODO: Send SMS to mobile number looked up from ResetPassword table

            return result;
        }

        private async Task ResumeAfterResetPasswordFormDialog(IDialogContext context, IAwaitable<ResetPasswordPrompt> userReply)
        {
            var prompt = await userReply;

            //TODO: Lookup ResetPassword table and generate temporary password if pass code matches


            context.Done<object>(null);
        }
    }
}