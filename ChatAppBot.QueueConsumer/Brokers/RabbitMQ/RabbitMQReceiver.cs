using ChatAppBot.CrossCutting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public class RabbitMQReceiver : IRabbitMQReceiver
    {
        private readonly ConnectionFactory connectionFactory;

        public RabbitMQReceiver(RabbitMqConfiguration config)
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

        public void ReadMessages(string queue, EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += eventHandler;

                    channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
                }
            }
        }
    }
}
