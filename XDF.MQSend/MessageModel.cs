using System;
using XDF.RabbitMq;

namespace XDF.MQSend
{
    [RabbitMq("QueueNameTest", ExchangeName = "ExchangeNameTest", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}