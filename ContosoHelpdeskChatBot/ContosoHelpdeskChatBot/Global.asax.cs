using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ContosoHelpdeskChatBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            BotConfig.UpdateConversationContainer();
        }

        //setting Bot data store policy to use last write win
        //example if bot service got restarted, existing conversation would just overwrite data to store
        public static class BotConfig
        {
            public static void UpdateConversationContainer()
            {
                var store = new InMemoryDataStore();

                Conversation.UpdateContainer(
                           builder =>
                           {
                               builder.Register(c => store)
                                         .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                                         .AsSelf()
                                         .SingleInstance();

                               builder.Register(c => new CachingBotDataStore(store,
                                          CachingBotDataStoreConsistencyPolicy
                                          .ETagBasedConsistency))
                                          .As<IBotDataStore<BotData>>()
                                          .AsSelf()
                                          .InstancePerLifetimeScope();


                           });
            }
        }
    }
}
