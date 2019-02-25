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

            //get the luis app id & key
            var MicrosoftLuisAppId = ConfigurationManager.AppSettings["MicrosoftLuisAppId"];
            var MicrosoftLuisKey = ConfigurationManager.AppSettings["MicrosoftLuisKey"];

            var LuisApp = new LuisApplication(ConfigurationManager.AppSettings["MicrosoftLuisAppId"], ConfigurationManager.AppSettings["MicrosoftLuisKey"], ConfigurationManager.AppSettings["MicrosoftLuisEndPoint"]);
            var LuisOptions = new LuisPredictionOptions
            {
                IncludeAllIntents = true,
            };

            luisRecognizer = new LuisRecognizer(LuisApp, LuisOptions, true);
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Check LUIS model
                var recognizerResult = await luisRecognizer.RecognizeAsync(turnContext, cancellationToken);
                var topIntent = recognizerResult?.GetTopScoringIntent();

                // Get intent and score
                string strIntent = (topIntent != null) ? topIntent.Value.intent : "";
                double dblIntentScore = (topIntent != null) ? topIntent.Value.score : 0.0;

                if (strIntent != "" && (dblIntentScore > 0.65))
                {
                    switch (strIntent)
                    {
                        case "None":
                            await turnContext.SendActivityAsync("Sorry, I don't understand.");
                            break;
                        case "Greeting":
                            await turnContext.SendActivityAsync("Welcome to Fabrikam Investment Customer Service. I can understand phrases and full sentences. Now how can I help you?");
                            break;
                        case "CheckingAccountBalance":
                            var balance = new Random().Next(0, 1000000);
                            await turnContext.SendActivityAsync($"Your checking account balance is {balance} dollars");
                            break;
                        default:
                            await turnContext.SendActivityAsync(
                                $"Intent: {topIntent.Value.intent} ({topIntent.Value.score}).");
                            break;
                    }
                }
                else if (strIntent != "")
                {
                    // Intent score too low
                    await turnContext.SendActivityAsync($"Intent '{topIntent.Value.intent}' had too low of an intent score, score of {topIntent.Value.score}.");
                }
                else
                {
                    //No intent detected
                    await turnContext.SendActivityAsync("No intent detected.");
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