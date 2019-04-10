﻿using System.Collections.Generic;
using System.Linq;
using demo8statemiddleware;
using demo8statemiddleware.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class Startup
{
    // Inject the IHostingEnvironment into constructor
    public Startup(IHostingEnvironment env)
    {
        // Set the root path
        ContentRootPath = env.ContentRootPath;
    }

    // Track the root path so that it can be used to setup the app configuration
    public string ContentRootPath { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Set up the service configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        services.AddSingleton(configuration);

        // Add your SimpleBot to your application
        services.AddBot<StateBot>(options =>
        {
            options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
            var conversationState = new ConversationState(new MemoryStorage());
            options.State.Add(conversationState);
        });

        services.AddSingleton<BotAccessors>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<BotFrameworkOptions>>().Value;
            var conversationState = options.State.OfType<ConversationState>().FirstOrDefault();

            var accessors = new BotAccessors(conversationState)
            {
                DemoStateAccessor = conversationState.CreateProperty<DemoState>(BotAccessors.DemoStateName),
            };

            return accessors;
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseStaticFiles();

        // Tell your application to use Bot Framework
        app.UseBotFramework();
    }
}