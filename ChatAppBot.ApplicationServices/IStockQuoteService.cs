using ChatAppBot.DataContracts;

namespace ChatAppBot.ApplicationServices
{
    public interface IStockQuoteService
    {
        Task<StockQuote> GetStockQuoteAsync(string stockCode);
    }
}
