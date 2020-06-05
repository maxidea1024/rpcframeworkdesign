//.netcore에서 Thrift 지원하기
//  https://dev.to/jeikabu/migrating-to-aspnet-core-w-apache-thrift-45f6

#if 0

if (!System.Net.HttpListener.IsSupported)
{
    return;
}

var thrift = new Thrift.Transport.THttpHandler(MultiProcessors.HttpProcessor, new Thrift.TJSONProtocol.Factory());
var listener = new System.Net.HttpListener();
listener.Prefixes.Add("http://localhost:8282/");
listener.Start();

while (!cts.IsCancellationRequested)
{
    var context = await listener.GetContextAsync();
    await Task.Run(() =>
    {

    });
}


#endif


// Json RPC
// https://github.com/alexanderkozlenko/aspnetcore-json-rpc


[JsonRpcRoute("/api")]
public class JsonRpcService : IJsonRpcService
{
    [JsonRpcMethod("m1", "p1", "p2")]
    public Task<long> InvokeMethodAsync(long p1, long p2)
    {
        if (p2 == 0L)
        {
            throw new JsonRpcServiceException(100L);
        }

        return Task.FromResult(p1 / p2);
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJsonRpc();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseJsonRpc();
    }
}




namespace fun.Rpc
{
     public interface IClientService
     {
         Task<HelloReply> SayHelloAsync(HelloRequest request, CancellationToken cancellationToken);
     }

     public interface IServerService
     {
         Task<HelloReply> SayHelloAsync(HelloRequest request, ServerCallContext context);
     }
}

public enum RpcMessageType
{
    Call,
    Reply,
    Oneway,
    Error,
}

/// <summary>
/// RPC Message Header.
/// </summary>
struct RpcMessageHeader
{
    /// <summary>
    /// Message type.
    /// </summary>
    public RpcMessageType Type;

    /// <summary>
    /// Function name. (fully qualified)
    /// </summary>
    public string Name;

    /// <summary>
    /// Function ID. (> 0)
    /// </summary>
    public int ID;

    /// <summary>
    /// Transaction ID.
    /// </summary>
    public int XID;
}

#if 0

//todo extension으로 처리하면 될듯한데...


    public interface IClientService
    {
        Task<HelloReply> SayHelloAsync(HelloRequest request, CancellationToken cancellationToken = default);

        Task NotifyAsync(HelloRequest request, CancellationToken cancellationToken = default);
    }


    var response = await client.SayHelloAsync(new HelloRequest { Name = "ChungHyuk"} );













#region Server related stuff.

    [BindService("HelloProcessor")]
    public interface IServerService
    {
        Task<HelloReply> SayHelloAsync(HelloRequest request, RpcServerCallContext context);

        Task NotifyAsync(HelloRequest request, RpcServerCallContext context);
    }


    public class HelloService : Hello.IServerService
    {
        public async Task<HelloReply> SayHelloAsync(HelloRequest request, RpcServerCallContext context)
        {
            //...
        }

        public async Task NotifyAsync(HelloRequest request, RpcServerCallContext context)
        {
            // ...
        }
    }


    public class ServerProcessor : RpcServerProcessorBase
    {
        protected delegate Task ProcessFunction(int xid, global::fun.Rpc.RpcProtocol input, global::fun.Rpc.RpcProtocol output, global::fun.Rpc.RpcServerCallContext context);
        protected Dictionary<string, ProcessFunction> _processMapByName = new Dictionary<string, ProcessFunction>();
        protected Dictionary<int, ProcessFunction> _processMapByID = new Dictionary<int, ProcessFunction>();

        public ServerProcessor(IServerService service)
        {
            _service = PreValidations.CheckNotNull(service, "service");

            _processMapByName.Add(RpcNames.SayHello, ProcessSayHelloAsync);
            _processMapByName.Add(RpcNames.Notify, ProcessNotifyAsync);

            // ID가 지정된 경우에 한해서 등록함.
            _processMapByID.Add(RpcIDs.SayHello, ProcessSayHelloAsync);
            _processMapByID.Add(RpcIDs.Notify, ProcessNotifyAsync);
        }

        public async global::Threading.Tasks.Task<bool> ProcessAsync(global::fun.Rpc.RpcProtocol input, global::fun.Rpc.RpcProtocol output, global::fun.Rpc.RpcServerCallContext context)
        {
            try
            {
                var header = awit input.ReadRpcMessageBeginAsync(context.CancellationToken);

                ProcessFunction func;

                if (header.ID > 0)
                {
                    _processMapByID.TryGetValue(header.ID, out func);
                }

                if (func == null)
                {
                    _processMapByName.TryGetValue(header.Name, out func);
                }

                if (func == null)
                {
                    await input.SkipStructAsync(context.CancellationToken);
                    await input.ReadMessageEndAsync(context.CancellationToken);

                    var unknownFunctionException = new global::fun.Rpc.RpcApplicationException(global::fun.Rpc.RpcApplicationException.MessageType.UnknownMethod, $"Invalid method: (Nam={header.Name}, ");
                    await output.WriteRpcMessageBeginAsync(global::fun.Rpc.RpcMessageType.Error, header.Name, header.ID, xid, context.CancellationToken);
                    await unknownFunctionException.WriteAsync(output, context.CancellationToken);
                    await output.FlushAsync(context.CancellationToken);
                    return true;
                }

                // Call wrapped process function.
                await func(xid, input, output, context);
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        private async global::Threading.Tasks.Task ProcessSayHelloAsync(int xid, global::fun.Rpc.RpcProtocol input, global::fun.Rpc.RpcProtocol output, global::fun.Rpc.RpcServerCallContext context)
        {
            // Unpack arguments.
            var args = new SayHello_ARGS();
            await args.ReadAsync(input, context.CancellationToken);
            await input.ReadRpcMessageEndAsync(context.CancellationToken);

            try
            {
                var result = new SayHello_RESULT();

                // Process
                result.Result = await _service.SayHelloAsync(args.Request, context.CancellationToken);

                // Send response.
                await output.WriteRpcMessageBeginAsunc(global::fun.Rpc.RpcMessageType.Reply, RpcIDs.SayHello, RpcNames.SayHello, xid, context.CancellationToken);
                await result.WriteAsync(output, context.CancellationToken);
            }
            catch (RpcTransportException)
            {
                // 전송 오류가 난 경우에는 호출자 쪽으로 예외를 전파시킴.
                throw;
            }
            catch (System.Exception)
            {
                if (_logger != null)
                {
                    //todo logging...

                    //session id, meta data, extra payload?
                }
                
                var appError = new global::fun.Rpc.RpcApplicationException(global::fun.Rpc.RpcApplicationException.ExceptionType.InternalError, "Internal error.");
                await output.WriteRpcMessageBeginAsunc(global::fun.Rpc.RpcMessageType.Reply, RpcIDs.SayHello, RpcNames.SayHello, xid, context.CancellationToken);
                await appError.WriteAsync(output, context.CancellationToken);
            }

            // Finish and Send now!

            await output.WriteRpcMessageEndAsync(context.CancellationToken);
            await output.FlushAsync(context.CancellationToken);
        }

        // oneway 형태의 경우
        private async global::Threading.Tasks.Task ProcessNotifyAsync(int xid, global::fun.Rpc.RpcProtocol input, global::fun.Rpc.RpcProtocol output, global::fun.Rpc.RpcServerCallContext context)
        {
            // Unpack arguments.
            var args = new Notify_ARGS();
            await args.ReadAsync(intput, context.CancellationToken);
            await input.ReadRpcMessageEndAsync(context.CancellationToken);

            try
            {
                // Invoke process function.
                await _service.NotifyAsync(args.X, context.CancellationToken);
            }
            catch (global::func.Rpc.RpcTransportException)
            {
                // Transport error.
                throw;
            }
            catch (global::System.Exception e)
            {
                //TODO logging...
                Console.Error.WriteLine("Error occurred in processor: ");
                Console.Error.WriteLine(e.ToString());

                if (_logger != null)
                {
                    _logger.LogWarning("...");
                }
            }
        }
    }



    //RpcServer server = new RpcServer();
    //server.BindService(new Hello.ServerProcessor(new HelloService()));
    //server.Serve();

#endif
