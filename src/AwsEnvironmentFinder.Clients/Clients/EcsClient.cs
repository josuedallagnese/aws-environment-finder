using System.Collections.Generic;
using System.Linq;
using Amazon.ECS;
using Amazon.ECS.Model;
using System.Threading.Tasks;

namespace AwsEnvironmentFinder.Clients.Clients
{
    public class EcsClient : IEcsClient
    {
        private readonly IAmazonECS _client;

        public EcsClient(IAmazonECS client)
        {
            _client = client;
        }

        public async Task<IEnumerable<string>> GetClusters()
        {
            var all = new List<string>();

            var request = new ListClustersRequest()
            {
                MaxResults = 100
            };

            do
            {
                var response = await _client.ListClustersAsync(request);

                request.NextToken = response.NextToken;

                all.AddRange(response.ClusterArns);
            }
            while (request.NextToken != null);

            return all;
        }

        public async Task<IEnumerable<string>> GetServices(string clusterName)
        {
            var all = new List<string>();

            var request = new ListServicesRequest()
            {
                Cluster = clusterName,
                MaxResults = 100
            };

            do
            {
                var response = await _client.ListServicesAsync(request);

                request.NextToken = response.NextToken;

                all.AddRange(response.ServiceArns);
            }
            while (request.NextToken != null);

            return all;
        }

        public async Task<IEnumerable<Service>> GetServiceDetails(IEnumerable<string> services, string cluster)
        {
            var detailedServices = new List<Service>();

            var pages = services.Select((service, index) => new { Service = service, Page = index / 10 })
              .GroupBy(g => g.Page);

            var request = new DescribeServicesRequest()
            {
                Cluster = cluster
            };

            foreach (var page in pages)
            {
                request.Services = page.Select(x => x.Service).ToList();

                var details = await _client.DescribeServicesAsync(request);

                detailedServices.AddRange(details.Services);
            }

            return detailedServices;
        }

        public async Task<TaskDefinition> GetTaskDefinitionDetails(string taskDefinition)
        {
            var request = new DescribeTaskDefinitionRequest()
            {
                TaskDefinition = taskDefinition
            };

            var response = await _client.DescribeTaskDefinitionAsync(request);

            return response.TaskDefinition;
        }
    }
}
