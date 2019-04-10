using demo5luismiddleware.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Bot.Builder.AI.Luis;

public class Startup
{
    // Inject the IHostingEnvironment into constructor
    public Startup(IHostingEnvironment env)
    {
        // Set the root path
        ContentRootPath = env.ContentRootPath;
    }

    // Track the root path so that it can be used to setup the app configuration
    public string ContentRootPath { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Set up the service configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();
        var configuration = builder.Build();
        services.AddSingleton(configuration);

        // register luis in service provider as singleton when applicaiton build
        services.AddSingleton(sp =>
        {
            // get these values from luis.ai
            // i've left the endpoint in so you have an example of an url that works
            // because all luis client libs seem to vary 
            var luisApp = new LuisApplication(
                applicationId: "",
                endpointKey: "", 
                endpoint:"https://westus.api.cognitive.microsoft.com");

            var luisPredictionOptions = new LuisPredictionOptions
            {
                IncludeAllIntents = true,
            };

        // return LuisRecognizer
        // Runs an utterance through a recognizer and returns a generic recognizer result.
        
        // Q: what does the aciton do?
        // utterance 
        // intent (categories), turn on/off
        // action
        // entity, associate with the intent, bathroom/washroom/bedroom

        // train luis using utterance by label it -> 
        // publish trained applicaiton as web service through url via http
        // return {query:"turn off the kitchen light", "topScoringIntent":{"intent":"turn off light", "score": 1.0, "actions":[{parameters:["type":"Room", "value":[{entity:"kitchen", type:"Room"}]]}]}}
            return new LuisRecognizer(
                application: luisApp,
                predictionOptions: luisPredictionOptions,
                includeApiResults: true);
        });

        // Add your SimpleBot to your application
        services.AddBot<SimpleBot>(options =>
        {
            options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
        });

    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseStaticFiles();

        // Tell your application to use Bot Framework
        app.UseBotFramework();
    }
}