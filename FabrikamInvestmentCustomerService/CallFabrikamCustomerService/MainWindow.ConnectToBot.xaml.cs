using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading;

namespace CallFabrikamCustomerService
{
    public partial class MainWindow : Window
    {
        //Send the transcribed text to the bot and get a response
        private async Task<string> GetBotReplyAsync(string text)
        {
            string result = "";

            //TODO: check to see if bot client has been created


            //TODO: create an activity to send a message to bot
            //any correspondence with a bot is an activity


            //TODO: post message to bot


            return result;
        }

        //Setup for conversation if not already
        private async Task CreateBotConversationAsync()
        {
            //TODO: instantiate a directline client to talk to bot


            //TODO: we are starting a conversation with the bot

        }

        //Get bot Activities i.e. messages from the bot
        private async Task<string> ReadBotMessagesAsync(DirectLineClient client, string conversationId)
        {
            string result = "";
            ActivitySet activitySet;
            IEnumerable<Activity> activities;
            int retry = 3;

            
            //the http get on activities for directline uses a poll pattern
            //because of that it is possible to be in a race condition where we are not receiving messages/activities on a premature call
            //here we use a simple retry and delay in between to address that but consider using web sockets
            do
            {
                //TODO: get bot replies on the conversation we started earlier
                

                //TODO: activity watermark helps determine if we have the latest messages
                

                //TODO: get the replies from botId and get the latest Text which is where the message is


                retry--;

                //using a task delay helps us avoid doing thread sleep which is the worst
                //hence this method is async and the pattern is usually async all the way up
                await Task.Delay(500);

            } while (retry > 0);
            
            return result;

        }
    }
}
