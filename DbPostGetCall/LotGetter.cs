using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using System.Xml;

namespace DbPostGetCall
{
    public class LotGetter : IGetLot
    {
        const string connectionString = "ConnectionStrings:ConnectionString";

        const string selectAllCall = "SELECT json_agg(parkingLots) FROM parkingLots";
        const string selectByIdCall = "SELECT json_agg(parkingLots) FROM parkingLots WHERE Id  = @Id";
        const string sqlDeleteCall = "DELETE FROM ParkingLots WHERE Id  = @Id ";
        const string postNewLotCall = "INSERT INTO ParkingLots (Id, CarModel, CarNumber, LotNumber) VALUES (@Id, @CarModel, @CarNumber, @LotNumber)";

        private readonly IConfiguration _configuration;

        public LotGetter(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //GET
        public async Task<MyResponse> GetCars()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var cars = await connection.QueryAsync<string>(selectAllCall);

            if (cars == null)
            {
                return new MyResponse() { Status = HttpStatusCode.NotFound};
            }

            return new MyResponse() { Status = HttpStatusCode.OK, Data = cars.ToArray()[0]};
        }

        //GET BY ID
        public async Task<MyResponse> GetTakenLot(int carId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var cars = await connection.QueryAsync<string>(selectByIdCall,
                new { Id = carId });

            if (cars == null)
            {
                return new MyResponse() { Status = HttpStatusCode.NotFound };
            }

            return new MyResponse() { Status = HttpStatusCode.OK, Data = cars.ToArray()[0] };
        }

        //POST
        public async Task<MyResponse> PostCarLot(ParkingLot lot)
        {

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var affected = await connection.ExecuteAsync(postNewLotCall,
                            new { lot.Id, lot.CarModel, lot.CarNumber, lot.LotNumber });

            if (affected == 0)
            {
                return new MyResponse() { Status = HttpStatusCode.Conflict };
            }

            return new MyResponse() { Status = HttpStatusCode.OK, Data = true };
        }

        //DELETE
        public async Task<MyResponse> RemoveCarFromLot(int idToDelete)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var affected = await connection.ExecuteAsync(sqlDeleteCall,
                new { Id = idToDelete });

            if (affected == 0)
            {
                return new MyResponse() { Status = HttpStatusCode.Conflict};
            }

            return new MyResponse() { Status = HttpStatusCode.OK, Data = true};
        }
    }
}