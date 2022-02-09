using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaToppings.Models;
using System.Diagnostics;

namespace PizzaToppings.Controllers
{
    public class HomeController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://pizzatoppings.azurewebsites.net/api/Values");
           
            if (response.IsSuccessStatusCode)
            {
                string JSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ToppingsClass[]>(JSON);
                return View("Index", result);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> ConvertToListAsync()
        {
            List<string> Topping = new();
            HttpResponseMessage response = await client.GetAsync("https://pizzatoppings.azurewebsites.net/api/Values");
            if (response.IsSuccessStatusCode)
            {
                string JSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ToppingsClass[]>(JSON);
                foreach (var Top in result)
                {
                    foreach (var Top1 in Top.toppings)
                    {
                        Topping.Add(Top1);
                    }
                }
                var Lista = Topping.Distinct().ToList();

                return View("ConvertToList", Lista);
            }
            else { return View("Error"); }
            
        }

        public async Task<IActionResult> OccurrencesToListAsync()
        {
            List<string> Topping = new();
            List<OccurrencesClass> Occurrences = new();
            HttpResponseMessage response = await client.GetAsync("https://pizzatoppings.azurewebsites.net/api/Values");
            if (response.IsSuccessStatusCode)
            {
                string JSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ToppingsClass[]>(JSON);
                foreach (var Top in result)
                {
                    foreach(var Top1 in Top.toppings)
                    {
                        Topping.Add(Top1);
                    }
                }

                var Lista = Topping.Distinct().ToList();

                foreach(var Top in Lista)
                {
                    var matchQuery = from word in Topping
                                     where word == Top
                                     select word;
                    int wordCount = matchQuery.Count();
                    var Temp = new OccurrencesClass();
                    Temp.Topping = Top;
                    Temp.Quantity = wordCount;
                    Occurrences.Add(Temp);
                }

                var enum1 = from user in Occurrences
                            orderby user.Quantity descending
                            select user;

                ViewData["Total"] =  enum1.Sum(s => s.Quantity);

                return View("OccurrencesToList", enum1.ToList());
            }
            else { return View("Error"); }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}