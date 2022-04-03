using RabbitMQ.Client.Events;

namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public interface IConsumer
    {
        void Consume(string queue, EventHandler<BasicDeliverEventArgs> eventHandler);
    }
}
