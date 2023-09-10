using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System.Net;

namespace DbPostGetCall.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ILogger<ParkingLotController> _logger;
        private readonly IGetLot _getLot;

        public ParkingLotController(ILogger<ParkingLotController> logger, IGetLot getLot)
        {
            _logger = logger;
            _getLot = getLot ?? throw new ArgumentNullException(nameof(getLot));
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetCars()
        {
            var response = await _getLot.GetCars();

            string output = (string)response.Data;

            return output;
        }

        [HttpGet("{carId}")]
        public async Task<ActionResult<string>> GetCar(int carId)
        {
            var response = await _getLot.GetTakenLot(carId);

            string output = (string)response.Data;

            return output;
        }

        [HttpPost()]
        public async Task<ActionResult<HttpStatusCode>> AddCarToDb(string json)
        {
            ParkingLot newLot = JsonConvert.DeserializeObject<ParkingLot>(json) ?? new ParkingLot();

            var response = await _getLot.PostCarLot(newLot);

            return response.Status;
        }

        [HttpDelete("{carId}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteDiscount(int carId)
        {
            var response = await _getLot.RemoveCarFromLot(carId);

            return response.Status;
        }
    }
}
