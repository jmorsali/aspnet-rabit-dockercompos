using System;
using System.IO;
using Consumer_Worker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Consumer_Worker
{
    static class Program
    {
        static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            var context = builder.Services.GetService<MessagesStoreContext>();
            context.Database.Migrate();
            builder.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Path.Combine(AppContext.BaseDirectory));
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddLogging();
                    services.AddTransient<IMqLogMessageRepository, MqLogMessageRepository>();
                    services.AddTransient<IServiceClient, ServiceClient>();
                    services.AddHostedService<Worker>();
                    services.AddDbContext<MessagesStoreContext>(
                        options => options.UseSqlServer(configuration.GetConnectionString("Container")),
                        ServiceLifetime.Transient);

                });



    }
}