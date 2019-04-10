using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;

namespace demo7dialogs.Dialogs.Balance.CurrentAccount
{
    public class CheckCurrentAccountBalanceDialog : WaterfallDialog
    {
        public CheckCurrentAccountBalanceDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                await stepContext.Context.SendActivityAsync($"Your current balance is...");
                // EndDialogAsync: Task<DialogTurnResult>, Ends a dialog by popping it off the stack and returns an optional result to the dialogs parent
                // the parent dialog will have its `Dialog.resume()` method invoked with any returned result.
                // If the parent dialog hasn't implemented a `resume()` method then it will be automatically ended as well and the result passed to its parent
                return await stepContext.EndDialogAsync();
            });
        }

        public static string Id => "checkCurrentAccountBalanceDialog";
        public static CheckCurrentAccountBalanceDialog Instance { get; } = new CheckCurrentAccountBalanceDialog(Id);
    }
}