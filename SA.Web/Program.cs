using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SA.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config)
                        .ConfigureKestrel(options =>
                        {
                            options.ListenAnyIP(5001);
                        })                        
                        .UseWebRoot("wwwroot")
                        .UseUrls("http://*:5000", "https://*5001")
                        .UseIISIntegration()
                        .UseStartup<Startup>();
                });
        }
    }
}
