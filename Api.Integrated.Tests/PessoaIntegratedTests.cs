using Domain.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Api.Integrated.Tests
{
    public class PersonIntegratedTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public PersonIntegratedTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Person_Returns_200()
        {
            var httpResponse = await _client.GetAsync("/Person/11111111111");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var Person = JsonConvert.DeserializeObject<Person>(stringResponse);

            Person
                .Should()
                .NotBeNull();

            Person
                .CPF
                .Should()
                .Be(11111111111);

            httpResponse
                .StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            httpResponse.Content.Headers.ContentType.ToString()
                .Should()
                .Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task Get_Person_Returns_204()
        {
            var httpResponse = await _client.GetAsync("/Person/22222222222");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var Person = JsonConvert.DeserializeObject<Person>(stringResponse);
            Person
                .Should()
                    .BeNull();

            httpResponse
                 .StatusCode
                 .Should()
                 .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Get_Person_Returns_400()
        {
            var httpResponse = await _client.GetAsync("/Person/abcd");

            httpResponse
             .StatusCode
             .Should()
             .Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Get_Person_Returns_404()
        {
            var httpResponse = await _client.GetAsync("/Person");

            httpResponse
             .StatusCode
             .Should()
             .Be(HttpStatusCode.NotFound);
        }
    }
}
