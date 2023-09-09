using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Xml;

namespace DbPostGetCall
{
    public class LotGetter : IGetLot
    {
        private readonly IConfiguration _configuration;

        public LotGetter(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> GetCars()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));

            //var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[nameOfString].ConnectionString);

            var cars = await connection.QueryAsync<string>("SELECT json_agg(parkingLots) FROM parkingLots");

            if (cars == null)
            {
                return "Null or empty";
            }

            return cars.ToArray()[0];
        }


        public async Task<bool> RemoveCarFromLot(int idToDelete)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM ParkingLots WHERE Id  = @Id ",
                new { Id = idToDelete });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> PostCarLot(ParkingLot lot)
        {
            

          // string sqlcarInsert = $"INSERT INTO ParkingLots (Id, CarModel, CarNumber, LotNumber) VALUES (@Id, @CarModel, @CarNumber, @LotNumber)";

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));

            var affected = await connection.ExecuteAsync("INSERT INTO ParkingLots (Id, CarModel, CarNumber, LotNumber) VALUES (@Id, @CarModel, @CarNumber, @LotNumber)",
                            new { lot.Id, lot.CarModel, lot.CarNumber, lot.LotNumber});

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}

        //public async Task<ParkingLot> GetTakenLot(int id)
        //{
        //    using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        //
        //    var car = await connection.QueryAsync<ParkingLot>("SELECT * FROM ParkingLots WHERE Id = @Id", new { Id = id});
        //
        //    if (car == null)
        //    {
        //        return new ParkingLot();
        //    }
        //
        //    return (ParkingLot)car;
        //}
