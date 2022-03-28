using ChatAppBot.CrossCutting;
using ChatAppBot.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "ChatAppBot";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient();
    })
    .ConfigureAppConfiguration(LoadConfiguration)
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
        HostName = configuration.GetValue<string>("RabbitMq:HostName"),
        UserName = configuration.GetValue<string>("RabbitMq:UserName"),
        Password = configuration.GetValue<string>("RabbitMq:Password"),
        VirtualHost = configuration.GetValue<string>("RabbitMq:VirtualHost"),
        Port = configuration.GetValue<int>("RabbitMq:Port")
    };
}

await host.RunAsync();