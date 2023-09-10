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

        [HttpGet(Name = "All")]
        public async Task<ActionResult<string>> GetCars()
        {
            var models = await _getLot.GetCars();
            return models;
        }

        [HttpGet("{carId}", Name = "Single")]
        public async Task<ActionResult<string>> GetCar(int carId)
        {
            var models = await _getLot.GetTakenLot(carId);
            return Ok(models);
        }

        //[HttpPost()]
        //public async Task<OkResult> AddCarToDb(int id, string model, int carNumber, int lotNumber)
        //{
        //    var newLot = new ParkingLot() { Id = id, CarModel = model, CarNumber = carNumber, LotNumber = lotNumber };
        //
        //    await _getLot.PostCarLot(newLot);
        //    return Ok();
        //}

        [HttpPost()]
        public async Task<OkResult> AddCarToDb(string json)
        {
            ParkingLot newLot = JsonConvert.DeserializeObject<ParkingLot>(json);

            await _getLot.PostCarLot(newLot);
            return Ok();
        }

        [HttpDelete("{carId}", Name = "Delete")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(int carId)
        {
            return Ok(await _getLot.RemoveCarFromLot(carId));
        }
    }
}
