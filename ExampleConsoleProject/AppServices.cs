using Microsoft.Extensions.Logging;
using System;

namespace ExampleConsoleProject
{
    public class AppServices
    {
        //public DTemplate Dal;
        private readonly ILogger _logger;

        public AppServices(ILogger<AppServices> logger)
        {
            this._logger = logger;
        }

        public void ErrorExample()
        {
            Console.WriteLine("done!");
            _logger.LogInformation("test");

            try
            {
                throw new Exception("You forgot to catch me");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "There was a fatal exception as {Time}", DateTime.UtcNow);
            }

        }
        public void SomeProcess()
        {
            _logger.LogInformation("Some process launched at {Time}", DateTime.UtcNow);

            _logger.LogInformation( "Something important started at {Time}", DateTime.UtcNow);

            _logger.LogCritical( "Oh no, it failed at {Time}", DateTime.UtcNow);

            _logger.LogInformation("Some process ended at {Time}", DateTime.UtcNow);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // _logger.Log(LogLevel.Information, LoggingEvents.ProcessEnded, "some text"); // another way of logging messages ///////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }
    }
}
