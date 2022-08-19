using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace publisher_api.Services
{
    // define interface and service
    public interface IMessageService
    {
        bool Enqueue(string message);
    }

    public class MessageService : IMessageService
    {
        readonly IModel _channel;
        public MessageService(IConfiguration configuration)
        {
            Console.WriteLine("about to connect to rabbit");

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("Rabbit:Server").Value, //rabbitmq
                Port = Convert.ToInt32(configuration.GetSection("Rabbit:Port").Value), //5672,
                UserName = configuration.GetSection("Rabbit:UserName").Value, //"guest",
                Password = configuration.GetSection("Rabbit:Password").Value, //"guest"
            };
            var conn = factory.CreateConnection();
            _channel = conn.CreateModel();
            _channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }
        public bool Enqueue(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            _channel.BasicPublish(exchange: "",
                                routingKey: "hello",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
            return true;
        }
    }
}

