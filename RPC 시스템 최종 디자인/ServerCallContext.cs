using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Grpc.Core
{
    public abstract class ServerCallContext
    {
        protected ServerCallContext();

        public IDictionary<object, object> UserState { get; }

        public AuthContext AuthContext { get; }

        public Status Status { get; set; }

        public Metadata ResponseTrailers { get; }

        public CancellationToken CancellationToken { get; }

        public Metadata RequestHeaders { get; }

        public DateTime Deadline { get; }

        public WriteOptions WriteOptions { get; set; }

        public string Host { get; }

        public string Method { get; }

        public string Peer { get; }

        protected abstract WriteOptions WriteOptionsCore { get; set; }

        protected abstract Status StatusCore { get; set; }

        protected abstract Metadata ResponseTrailersCore { get; }

        protected abstract CancellationToken CancellationTokenCore { get; }

        protected abstract Metadata RequestHeadersCore { get; }

        protected abstract DateTime DeadlineCore { get; }

        protected abstract string HostCore { get; }

        protected abstract string MethodCore { get; }

        protected abstract string PeerCore { get; }

        protected virtual IDictionary<object, object> UserStateCore { get; }

        public ContextPropagationToken CreatePropagationToken(ContextPropagationOptions options = null);

        public Task WriteResponseHeadersAsync(Metadata responseHeaders);

        protected abstract ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options);

        protected abstract Task WriteResponseHeadersAsyncCore(Metadata responseHeaders);
    }
}
