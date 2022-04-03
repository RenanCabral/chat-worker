using ChatAppBot.CrossCutting;
using RabbitMQ.Client;
using System.Text;

namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public class Producer : IProducer
    {
        private readonly ConnectionFactory connectionFactory;

        public Producer(RabbitMqConfiguration config)
        {
            this.connectionFactory = new ConnectionFactory()
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                Port = config.Port,
                VirtualHost = config.VirtualHost
            };
        }

        public void Publish(string exchange, string message)
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
                }
            }
        }
    }
}