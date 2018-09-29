using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf;
using XDF.RabbitMq;

namespace XDF.MQReceive
{
    class Program
    {
        private static string Input()
        {
            Console.WriteLine("是否从队列取一条数据：Y/N");
            var input = Console.ReadLine();
            return input;
        }
        static void Main(string[] args)
        {
            RabbitMqService.Instance.Pull<MessageModel>(msg =>
            {
                Console.WriteLine(msg.ToJson());
            });
            //var input = Input();
            //while (input.ToLower() != "n")
            //{
            //    RabbitMqService.Instance.Pull<MessageModel>(msg =>
            //    {
            //        Console.WriteLine(msg.ToJson());
            //    });

            //    input = Input();
            //}
            //RabbitMqService.Instance.Dispose();
        }
    }
}
