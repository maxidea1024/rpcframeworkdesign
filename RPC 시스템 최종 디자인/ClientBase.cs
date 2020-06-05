using fun.Rpc.Core;

public abstract class ClientBase
{
    public ClientBase(ChannelBase channel)
    {

    }

    public ClientBase(CallInvoker callInvoker)
    {

    }

    protected ClientBase()
    {

    }

    protected ClientBase(ClientBaseConfiguration configuration)
    {

    }

    protected CallInvoker CallInvoker { get; }

    protected internal class ClientBaseConfiguration
    {

    }
}
