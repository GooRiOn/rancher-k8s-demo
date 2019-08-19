using Convey.MessageBrokers;

namespace MyApplication.Messaging
{
    public class CorrelationContext : ICorrelationContext
    {
        public string CorrelationId { get; set; }
        public string SpanContext { get; set; }
        public int Retries { get; set; }
    }
}