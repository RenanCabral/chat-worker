namespace ChatAppBot.Worker.RabbitMQ
{
    public interface IMessageQueueManager
    {
        public void ReadMessagesFromQueue(string queue);
    }
}
