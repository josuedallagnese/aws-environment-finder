using AwsEnvironmentFinder;
using AwsEnvironmentFinder.Clients;
using AwsEnvironmentFinder.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(hostConfig =>
    {
        hostConfig.SetBasePath(Directory.GetCurrentDirectory());
        hostConfig.AddJsonFile("appsettings.json");
        hostConfig.AddCommandLine(args);
    })
    .ConfigureServices((host, services) =>
    {
        services.AddLogging(configure => configure.AddSimpleConsole(options =>
        {
            options.TimestampFormat = "HH:mm:ss ";
        }));

        services.AddOptions<AwsCredentialOptions>()
            .Bind(host.Configuration.GetSection(AwsCredentialOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FindForOptions>()
            .Bind(host.Configuration.GetSection(FindForOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAwsClients();

        services.AddSingleton<Watchmen>();
        services.AddHostedService<Worker>();
    }).Build();

await host.StartAsync();
