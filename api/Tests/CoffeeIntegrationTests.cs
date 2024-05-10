using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMachineAPI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace api.Tests
{
    public class CoffeeControllerIntegrationTests 
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public CoffeeControllerIntegrationTests()
        {
            // Arrange
            //cilent server connection
            _server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                }));

            _client = _server.CreateClient();
        }

        [Fact]
        //integration test to check the response anc check client server integration with api
        public async Task BrewCoffee_Returns_200OK_With_DateTime()
        {
            // Act
            var response = await _client.GetAsync("/brew-coffee");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}