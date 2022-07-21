using System;
using System.Threading.Tasks;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;

namespace worker
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string[] testStrings = new string[] { "message one", "message two", "message three", "message four", "message five" };

                Console.WriteLine("Sleeping to wait for Rabbit***********************************");
                Task.Delay(10000).Wait();
                Console.WriteLine("Posting messages to webApi??????????????????????????????????????");
                for (int i = 0; i < 5; i++)
                {
                    ServiceClient.PostMessage(testStrings[i]).Wait();
                }

                Task.Delay(1000).Wait();
                Console.WriteLine("Consuming Queue Now");

                ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
                factory.UserName = "guest";
                factory.Password = "guest";
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

        private static void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(" [x] Received from Rabbit: {0}", message);
            SaveMessageToDB(message);

        }

        private static void SaveMessageToDB(string message)
        {
            using (var connection = new SqlConnection("Server=db;Database=MQRepository;User ID= sa;password=@A123456789@;MultipleActiveResultSets=True"))
            {
                using (var command = new SqlCommand("",connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = $"Insert Into TBL_LOG (Id,Message) Values ('{Guid.NewGuid()}','{message}')";
                        command.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine(" [x] stored in DB from Rabbit: {0}", message);
                    }
                    catch (Exception ex){
                        Console.WriteLine("Error In Insert To DB: {0}", ex);
                    }
                }
            }
        }
    }
}