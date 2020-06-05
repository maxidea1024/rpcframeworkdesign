using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SimpleRpc.Sample.Shared;
using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;
using System.Collections;

namespace PromRpc.Sample.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sc = new ServiceCollection();

            sc.AddPromRpcClient("sample", new HttpClientTransportOptions
            {
                Url = "http://127.0.0.1:5000/api",
            })
            .AddPromRpcHyperionSerialize();

            sc.AddPromRpcProxy<IGreeterService>("sample");
            //or
            sc.AddPromRpcProxy(typeof(IGreeterService), "sample");

            var pr = sc.BuildServiceProvider();
        }
    }
}
