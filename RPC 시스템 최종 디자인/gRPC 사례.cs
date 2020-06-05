
// Client side

using var channel = RpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.Client(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });


// Server side

public class GreeterService : Greeter.GreeterBase
{
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHelloAsync(HelloRequest request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var clientCertificate = httpContext.Connection.ClientCertificate;

        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name + " from " + clientCertificate.Issuer
        });
    }
}
