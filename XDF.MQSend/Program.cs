using System;
using System.Text;
using RabbitMQ.Client;
using XDF.RabbitMq;

namespace XDF.MQSend
{
    class Program
    {
        static void Main(string[] args)
        {
            var rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = 60,
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "",
                UserName = "guest",
                Password = "guest"
            });

            var input = Input();

            while (input != "exit")
            {
                var log = new MessageModel
                {
                    CreateDateTime = DateTime.Now,
                    Msg = input
                };
                rabbitMqProxy.Publish(log);

                input = Input();
            }

            rabbitMqProxy.Dispose();
        }

        private static string Input()
        {
            System.Console.WriteLine("请输入信息：");

            var input = Console.ReadLine();
            return input;
        }
    }
}
