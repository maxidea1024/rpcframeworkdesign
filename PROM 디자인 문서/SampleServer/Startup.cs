using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleRpc.Sample.Shared;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Server;
using SimpleRPC.Sample.Server;
using SimpleRpc.Serialization.Hyperion;

namespace PromRpc.Sample.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPromRpcServer(new HttpServerTransportOptions { Path = "/api"})
                    .AddPromRpcHyperSerializer();

            services.AddSingleton<IGreeterService, IGreeterServiceImpl>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            app.UsePromRpcServer();
        }
    }
}
