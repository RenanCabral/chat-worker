using ChatAppBot.CrossCutting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public class Consumer : IConsumer
    {
        private readonly ConnectionFactory connectionFactory;

        public Consumer(RabbitMqConfiguration config)
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

        public void Consume(string queue, EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += eventHandler;

                    // accept only one unack-ed message at a time
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
                }
            }
        }
    }
}
