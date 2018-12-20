using ContosoHelpdeskChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class LocalAdminDialog : WaterfallDialog
    {
        private LocalAdmin admin = new LocalAdmin();

        public LocalAdminDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            //request for machine name
            AddStep(async (stepContext, cancellationToken) =>
            {
                await stepContext.Context.SendActivityAsync($"Great! I will help you request local machine admin.");
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"What is the machine name to add you to local admin group?")
                    });
            });

            //request for number of days
            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                state.MachineName = stepContext.Result.ToString();
                return await stepContext.PromptAsync("numberPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"How many days do you need the admin access?")
                    });
            });

            //write to database
            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                int days = 0;
                var isNum = int.TryParse(stepContext.Result.ToString(), out days);

                if (isNum)
                {
                    state.AccessDays = int.Parse(stepContext.Result.ToString());
                    using (var db = new ContosoHelpdeskContext(new DbContextOptions<ContosoHelpdeskContext>()))
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
                    await stepContext.Context.SendActivityAsync($"Sorry, invalid response!");
                    return await stepContext.EndDialogAsync();
                }
            });
        }

        public static string Id => "localAdminDialog";
        public static LocalAdminDialog Instance { get; } = new LocalAdminDialog(Id);
        /*
         await context.PostAsync("");

            var localAdminDialog = FormDialog.FromForm(this.BuildLocalAdminForm, FormOptions.PromptInStart);

            context.Call(localAdminDialog, this.ResumeAfterLocalAdminFormDialog);
         */

    }
}
