using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromRpc.Sample.Shared;

namespace PromRpc.Sample.Server
{
    public class GreeterServiceImpl : IGreeterService
    {
        public void Plus(int a, int b)
        {

        }

        public string Concat(string a, string b)
        {
            return a + b;
        }

        public async Task WriteFooAsync(string a, string b)
        {
            await Task.Delay(10);
        }
    }
}
