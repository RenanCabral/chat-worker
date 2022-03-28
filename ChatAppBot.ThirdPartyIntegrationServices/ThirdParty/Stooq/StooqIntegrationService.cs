using ChatAppBot.CrossCutting;
using ChatAppBot.DataContracts.ThirdParty.Stooq;
using ChatAppBot.ThirdPartyIntegrationServices.ThirdParty.Stooq.Mappers;

namespace ChatAppBot.ThirdPartyIntegrationServices.ThirdParty.Stooq
{
    public class StooqIntegrationService : IStooqIntegrationService
    {
        #region Fields

        private static HttpClient httpClient;

        #endregion

        #region Contructors

        public StooqIntegrationService(IHttpClientFactory httpFactory)
        {
            httpClient = httpFactory.CreateClient();
            ConfigureHttpClient(httpClient);
        }

        #endregion

        #region Public Methods

        public async Task<StockQuoteResponse> GetStockQuoteAsync(string stockCode)
        {
            using (httpClient)
            {
                var response = await httpClient.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
               
                using (var streamReader = new StreamReader(response.Content.ReadAsStream()))
                {
                    var columns = streamReader.ReadLine();
                    var values = streamReader.ReadLine();

                    return StockQuoteCsvMapper.ToResponse(values);
                }
            }
        }

        #endregion

        #region Private Methods

        private void ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(AppConfiguration.Settings.StooqApiBaseAddress);
            httpClient.Timeout = AppConfiguration.Settings.StooqApiTimeout;
        }

        #endregion
    }
}