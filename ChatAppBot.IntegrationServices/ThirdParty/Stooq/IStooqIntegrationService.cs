using ChatAppBot.DataContracts.ThirdParty.Stooq;

namespace ChatAppBot.IntegrationServices.ThirdParty.Stooq
{
    public interface IStooqIntegrationService
    {
        Task<StockQuoteResponse> GetStockQuoteAsync(string stockCode);
    }
}
