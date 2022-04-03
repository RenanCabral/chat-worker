using ChatAppBot.CrossCutting;
using ChatAppBot.IntegrationServices.ThirdParty.Stooq;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace ChatAppBot.IntegrationServices.Tests.ThirdParty.Stooq
{
    public class StooqIntegrationServiceTest
    {
        public StooqIntegrationServiceTest()
        {
            AppConfiguration.Settings = new Settings()
            {
                StooqApiBaseAddress = "https://stooq.com/q/l/",
                StooqApiTimeout = TimeSpan.FromSeconds(30)
            };
        }

        [Fact]
        public async void When_Getting_StockQuote_With_Valid_Parameters_Expect_Valid_Response()
        {
            //arrange
            var mockHttp = new MockHttpMessageHandler();

            var csvContent = @"Symbol,Date,Time,Open,High,Low,Close,Volume
AAPL.US,2022-03-25,21:00:05,173.88,175.28,172.75,174.72,65781668";

            var headers = new Dictionary<string, string>()
            {
                {"content-type", "text/csv; charset=utf-8" },
                {"content-disposition", "attachment;filename=aapl.us.csv"}
            };

            mockHttp.When("https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv")
                    .Respond(HttpStatusCode.OK, headers, new StringContent(csvContent));

            var mockHttpFactory = new Mock<IHttpClientFactory>();
            mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttp.ToHttpClient());

            var stooqIntegrationService = new StooqIntegrationService(mockHttpFactory.Object);

            //act
            var result = await stooqIntegrationService.GetStockQuoteAsync("aapl.us");

            //assert
            Assert.NotNull(result);
            Assert.Equal("AAPL.US", result.Symbol);
            Assert.Equal("2022-03-25", result.Date);
            Assert.Equal("21:00:05", result.Time);
            Assert.Equal("173.88", result.Open);
            Assert.Equal("175.28", result.High);
            Assert.Equal("172.75", result.Low);
            Assert.Equal("174.72", result.Close);
            Assert.Equal("65781668", result.Volume);
        }
    }
}