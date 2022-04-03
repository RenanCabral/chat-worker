using ChatAppBot.ApplicationServices;
using ChatAppBot.DataContracts;
using ChatAppBot.QueueConsumer.Brokers.RabbitMQ;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatAppBot.Worker.RabbitMQ
{
    public class MessageQueueManager : IMessageQueueManager
    {
        private readonly IConsumer consumer;
        private readonly IProducer producer;
        private readonly IStockQuoteService stockQuoteService;

        public MessageQueueManager(IConsumer consumer, IProducer producer, IStockQuoteService stockQuoteService)
            => (this.consumer, this.producer, this.stockQuoteService) = (consumer, producer, stockQuoteService);

        public void ReadMessagesFromQueue(string queue)
        {
            this.consumer.Consume(queue, OnMessageReceived);
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var stockCode = ReadMessageFromBody(e.Body);

            var stockQuote = this.stockQuoteService.GetStockQuoteAsync(stockCode).Result;

            var message = FormatMessageToPublish(stockQuote);

            this.producer.Publish("chat-app", message);
        }

        private string ReadMessageFromBody(ReadOnlyMemory<byte> body)
        {
            return Encoding.UTF8.GetString(body.ToArray());
        }

        private string FormatMessageToPublish(StockQuote stockQuote)
        {
            return $"{stockQuote.Code} quote is ${ string.Format("{0:C}", stockQuote.Value)} per share.";
        }
    }
}