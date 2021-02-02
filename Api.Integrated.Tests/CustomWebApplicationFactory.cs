using Api;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Integrated.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        private readonly ISQLConnectionFactory _mockSqlConnectionFactory;

        public CustomWebApplicationFactory()
        {
            _mockSqlConnectionFactory = new MockSqlConnectionFactory();
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.SwapTransient<ISQLConnectionFactory>(provider => _mockSqlConnectionFactory);

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Seed the database with some specific test data.
                DatabaseBootstrap.Setup();

            });
        }
    }
}


