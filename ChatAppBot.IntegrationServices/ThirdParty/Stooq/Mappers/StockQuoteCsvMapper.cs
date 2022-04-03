using ChatAppBot.DataContracts.ThirdParty.Stooq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBot.IntegrationServices.ThirdParty.Stooq.Mappers
{
    public static class StockQuoteCsvMapper
    {
        public static StockQuoteResponse ToResponse(string stockQuoteCsvContent)
        {
            var contentArray = stockQuoteCsvContent.Split(',');

            return new StockQuoteResponse()
            {
                Symbol = contentArray[0],
                Date = contentArray[1],
                Time = contentArray[2],
                Open = contentArray[3],
                High = contentArray[4],
                Low = contentArray[5],
                Close = contentArray[6],
                Volume = contentArray[7]
            };
        }
    }
}
