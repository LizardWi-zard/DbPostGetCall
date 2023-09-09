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

        [HttpGet(Name = "GetCarModels")]
        public async Task<ActionResult<string>> GetCarModels()
        {
            var models = await _getLot.GetCarModels();
            return Ok(models);
        }

        [HttpPost(Name = "AddItemsToDB")]
        public async Task<ActionResult<bool>> AddCarToDb()
        {
            await _getLot.PostCarLot();
            return true;

        }

        [HttpDelete("{parlingLot}", Name = "DeleteCar")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(int parlingLot)
        {
            return Ok(await _getLot.RemoveCarFromLot(parlingLot));
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