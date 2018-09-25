using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Moq.Protected;
using Repositories;

namespace HealthAPI.Acceptance.Tests
{
    public class TestStartup : Startup
    {
        Uri _capturedUri = new Uri("http://www.null.com");

        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<HealthContext>(options =>
            {
                options.UseInMemoryDatabase("fake");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var descriptorFitbitService = new ServiceDescriptor(
                typeof(IFitbitService),
                typeof(FitbitServiceStub),
                ServiceLifetime.Singleton);

            services.Replace(descriptorFitbitService);

            var _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri);

            var _httpClient = new HttpClient(_httpMessageHandler.Object);

            //var descriptor
            var descriptorHttpClient = new ServiceDescriptor(
                typeof(HttpClient),
                _httpClient);

            services.Replace(descriptorHttpClient);



        }
    }

}

