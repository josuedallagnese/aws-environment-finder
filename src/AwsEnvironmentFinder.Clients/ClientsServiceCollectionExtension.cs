using Amazon.Runtime;
using AwsEnvironmentFinder.Configuration;
using Amazon;
using Microsoft.Extensions.DependencyInjection;
using AwsEnvironmentFinder.Clients.Clients;
using Amazon.ECS;
using Microsoft.Extensions.Options;

namespace AwsEnvironmentFinder.Clients
{
    public static class ClientsServiceCollectionExtension
    {
        public static IServiceCollection AddAwsClients(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonECS>(provider =>
            {
                var options = provider.GetService<IOptions<AwsCredentialOptions>>();
                var credentials = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
                var region = RegionEndpoint.GetBySystemName(options.Value.Region);

                return new AmazonECSClient(credentials, region);
            });

            services.AddSingleton<IEcsClient, EcsClient>();

            return services;
        }
    }
}
