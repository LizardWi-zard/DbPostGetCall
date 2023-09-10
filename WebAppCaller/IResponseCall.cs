using DbPostGetCall;

namespace WebAppCaller
{
    public interface IResponseCall
    {
        Task<MyResponse> GetCars();

        Task<MyResponse> AddCar();
    }
}
