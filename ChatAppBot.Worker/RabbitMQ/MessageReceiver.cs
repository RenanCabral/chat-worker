using ChatAppBot.ApplicationServices;
using ChatAppBot.QueueConsumer.Brokers.RabbitMQ;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatAppBot.Worker.RabbitMQ
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly IRabbitMQReceiver rabbitMqReceiver;
        private readonly IStockQuoteService stockQuoteService;

        public MessageReceiver(IRabbitMQReceiver rabbitMqReceiver, IStockQuoteService stockQuoteService)
        {
            this.rabbitMqReceiver = rabbitMqReceiver;
            this.stockQuoteService = stockQuoteService;
        }

        public void ReadMessagesFromQueue(string queue)
        {
            this.rabbitMqReceiver.ReadMessages(queue, OnMessageReceived);
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var stockQuote = TransformBodyToMessage(e.Body);
            this.stockQuoteService.GetStockQuoteAsync(stockQuote);
        }

        private string TransformBodyToMessage(ReadOnlyMemory<byte> body)
        {
            return Encoding.UTF8.GetString(body.ToArray());
        }
    }
}
