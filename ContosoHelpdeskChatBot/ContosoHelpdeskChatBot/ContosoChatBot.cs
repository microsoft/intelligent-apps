using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ContosoHelpdeskChatBot.Dialogs;

namespace ContosoHelpdeskChatBot
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class ContosoChatBot : IBot
    {
        private DialogSet dialogs { get; set; }

        private readonly ConsotoChatBotAccessors _accessors;

        public ContosoChatBot(ConsotoChatBotAccessors accessors)
        {
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            // Create top - level dialog(s)
            dialogs = new DialogSet(_accessors.ConversationState.CreateProperty<DialogState>(nameof(ContosoChatBot)));
            dialogs.Add(new MainDialog("MainDialog"));
            dialogs.Add(new ChoicePrompt("promptChoice"));
            dialogs.Add(new TextPrompt("promptText"));
            dialogs.Add(new NumberPrompt<int>("promptNumber"));
            dialogs.Add(new InstallAppDialog("InstallAppDialog"));
            dialogs.Add(new LocalAdminDialog("LocalAdminDialog"));
            dialogs.Add(new ResetPasswordDialog("ResetPasswordDialog"));
            //_accessors = accessors;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogContext = await dialogs.CreateContextAsync(turnContext, cancellationToken);
                DialogTurnResult dialogResult = await dialogContext.ContinueDialogAsync(cancellationToken);

                if (dialogResult.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync(MainDialog.dialogId, cancellationToken);
                }
                else if (dialogResult.Status == DialogTurnStatus.Complete)
                {
                    await dialogContext.BeginDialogAsync(MainDialog.dialogId, cancellationToken);
                }

                await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
        }
    }
}