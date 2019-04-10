using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace demo2mysimplemiddleware.Middleware
{
    public class SimpleMiddleware1 : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext context, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            await context.SendActivityAsync($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/Before");

            // using next to pass on in middleware pipeline(registered in startup.cs), this middleware will be stacked
            // will go back, when there is no next
            // Q: where is the stack?

            // use if and next to redirect to middleware or not, which means calling next() is totally optional.

            // Encapsulates an asynchronous method that calls the next 
            // Microsoft.Bot.Builder.IMiddleware.Microsoft.Bot.Builder.IMiddleware.OnTurnAsync -> middleware fisrt then bot
            //or Microsoft.Bot.Builder.IBot.Microsoft.Bot.Builder.IBot.OnTurnAsync
            await next(cancellationToken);

            // the folling will be trigger, after the top middleware is been popped
            await context.SendActivityAsync($"[SimpleMiddleware1] {context.Activity.Type}/OnTurn/After");
        }
    }
}
