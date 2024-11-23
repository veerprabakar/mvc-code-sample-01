using System.Diagnostics;
using Azure;
using Microsoft.AspNetCore.Mvc;
using MVC_Calls_Dynamics_API.Models;

namespace MVC_Calls_Dynamics_API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            await GetAccounts();
            return View();
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

        public async Task<IActionResult> GetAccounts()
        {
            var client = _httpClientFactory.CreateClient("DynamicsAPI");

            // Make API request
            var response = await client.GetAsync("accounts?$select=name");

            if (response != null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                // Deserialize and process the data as needed
                return Content(content, "application/json");
            }
            else
            {
                // Handle errors
                return new StatusCodeResult((int)response.StatusCode);
            }
        }
    }
}
