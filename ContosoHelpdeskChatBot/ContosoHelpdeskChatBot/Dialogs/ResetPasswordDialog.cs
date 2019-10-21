using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ContosoHelpdeskChatBot.Models;
using ContosoHelpdeskSms;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Bot.Builder;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class ResetPasswordDialog : WaterfallDialog
    {
        public static string dialogId = "ResetPasswordDialog";

        public ResetPasswordDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(GreetingStepAsync);
            AddStep(PasscodeReceivedStepAsync);
        }


        private async Task<DialogTurnResult> GreetingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Alright I will help you create a temp password");
            if (sendPassCode(stepContext))
            {
                //TODO: prompt user
                return null;
            }
            else
            {
                //TODO: notify user and end dialog
                return null;
            }
        }

        private async Task<DialogTurnResult> PasscodeReceivedStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int result;
            bool isNum = int.TryParse(stepContext.Context.Activity.Text, out result);
            if (isNum)
            {
                var email = stepContext.Context.Activity.From.Id;
                int? passcode = null;

                //TODO: Lookup ResetPassword table and generate temporary password if pass code matches

                if (result == passcode)
                {
                    //TODO: Create temporary password and store in database
                }
                else
                {
                    await stepContext.Context.SendActivityAsync($"Incorrect passcode.");
                    return await stepContext.EndDialogAsync();
                }
            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Invalid passcode.");
                return await stepContext.EndDialogAsync();
            }
        }

        private bool sendPassCode(WaterfallStepContext context)
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
    }
}