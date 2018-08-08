using System;

namespace XDF.RabbitMq
{
    /// <summary>
    /// 自定义的RabbitMq队列信息实体特性
    /// </summary>
    public class RabbitMqAttribute : Attribute
    {
        public RabbitMqAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }
        /// <summary>
        /// 路由键
        /// </summary>
        public  string RoutingKey { get; set; }
        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool IsProperties { get; set; }
    }
}