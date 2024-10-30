using System.Collections.Generic;
using System.Threading.Tasks;
using AwsEnvironmentFinder.Clients;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Options;
using AwsEnvironmentFinder.Configuration;
using AwsEnvironmentFinder.Clients.Entities;

namespace AwsEnvironmentFinder
{
    public class Watchmen
    {
        private readonly IEcsClient _ecsClient;
        private readonly ILogger<Watchmen> _logger;
        private readonly IOptions<FindForOptions> _options;

        public Watchmen(IEcsClient ecsClient, ILogger<Watchmen> logger, IOptions<FindForOptions> options)
        {
            _ecsClient = ecsClient;
            _logger = logger;
            _options = options;
        }

        public async Task Find()
        {
            _logger.LogInformation("Starting to find the keys ...");

            var containers = new List<Container>();

            var clusters = await _ecsClient.GetClusters();

            foreach (var cluster in clusters)
            {
                var services = await _ecsClient.GetServices(cluster);
                var detailedServices = await _ecsClient.GetServiceDetails(services, cluster);

                foreach (var service in detailedServices)
                {
                    var taskDefinition = await _ecsClient.GetTaskDefinitionDetails(service.TaskDefinition);

                    if (!taskDefinition.ContainerDefinitions.Any())
                        continue;

                    foreach (var containerDefinition in taskDefinition.ContainerDefinitions)
                    {
                        var container = new Container(cluster, containerDefinition);

                        _logger.LogInformation("Inspect in: {repositoryName}: {imageTag}", container.ImageRepositoryName, container.ImageTag);

                        foreach (var key in _options.Value.Keys)
                            container.Inspect(key);

                        containers.Add(container);
                    }
                }
            }

            _logger.LogInformation("Computing results on {Count} containers...", containers.Count);

            containers = containers.Where(w => w.HasResults).ToList();

            foreach (var container in containers)
            {
                _logger.LogInformation(container.GetResults());
            }

            if (containers.Count == 0)
            {
                _logger.LogInformation("No results found in the search.");
            }

            _logger.LogInformation("Done.");
        }
    }
}
