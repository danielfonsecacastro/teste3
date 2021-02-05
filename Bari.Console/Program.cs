using Bari.Domain.Interfaces;
using Bari.IoC;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Console
{
    class Program
    {
        private static readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            DependencyInjectionConsole.Configure();

            var settings = DependencyInjectionConsole.GetRequiredService<ISettings>();
            var factory = new ConnectionFactory()
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
            };

            DependencyInjectionConsole.Configure();
            var messageService = DependencyInjectionConsole.GetRequiredService<IMessageService>();

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Task task = new Task(() =>
            {
                while (true)
                {
                    var body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(messageService.Create("Hello Word")));
                    channel.BasicPublish(string.Empty, settings.Queue, null, body);
                    Thread.Sleep(settings.Interval);
                }
            });
            task.Start();

            channel.QueueDeclare(settings.Queue, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, basicDeliver) =>
            {
                messageService.Proccess(Encoding.UTF8.GetString(basicDeliver.Body.ToArray()));
                channel.BasicAck(deliveryTag: basicDeliver.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: settings.Queue, autoAck: false, consumer: consumer);

            System.Console.WriteLine("Aguardando mensagens para processamento");
            System.Console.CancelKeyPress += (o, e) =>
            {
                System.Console.WriteLine("Saindo...");
                _waitHandle.Set();
                e.Cancel = true;
            };

            _waitHandle.WaitOne();
        }
    }
}
