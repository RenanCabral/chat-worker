using RabbitMQ.Client.Events;

namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public interface IRabbitMQReceiver
    {
        void ReadMessages(string queue, EventHandler<BasicDeliverEventArgs> eventHandler);
    }
}
