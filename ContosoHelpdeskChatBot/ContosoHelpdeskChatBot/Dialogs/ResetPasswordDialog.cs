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
    [Serializable]
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
                return await stepContext.PromptAsync("promptNumber", new PromptOptions { Prompt = MessageFactory.Text("Please provide four digit pass code") }, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync("Failed to send SMS. Make sure email & phone number has been added to database.");
                return await stepContext.EndDialogAsync();
            }
        }

        private async Task<DialogTurnResult> PasscodeReceivedStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int result;
            bool isNum = int.TryParse(stepContext.Context.Activity.Text, out result);
            if (isNum)
            {
                var email = stepContext.Context.Activity.From.Id;
                int? passcode;

                using (var db = new ContosoHelpdeskContext())
                {
                    passcode = db.ResetPasswords.Where(r => r.EmailAddress == email).First().PassCode;
                }

                if (result == passcode)
                {
                    string temppwd = "TempPwd" + new Random().Next(0, 5000);
                    await stepContext.Context.SendActivityAsync($"Your temp password is {temppwd}");
                    return await stepContext.EndDialogAsync();
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

            //Recipient Id varies depending on channel
            //refer ChannelAccount class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dd/def/class_microsoft_1_1_bot_1_1_connector_1_1_channel_account.html#a0b89cf01fdd73cbc00a524dce9e2ad1a
            //as well as Activity class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dc/d2f/class_microsoft_1_1_bot_1_1_connector_1_1_activity.html
            var email = context.Context.Activity.From.Id;
            int passcode = new Random().Next(1000, 9999);
            Int64? smsNumber = 0;
            string smsMessage = "Your Contoso Pass Code is ";
            string countryDialPrefix = "+1";

            //save PassCode to database
            using (var db = new ContosoHelpdeskContext())
            {
                var reset = db.ResetPasswords.Where(r => r.EmailAddress == email).ToList();
                if (reset.Count >= 1)
                {
                    reset.First().PassCode = passcode;
                    smsNumber = reset.First().MobileNumber;
                    result = true;
                }
                
                db.SaveChanges();
            }

            if (result)
            {
                result = Helper.SendSms($"{countryDialPrefix}{smsNumber.ToString()}", $"{smsMessage} {passcode}");
            }

            return result;
        }
    }
}