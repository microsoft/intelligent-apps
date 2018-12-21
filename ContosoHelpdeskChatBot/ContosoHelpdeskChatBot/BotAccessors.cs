using ContosoHelpdeskChatBot.Bots;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoHelpdeskChatBot
{
    public class BotAccessors
    {
        public BotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string ContosoBotStateAccessorName { get; } = $"{nameof(BotAccessors)}.ContosoBotState";
        public IStatePropertyAccessor<ContosoBotState> ContosoBotStateStateAccessor { get; internal set; }

        public static string DialogStateAccessorName { get; } = $"{nameof(BotAccessors)}.DialogState";
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; internal set; }
        public ConversationState ConversationState { get; }
    }
}
