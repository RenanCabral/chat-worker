using ChatAppBot.DataContracts.ThirdParty.Stooq;

namespace ChatAppBot.ThirdPartyIntegrationServices.ThirdParty.Stooq
{
    public interface IStooqIntegrationService
    {
        Task<StockQuoteResponse> GetStockQuoteAsync(string stockCode);
    }
}
