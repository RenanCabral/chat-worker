namespace ChatAppBot.Worker.RabbitMQ
{
    public interface IMessageReceiver
    {
        public void ReadMessagesFromQueue(string queue);
    }
}
