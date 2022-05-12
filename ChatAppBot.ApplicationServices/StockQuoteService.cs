using ChatAppBot.DataContracts;
using ChatAppBot.IntegrationServices.ThirdParty.Stooq;

namespace ChatAppBot.ApplicationServices
{
    public class StockQuoteService : IStockQuoteService
    {
        private readonly IStooqIntegrationService stooqIntegrationService;

        public StockQuoteService(IStooqIntegrationService stooqIntegrationService)
            => (this.stooqIntegrationService) = (stooqIntegrationService);

        public async Task<StockQuote> GetStockQuoteAsync(string stockCode)
        {
            try
            {
                var stockQuoteResponse = await stooqIntegrationService.GetStockQuoteAsync(stockCode);

                var stockQuote = new StockQuote()
                {
                    Code = stockQuoteResponse.Symbol,
                    Value = decimal.Parse(stockQuoteResponse.Close)
                };

                return stockQuote;
            }
            catch (Exception)
            {
                //TODO: some log here
                return null;
            }
        }
    }
}