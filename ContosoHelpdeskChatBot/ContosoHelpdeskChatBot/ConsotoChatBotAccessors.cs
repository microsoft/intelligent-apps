using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace ContosoHelpdeskChatBot
{
    public class ConsotoChatBotAccessors
    {
        public ConsotoChatBotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        //public static string CountosoBotStateName { get; } = $"{nameof(ConsotoChatBotAccessors)}.ContosoBotState";

        //public IStatePropertyAccessor<BotState> ContosoBotState { get; set; }

        public ConversationState ConversationState { get; }
    }
}