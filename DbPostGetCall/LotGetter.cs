using Dapper;
using Npgsql;

namespace DbPostGetCall
{
    public class LotGetter : IGetLot
    {
        private readonly IConfiguration _configuration;

        public LotGetter(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string[]> GetCarModels()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var carModels = await connection.QueryAsync<string>("SELECT CarModel FROM ParkingLots GROUP BY Id");

            if (carModels == null)
            {
                return new string[] { "Null or empty" };
            }

            return carModels.ToArray();
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

        public async Task<bool> RemoveCarFromLot(int parkingLot)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM ParkingLots WHERE LotNumber  = @LotNumber ",
                new { LotNumber = parkingLot });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> PostCarLot()
        {
            int Id = Random.Shared.Next(1, 100);
            string CarModel = "Nissan";
            int CarNumber = Random.Shared.Next(1000, 10000);
            int LotNumber = Random.Shared.Next(1, 2000);

            string sqlcarInsert = $"INSERT INTO ParkingLots (Id, CarModel, CarNumber, LotNumber) VALUES ({Id}, {CarModel}, {CarNumber}, {LotNumber})";

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync(sqlcarInsert);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}
