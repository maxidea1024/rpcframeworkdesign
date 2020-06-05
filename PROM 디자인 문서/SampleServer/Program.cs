using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PromRpc.Sample.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureHostBuilder(args).Build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
