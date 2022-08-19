using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consumer_Worker.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer_Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IServiceClient _serviceClient;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration, IServiceClient serviceClient)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _serviceClient = serviceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            string[] testStrings = { "message one", "message two", "message three", "message four", "message five" };

            Console.WriteLine("Sleeping to wait for Rabbit***********************************");
            //await Task.Delay(10000, stoppingToken).WaitAsync(stoppingToken);
            Console.WriteLine("Posting messages to webApi??????????????????????????????????????");
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    await _serviceClient.PostMessage($"{testStrings[i]} ---- {DateTime.Now}").WaitAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            //await Task.Delay(1000, stoppingToken).WaitAsync(stoppingToken);
            Console.WriteLine("Consuming Queue Now");

            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("Rabbit:Server").Value, //rabbitmq
                Port = Convert.ToInt32(_configuration.GetSection("Rabbit:Port").Value), //5672,
                UserName = _configuration.GetSection("Rabbit:UserName").Value, //"guest",
                Password = _configuration.GetSection("Rabbit:Password").Value, //"guest"
            };

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageReceived;
            channel.BasicConsume(queue: "hello",
                autoAck: true,
                consumer: consumer);
        }
    }

    private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
    {
        var body = e.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine(" [x] Received from Rabbit: {0}", message);
        var _repo = _serviceProvider.GetService<IMqLogMessageRepository>();
        _repo.SaveMessageToDb(message);
    }
}