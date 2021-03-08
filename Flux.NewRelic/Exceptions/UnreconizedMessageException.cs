using System;
using System.Runtime.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Exceptions
{
    [Serializable]
    internal class UnreconizedMessageException : Exception
    {
        public UnreconizedMessageException()
        {
        }

        public UnreconizedMessageException(string message) : base(message)
        {
        }

        public UnreconizedMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnreconizedMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}