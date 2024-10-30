using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AwsEnvironmentFinder
{
    public class Worker : IHostedService
    {
        private readonly Watchmen _watchmen;

        public Worker(Watchmen watchmen)
        {
            _watchmen = watchmen;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _watchmen.Find();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
