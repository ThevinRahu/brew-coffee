using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CoffeeMachine_200_OK_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            var result = controller.CoffeeMachine() as OkObjectResult;

            var response = new
            {
                message = "Your piping hot coffee is ready",
                prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };

            if (DateTime.Today.Month != 4 && DateTime.Today.Day != 1)
            {
                Assert.Equal(200, result.StatusCode);
                Assert.Equal(response, result.Value);
            }
            Assert.NotNull(result.Value);


        }

        [Fact]
        //5th call unit test to check the response when it should be 503 response 
        public void CoffeeMachine_5th_Call_503_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            //run the api 5 times to check
            for (int i = 1; i <= 5; i++)
            {
                var result = controller.CoffeeMachine() as ObjectResult;

                // Assert on the fifth call
                if (controller.GetRequests() == 5)
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
        public void CoffeeMachine_April_1st_Call_418_Test()
        {
            // Arrange
            var controller = new CoffeeController();

            // Act
            var result = controller.CoffeeMachine() as ObjectResult;

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
