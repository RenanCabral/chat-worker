using ChatAppBot.Worker.RabbitMQ;

namespace ChatAppBot.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        private readonly IServiceProvider serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
            => (this.logger, this.serviceProvider) = (logger, serviceProvider);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = this.serviceProvider.CreateScope())
                {
                    IMessageQueueManager messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageQueueManager>();

                    messageReceiver.ReadMessagesFromQueue("chat-app-bot-queue");
                }

                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}