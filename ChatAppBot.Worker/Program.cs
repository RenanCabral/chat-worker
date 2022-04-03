using ChatAppBot.ApplicationServices;
using ChatAppBot.CrossCutting;
using ChatAppBot.QueueConsumer.Brokers.RabbitMQ;
using ChatAppBot.IntegrationServices.ThirdParty.Stooq;
using ChatAppBot.Worker;
using ChatAppBot.Worker.RabbitMQ;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "ChatAppBot";
    })
    .ConfigureAppConfiguration(LoadConfiguration)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient();

        services.AddScoped<IMessageQueueManager, MessageQueueManager>();

        services.AddScoped<IStockQuoteService, StockQuoteService>();

        services.AddScoped<IStooqIntegrationService, StooqIntegrationService>();

        services.AddScoped<IConsumer>(rabbitReceiver => new Consumer(AppConfiguration.RabbitMqConfiguration));

        services.AddScoped<IProducer>(rabbitPublisher => new Producer(AppConfiguration.RabbitMqConfiguration));
    })
    .Build();

static void LoadConfiguration(HostBuilderContext ctx, IConfigurationBuilder config)
{
    var env = ctx.HostingEnvironment;

    config
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{ env.EnvironmentName }.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    var configuration = config.Build();

    AppConfiguration.Settings.StooqApiBaseAddress = configuration.GetValue<string>("Settings:StooqApiBaseAddress");
    AppConfiguration.Settings.StooqApiTimeout = TimeSpan.FromSeconds(configuration.GetValue<int>("Settings:StooqApiTimeout"));

    AppConfiguration.RabbitMqConfiguration = new RabbitMqConfiguration()
    {
        HostName = configuration.GetValue<string>("Settings:RabbitMq:HostName"),
        UserName = configuration.GetValue<string>("Settings:RabbitMq:UserName"),
        Password = configuration.GetValue<string>("Settings:RabbitMq:Password"),
        VirtualHost = configuration.GetValue<string>("Settings:RabbitMq:VirtualHost"),
        Port = configuration.GetValue<int>("Settings:RabbitMq:Port")
    };
}

await host.RunAsync();