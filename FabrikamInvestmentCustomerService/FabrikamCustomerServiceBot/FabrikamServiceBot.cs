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
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using System.Configuration;

namespace FabrikamCustomerServiceBot
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class FabrikamServiceBot : IBot
    {
        private DialogSet dialogs { get; set; }

        private readonly FabrikamServiceBotAccessors _accessors;
        private LuisRecognizer luisRecognizer;

        public FabrikamServiceBot(FabrikamServiceBotAccessors accessors)
        {
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                try
                {
                    await turnContext.SendActivityAsync($"You said {turnContext.Activity.Text}");
                }
                catch (Exception ex) // For production would want to catch more specific exception
                {
                    await turnContext.SendActivityAsync("An error occured, cancelling.");
                }

                await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
            else if (turnContext.Activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (turnContext.Activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (turnContext.Activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
        }
    }
}