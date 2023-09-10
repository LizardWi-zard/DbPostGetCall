using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;

namespace DbPostGetCall
{
    public interface IGetLot
    {
        Task<MyResponse> GetCars();
     
        Task<MyResponse> GetTakenLot(int Id);
        
        Task<MyResponse> RemoveCarFromLot(int parkingLot);
        
        Task<MyResponse> PostCarLot(ParkingLot lot);
    }
}
