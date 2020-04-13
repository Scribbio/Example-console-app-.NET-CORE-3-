using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;

namespace ExampleConsoleProject
{
    internal class Program
    {
        //static CancellationTokenSource source = new CancellationTokenSource();
        static CancellationToken token = new CancellationToken();

        public static async Task Main(string[] args)
        {
            //token = source.Token;

            Console.WriteLine("Starting Application");

            // Dependency injection
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("The application started at" + DateTime.Now);

            using (new ExampleContext("connectionString"))
            {
                Task.Run(() => ShowSpinner());
                await host.StartAsync();
            }

            host.Dispose();
            // source.Cancel();

            // Leave app
            Console.WriteLine("Press ESC to exit...");
            ConsoleKeyInfo k;
            while (true)
            {
                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Escape)
                    Environment.Exit(0);

                Console.WriteLine("{0} --- ", k.Key);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddJsonFile("appsettings.json");
                    builder.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    // used to pull config from appsetting.json
                    loggingBuilder.AddConfiguration(hostContext.Configuration);

                    // clear out the default providers (everything that's listening to logging events). 
                    loggingBuilder.ClearProviders();

                    // now we add our own loggers, things to log to
                    loggingBuilder.AddDebug();
                    loggingBuilder.AddConsole();

                    // Other loggers: 
                    // EventSource, 
                    // Eventlog(this is in your windows machine), 
                    // TraceSource
                    // AzureAppServicesFiles & AwureAppSerivcesBlob
                    // ApplicationInsights 
                    // Last 4 require a nuget packages to add them but are provided by Microsoft ///// 

                    //Log.Logger = new LoggerConfiguration()
                    //        .ReadFrom.Configuration(hostContext.Configuration)
                    //        .CreateLogger();

                    loggingBuilder.AddFile("C:/Users/jditrolio/Desktop/Tutorials/Pictures/mylog-{Date}.txt");

                })
                .ConfigureContainer<ContainerBuilder>(autofacContainer =>
                {
                    autofacContainer.RegisterType<AppServices>().InstancePerLifetimeScope();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddHostedService<ApplicationRoot>();
                    serviceCollection.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

                    //serviceCollection.Configure<TestOptions>(hostBuilderContext.Configuration.GetSection("Test")); // No options yet

                });
            


        // Load animation
        private static async Task ShowSpinner()
        {
            var counter = 0;
            for (int i = 0; i < 1000; i++)
            {
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                counter++;
                await Task.Delay(100);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }
}
