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
           // ParkingLot lot = JsonConvert.DeserializeObject<ParkingLot>(lotJson);

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));

            var affected = await connection.ExecuteAsync("INSERT INTO ParkingLots (Id, CarModel, CarNumber, LotNumber) VALUES (@Id, @CarModel, @CarNumber, @LotNumber)",
                            new { lot.Id, lot.CarModel, lot.CarNumber, lot.LotNumber});

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetTakenLot(int carId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));

            var cars = await connection.QueryAsync<string>("SELECT json_agg(parkingLots) FROM parkingLots WHERE Id  = @Id",
                new { Id = carId });

            if (cars == null)
            {
                return "Null or empty";
            }

            return cars.ToArray()[0];
        }
    }
}