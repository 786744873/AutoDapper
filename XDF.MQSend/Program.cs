using System;
using System.Text;
using RabbitMQ.Client;

namespace XDF.MQSend
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
            using (var connection = factorey.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明一个队列，设置队列是否持久化，排他性，与自动删除
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false,
                        arguments: null);
                    string message = "Hello RabbitMQ!1111";
                    string input;
                    do
                    {
                        input = Console.ReadLine();

                        var sendBytes = Encoding.UTF8.GetBytes(input);
                        //发布消息
                        channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: sendBytes);
                    } while (input != null && input.Trim().ToLower() != "exit");
                    var body = Encoding.UTF8.GetBytes(message);
                    
                }
            }
        }
    }
}
