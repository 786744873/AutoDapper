using System;
using XDF.RabbitMq;

namespace XDF.MQReceive
{
    [RabbitMq("QueueNameTest", ExchangeName = "ExchangeNameTest", RoutingKey = "dog", ExchangeType = ExchangeType.Direct, IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}