using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ContosoHelpdeskChatBot.Models;
using Microsoft.Bot.Builder.FormFlow;

namespace ContosoHelpdeskChatBot.Dialogs
{
    [Serializable]
    public class LocalAdminDialog : IDialog<object>
    {
        private LocalAdmin admin = new LocalAdmin();
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Great I will help you request local machine admin.");

            //TODO: use form builder call the dialog
        }

        private async Task ResumeAfterLocalAdminFormDialog(IDialogContext context, IAwaitable<LocalAdminPrompt> userReply)
        {
            //TODO: save data to LocalAdmin table


            context.Done<object>(null);
        }

        private IForm<LocalAdminPrompt> BuildLocalAdminForm()
        {
            //TODO: implement form builder and use validate to assign values to admin.MachineName & admin.AdminDuration
            var form = new FormBuilder<LocalAdminPrompt>();

            return form.Build();
        }
    }
}