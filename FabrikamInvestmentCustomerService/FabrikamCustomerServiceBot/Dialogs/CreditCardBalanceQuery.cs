using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FabrikamCustomerServiceBot.Dialogs
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using System.Text.RegularExpressions;

    [Serializable]
    public class CreditCardBalanceQuery
    {
        [Prompt("What is the last four digit of credit card?")]
        public int LastFour { get; set; }


        public IForm<CreditCardBalanceQuery> BuildCreditCardBalanceForm()
        {
            return new FormBuilder<CreditCardBalanceQuery>()
                .Field(nameof(CreditCardBalanceQuery.LastFour),
                validate: async (state, value) =>
                {
                    string input = value.ToString();
                    input = Regex.Replace(input, "[^0-9.]", "");
                    var result = new ValidateResult { IsValid = true, Value = input };
                    return result;
                })
                .Build();
        }

        public async Task ResumeAfterCreditCardBalanceFormDialog(IDialogContext context, IAwaitable<object> userReply)
        {
            var balance = new Random().Next(0, 500000);
            await context.PostAsync($"Credit card balance. Your current total is {balance} dollars");

            context.Done<object>(null);
        }
    }
}