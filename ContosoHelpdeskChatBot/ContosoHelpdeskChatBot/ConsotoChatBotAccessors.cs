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

        public static string CounterStateName { get; } = $"{nameof(ConsotoChatBotAccessors)}.CounterState";

        public IStatePropertyAccessor<BotState> CounterState { get; set; }

        /*
        public static string DialogStateAccessorName { get; } = $"{nameof(ConsotoChatBotAccessors)}.DialogState";
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; internal set; }
        */
        public ConversationState ConversationState { get; }
    }
}