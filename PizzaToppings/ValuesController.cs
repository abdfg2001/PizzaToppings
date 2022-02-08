using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PizzaToppings
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string GetToppings()
        {
            var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Toppings/pizzas.json");
            string JSON = System.IO.File.ReadAllText(folderDetails);
            return JSON;
        }
    }
}

//https://localhost:7221/api/Values