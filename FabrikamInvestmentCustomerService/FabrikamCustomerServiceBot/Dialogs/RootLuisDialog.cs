using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace FabrikamCustomerServiceBot.Dialogs
{
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        public RootLuisDialog(ILuisService luis) : base(luis)
        { }

        //TODO: post a reply of the default None message


        //TODO: post a reply of the welcome message


        //TODO: post a reply of checking account balance message


    }
}