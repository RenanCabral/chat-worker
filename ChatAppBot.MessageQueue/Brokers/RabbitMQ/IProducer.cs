namespace ChatAppBot.QueueConsumer.Brokers.RabbitMQ
{
    public interface IProducer
    {
        void Publish(string exchange, string message);
    }
}
