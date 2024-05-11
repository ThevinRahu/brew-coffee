using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeMachineAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace api.Tests
{
    public class CoffeeControllerTests
    {
        [Fact]
        //200 ok unit test to check the response when it should be 200 ok 
        public async Task CoffeeMachine_200_OK_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            var result = await controller.CoffeeMachine() as OkObjectResult;

            //using http client to call api and get response
            using (HttpClient client = new HttpClient())
            {
                string apiKey = "17bd70b483cac66319d04a1fd97ec36c";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={-36.8509}&lon={174.7645}&appid={apiKey}";
                HttpResponseMessage results = await client.GetAsync(apiUrl);

                string responseData = await results.Content.ReadAsStringAsync();

                double temp;

                using (JsonDocument doc = JsonDocument.Parse(responseData))
                {
                    var root = doc.RootElement;
                    var weather = root.GetProperty("weather")[0];
                    var main = root.GetProperty("main");
                    temp = main.GetProperty("temp").GetDouble();
                }
                
                /* if needed we can change the temperature manually here as a hard code and do the testing*/

                double tempC = temp - 273.15;
                var response = new
                {
                    message = tempC > 30.00 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready",
                    prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                };

                if (DateTime.Today.Month != 4 && DateTime.Today.Day != 1)
                {
                    Assert.Equal(200, result.StatusCode);
                    Assert.Equal(response, result.Value);
                }
                Assert.NotNull(result.Value);

            }


        }

        [Fact]
        //5th call unit test to check the response when it should be 503 response 
        public async Task CoffeeMachine_5th_Call_503_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            //run the api 5 times to check
            for (int i = 1; i <= 5; i++)
            {
                var result = await controller.CoffeeMachine() as ObjectResult;

                // Assert on the fifth call
                if (i == 5)
                {
                    // Assert
                    if (result != null)
                    {
                        Assert.Equal(503, result.StatusCode);
                    }
                    else
                    {
                        Assert.Null(result);
                    }
                }
                else
                {
                    // Assert on the first four calls
                    Assert.Equal(200, result.StatusCode);
                    Assert.NotNull(result.Value);
                }
            }
        }

        [Fact]
        //april 1st unit test to check the response when it should be 418 response 
        public async Task CoffeeMachine_April_1st_Call_418_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            var result = await controller.CoffeeMachine() as ObjectResult;

            // Assert
            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                Assert.Equal(418, result.StatusCode);
                Assert.Equal("I'm a teapot", result.Value);
            }
            Assert.NotNull(result);
        }

    }


}
