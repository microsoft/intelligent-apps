using ContosoHelpdeskChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class InstallAppDialog : WaterfallDialog
    {
        private Models.InstallApp install = new InstallApp();
        List<string> names = new List<string>();
        public InstallAppDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Ok, let’s get started! What is the name of the application?")
                    });
            });

            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                state.AppName = stepContext.Result.ToString();
                names = this.getAppsAsync(state.AppName);

                if (names.Count == 1)
                {
                    install.AppName = names.First();
                    return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Found {install.AppName}. What is the name of the machine to install application?")
                    });
                }
                else if (names.Count > 1)
                {
                    string appnames = "";
                    for (int i = 0; i < names.Count; i++)
                    {
                        appnames += $"<br/>&nbsp;&nbsp;&nbsp;{i + 1}.&nbsp;" + names[i];
                    }
                    return await stepContext.PromptAsync("numberPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"I found {names.Count()} applications.<br/> {appnames}<br/> Please reply 1 - {names.Count()} to indicate your choice.")
                    });
                }
                else
                {
                    await stepContext.Context.SendActivityAsync($"Sorry, I did not find any application with the name \"{state.AppName}\".");
                    return await stepContext.EndDialogAsync();
                }
            });

            //found multiple apps
            AddStep(async (stepContext, cancellationToken) =>
            {
                if (names.Count > 1)
                {
                    int choice;
                    var isNum = int.TryParse(stepContext.Result.ToString(), out choice);

                    if (isNum && choice <= names.Count && choice > 0)
                    {
                        //minus becoz index zero base
                        this.install.AppName = names[choice - 1];
                        return await stepContext.PromptAsync("textPrompt",
                        new PromptOptions
                        {
                            Prompt = stepContext.Context.Activity.CreateReply($"Your choice is {install.AppName}. What is the name of the machine to install application?")
                        });
                    }
                    else
                    {
                        await stepContext.Context.SendActivityAsync($"Sorry, invalid response!");
                        return await stepContext.EndDialogAsync();
                    }
                }
                return await stepContext.NextAsync();
            });

            //found app name & received machine name
            AddStep(async (stepContext, cancellationToken) =>
            {
                var state = await (stepContext.Context.TurnState["BotAccessors"] as BotAccessors).BankingBotStateStateAccessor.GetAsync(stepContext.Context);
                state.MachineName = stepContext.Result.ToString();

                this.install.MachineName = state.MachineName;

                //TODO: Save to database
                using (var db = new ContosoHelpdeskContext(new DbContextOptions<ContosoHelpdeskContext>()))
                {
                    db.InstallApps.Add(install);
                    db.SaveChanges();
                }
                await stepContext.Context.SendActivityAsync($"Great, your request to install {this.install.AppName} on {this.install.MachineName} has been scheduled.");
                return await stepContext.EndDialogAsync();
            });
        }

        public static string Id => "installAppDialog";
        public static InstallAppDialog Instance { get; } = new InstallAppDialog(Id);

        private List<string> getAppsAsync(string Name)
        {
            //TODO: Add EF to lookup database
            var names = new List<string>();

            using (var db = new ContosoHelpdeskContext(new DbContextOptions<ContosoHelpdeskContext>()))
            {
                names = (from app in db.AppMsis
                         where app.AppName.ToLower().Contains(Name.ToLower())
                         select app.AppName).ToList();
            }
            return names;
        }
    }
}
