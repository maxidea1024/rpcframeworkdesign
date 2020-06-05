using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromRpc.Sample.Shared;

namespace PromRpc.Sample.Server
{
    public interface IGreeterService
    {
        void Plus(int a, int b);

        string Concat(string a, string b);

        Task WriteFooAsync(string a, string b);

        Task<string> ConcatAsync(string a, string b);

        Task<string> ReturnGenericTypeAsString<T>();

        Task<IEnumerable<string>> ReturnGenericIEnumerable<T>();

        Task<T> ThrowException<T>();
    }
}
