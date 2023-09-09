using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpGet(Name = "Cars")]
        public async Task<ActionResult<string>> GetCars()
        {
            Console.WriteLine("Get another method call");


            var models = await _getLot.GetCars();
            return Ok(models);
        }

        [HttpPost(Name = "ItemsToDB")]
        public async Task<ActionResult<bool>> AddCarToDb()
        {
            int newCarId = Random.Shared.Next(1, 100);
            string newCarModel = "Honda";
            int newCarNumber = Random.Shared.Next(1000, 10000);
            int newLotNumber = Random.Shared.Next(1, 2000);

            ParkingLot newLot = new ParkingLot();
            newLot.Id = newCarId;
            newLot.CarModel = newCarModel;
            newLot.CarNumber = newCarNumber;
            newLot.LotNumber = newLotNumber;

            await _getLot.PostCarLot(newLot);
            return true;

        }

        [HttpDelete("{carId}", Name = "DeleteCar")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(int carId)
        {
            return Ok(await _getLot.RemoveCarFromLot(carId));
        }

    }
}


//  [HttpGet(Name = "GetTakenLots")]
//  public IEnumerable<ParkingLot> Get()
//  {
//      return Enumerable.Range(1, 5).Select(index => new ParkingLot
//      {
//          Id = Random.Shared.Next(1, 100),
//          CarModel = "Nissan", 
//          CarNumber = Random.Shared.Next(1000, 10000),
//          LotNumber = Random.Shared.Next(1, 2000)
//      })
//      .ToArray();
//  }