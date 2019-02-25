using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace FabrikamCustomerServiceBot
{
    public class FabrikamServiceBotAccessors
    {
        public FabrikamServiceBotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public ConversationState ConversationState { get; }
    }
}