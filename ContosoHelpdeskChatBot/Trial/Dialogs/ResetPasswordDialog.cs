using ContosoHelpdeskSms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trial.Models;

namespace Trial.Dialogs
{
    public class ResetPasswordDialog : WaterfallDialog
    {

        public ResetPasswordDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            //request for passcode
            AddStep(async (stepContext, cancellationToken) =>
            {

                await stepContext.Context.SendActivityAsync($"Alright! I will help you create a temp password.");

                var sendSMS = sendPassCode(stepContext);

                if (sendSMS)
                {
                    return await stepContext.PromptAsync("numberPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Please provide four digit pass code")
                    });
                }
                else
                {
                await stepContext.Context.SendActivityAsync($"Failed to send SMS. Make sure email & phone number has been added to database.");
                return await stepContext.EndDialogAsync();
                }
            });

            //passcode received
            AddStep(async (stepContext, cancellationToken) =>
            {
                int result = 0;
                bool checkpasscode = int.TryParse(stepContext.Context.Activity.Text, out result);

                if (checkpasscode)
                {
                    result = int.Parse(stepContext.Context.Activity.Text);
                    var email = stepContext.Context.Activity.From.Id;
                    int? passcode;

                    using (var db = new ContosoHelpdeskContext(new DbContextOptions<ContosoHelpdeskContext>()))
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
                        await stepContext.Context.SendActivityAsync($"Passcodes are not matching!");
                        return await stepContext.EndDialogAsync();
                    }
                }
                else
                {
                    await stepContext.Context.SendActivityAsync($"Invalid passcode!");
                    return await stepContext.EndDialogAsync();
                }
               
            });

        }

        public static string Id => "resetPasswordDialog";
        public static ResetPasswordDialog Instance { get; } = new ResetPasswordDialog(Id);

        private bool sendPassCode(WaterfallStepContext stepContext)
        {
            bool result = false;

            //Recipient Id varies depending on channel
            //refer ChannelAccount class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dd/def/class_microsoft_1_1_bot_1_1_connector_1_1_channel_account.html#a0b89cf01fdd73cbc00a524dce9e2ad1a
            //as well as Activity class https://docs.botframework.com/en-us/csharp/builder/sdkreference/dc/d2f/class_microsoft_1_1_bot_1_1_connector_1_1_activity.html
            var email = stepContext.Context.Activity.From.Id;
            int passcode = new Random().Next(1000, 9999);
            Int64? smsNumber = 0;
            string smsMessage = "Your Contoso Pass Code is ";
            string countryDialPrefix = "+1";

            //save PassCode to database
            using (var db = new ContosoHelpdeskContext(new DbContextOptions<ContosoHelpdeskContext>()))
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
