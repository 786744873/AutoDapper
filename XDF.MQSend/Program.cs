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
           
            for (int i = 0; i < 100000; i++)
            {
                var log = new MessageModel
                {
                    CreateDateTime = DateTime.Now,
                    Msg = i.ToString()
                };
                RabbitMqService.Instance.Publish(log);
                Console.WriteLine(log.ToJson());
            }

            Console.ReadKey();
            // var input = Input();
            //while (input != "exit")
            //{
            //    var log = new MessageModel
            //    {
            //        CreateDateTime = DateTime.Now,
            //        Msg = input
            //    };
            //    RabbitMqService.Instance.Publish(log);
            //    input = Input();
            //}
        }

        private static string Input()
        {
            System.Console.WriteLine("请输入信息：");
            var input = Console.ReadLine();
            return input;
        }
    }
}
