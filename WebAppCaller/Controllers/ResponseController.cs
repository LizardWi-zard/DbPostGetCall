using DbPostGetCall;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace WebAppCaller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private static readonly string[] Models = new[]
        {
            "BMW", "Dodge", "Toyota", "Mercedes", "Volkswagen", "Reno", "Mazda"
        };

        private readonly ILogger<ResponseController> _logger;

        public ResponseController(ILogger<ResponseController> logger)
        {
            _logger = logger;
        }

        public static async Task<string> AskForCars()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7058");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("ParkingLot").Result;
                response.EnsureSuccessStatusCode();

                var responseAsString = await response.Content.ReadAsStringAsync();

                var responseAsJson = JsonConvert.DeserializeObject<string>(responseAsString);

                return responseAsJson;
            }

            return "call failed";
        }
      
        public static async Task<bool> AddToAllCars()
        {
            ParkingLot newLot = new ParkingLot();
            newLot.Id = Random.Shared.Next(1, 100);
            newLot.CarModel = Models[Random.Shared.Next(Models.Length)];
            newLot.CarNumber = Random.Shared.Next(1000, 10000);
            newLot.LotNumber = Random.Shared.Next(1, 2000);

            var newLotAsJson = JsonConvert.SerializeObject(newLot);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7058");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync($"ParkingLot?json={newLotAsJson}", null);

                response.EnsureSuccessStatusCode();

                return true;
            }

            return false;
        }

        [HttpGet(Name = "GetCall")]
        public async Task<ActionResult<string>> GetCars()
        {
            var models = await AskForCars();

            return Ok(models);
        }

        [HttpPost(Name = "GetCall")]
        public async Task<OkResult> AddCar()
        {
            await AddToAllCars();

            return Ok();
        }

    }
}