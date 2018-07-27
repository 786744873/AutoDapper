using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace XDF.MQReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factorey = new ConnectionFactory()
            {
                HostName = "",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection=factorey.CreateConnection())
            {
                using (var channel=connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine($"Received: {message}");
                    };
                    channel.BasicConsume(queue:"hello",autoAck:true, consumer: consumer);
                    Console.WriteLine("按任意键退出。。");
                    Console.ReadLine();
                }
            }
        }
    }
}
