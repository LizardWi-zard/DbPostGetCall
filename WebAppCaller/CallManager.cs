using DbPostGetCall;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace WebAppCaller
{
    public class CallManager : IResponseCall
    {
        const string baseAddres = "https://localhost:7058";

        private static readonly string[] Models = new[]
{
            "BMW", "Dodge", "Toyota", "Mercedes", "Volkswagen", "Reno", "Mazda"
        };

        //GET
        public async Task<MyResponse> GetCars()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddres);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("ParkingLot").Result;
                response.EnsureSuccessStatusCode();

                var responseAsString = await response.Content.ReadAsStringAsync();

                var responseAsJson = JsonConvert.DeserializeObject<string>(responseAsString);

                if (response != null)
                {
                    return new MyResponse() { Status = HttpStatusCode.OK, Data = responseAsJson };
                }
            }

            return new MyResponse() { Status = HttpStatusCode.BadRequest };
        }

        //POST
        public async Task<MyResponse> AddCar()
        {
            ParkingLot newLot = new ParkingLot();
            newLot.Id = Random.Shared.Next(1, 100);
            newLot.CarModel = Models[Random.Shared.Next(Models.Length)];
            newLot.CarNumber = Random.Shared.Next(1000, 10000);
            newLot.LotNumber = Random.Shared.Next(1, 2000);

            var newLotAsJson = JsonConvert.SerializeObject(newLot);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddres);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync($"ParkingLot?json={newLotAsJson}", null);

                response.EnsureSuccessStatusCode();

                if (response != null)
                {
                    return new MyResponse() { Status = HttpStatusCode.OK, Data = true };
                }

            }
                
            return new MyResponse() { Status = HttpStatusCode.Conflict };
        }
    }
}
