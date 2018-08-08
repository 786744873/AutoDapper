using System;
using XDF.RabbitMq;

namespace XDF.Subscribe
{
    [RabbitMq("QueueNameTest", ExchangeName = "ExchangeNameTest",RoutingKey = "cat", ExchangeType=ExchangeType.Direct,IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}