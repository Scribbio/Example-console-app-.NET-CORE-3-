using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleConsoleProject
{
    public class ApplicationRoot : IHostedService
    {
        private readonly AppServices appServices;

        // appServices are dependency injected
        public ApplicationRoot(/*IOptions<TestOptions> options,*/ AppServices appServices)
        {
            this.appServices = appServices;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.appServices.ErrorExample();
            this.appServices.SomeProcess();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
