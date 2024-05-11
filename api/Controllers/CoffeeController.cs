using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoffeeMachineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private static int requests = 0;

        //brew-coffee api controller function
        [HttpGet("/brew-coffee")]
        public async Task<IActionResult> CoffeeMachine()
        {
            requests++;

            //return 418 when april 1st
            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                return StatusCode(418, "I'm a teapot");
            }
            //return 503 on every 5th call
            else if (requests % 5 == 0)
            {
                return StatusCode(503);
            }
            //ok result with message
            else
            {
                //using http client to call api and get response
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string apiKey = "17bd70b483cac66319d04a1fd97ec36c";
                        string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={-36.8509}&lon={174.7645}&appid={apiKey}";
                        HttpResponseMessage result = await client.GetAsync(apiUrl);

                        //get results as string
                        string responseData = await result.Content.ReadAsStringAsync();

                        double temp;
                        //convert string to JSON object
                        using (JsonDocument doc = JsonDocument.Parse(responseData))
                        {
                            var root = doc.RootElement;
                            var weather = root.GetProperty("weather")[0];
                            var main = root.GetProperty("main");
                            temp = main.GetProperty("temp").GetDouble();
                        }
                        //convert kelvin to celsius
                        double tempC = temp - 273.15;
                        //check condition and set valid message
                        var response = new
                        {
                            message = tempC > 30.00 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready",
                            prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        };
                        return Ok(response);

                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        public int GetRequests(){
            return requests;
        }
    }
}
