using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using Microsoft.Bot.Builder.Luis;
using System.Configuration;

namespace FabrikamCustomerServiceBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, MakeRootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private static Func<IDialog<object>> MakeRootDialog()
        {
            //get the luis app id & key
            var MicrosoftLuisAppId = ConfigurationManager.AppSettings["MicrosoftLuisAppId"];
            var MicrosoftLuisKey = ConfigurationManager.AppSettings["MicrosoftLuisKey"];

            Func<IDialog<object>> luisDialog = null;
            //instantiate the root luis dialog and encapsulate it in a delegate
            luisDialog = () =>
            {
                var luisService = new LuisService(new LuisModelAttribute(MicrosoftLuisAppId, MicrosoftLuisKey));
                return new Dialogs.RootLuisDialog(luisService);
            };

            return luisDialog;

        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}