using System;
using XDF.RabbitMq;

namespace XDF.Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqService.Instance.Subscribe<MessageModel>(msg =>
            {
                var json = msg.ToJson();
                Console.WriteLine(json);
            });
            Console.ReadLine();
        }
    }
}
