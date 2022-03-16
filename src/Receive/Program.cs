using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Receive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration conf = hostContext.Configuration;

                    RabbitMq options = conf.GetSection("RabbitMq").Get<RabbitMq>();
                    services.AddSingleton(options);

                    services.AddHostedService<Worker>();
                });
    }
}
