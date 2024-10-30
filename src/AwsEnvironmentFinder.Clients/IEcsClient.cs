using Amazon.ECS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwsEnvironmentFinder.Clients
{
    public interface IEcsClient
    {
        Task<IEnumerable<string>> GetClusters();

        Task<IEnumerable<string>> GetServices(string clusterName);

        Task<IEnumerable<Service>> GetServiceDetails(IEnumerable<string> services, string cluster);

        Task<TaskDefinition> GetTaskDefinitionDetails(string taskDefinition);
    }
}
