using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CoffeeMachineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private static int requests = 0;

        //brew-coffee api controller function
        [HttpGet("/brew-coffee")]
        public IActionResult CoffeeMachine()
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
                var response = new
                {
                    message = "Your piping hot coffee is ready",
                    prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
                };
                return Ok(response);
            }
        }
        
        public int GetRequests(){
            return requests;
        }
    }
}
