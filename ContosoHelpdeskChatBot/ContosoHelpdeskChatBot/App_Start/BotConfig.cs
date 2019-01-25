using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.WebApi;
using Microsoft.Bot.Configuration;
using Unity;
using Unity.Lifetime;

namespace ContosoHelpdeskChatBot.App_Start
{
    public class BotConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapBotFramework(botConfig =>
            {
                // Load Connected Services from .bot file
                var path = HostingEnvironment.MapPath(@"~/contosohelpdeskchatbot.bot");
                var botConfigurationFile = BotConfiguration.Load(path);
                var endpointService = (EndpointService)botConfigurationFile.Services.First(s => s.Type == "endpoint");

                botConfig
                    .UseMicrosoftApplicationIdentity(endpointService?.AppId, endpointService?.AppPassword);

                // The Memory Storage used here is for local bot debugging only. When the bot
                // is restarted, everything stored in memory will be gone.
                IStorage dataStore = new MemoryStorage();

                // Create Conversation State object.
                // The Conversation State object is where we persist anything at the conversation-scope.
                var conversationState = new ConversationState(dataStore);

                // Create the custom state accessor.
                // State accessors enable other components to read and write individual properties of state.
                var accessors = new ConsotoChatBotAccessors(conversationState)
                {
                    //ContosoBotState = conversationState.CreateProperty<BotState>(ConsotoChatBotAccessors.CounterStateName),
                };

                UnityConfig.Container.RegisterInstance<ConsotoChatBotAccessors>(accessors, new ContainerControlledLifetimeManager());
            });
        }
    }
}