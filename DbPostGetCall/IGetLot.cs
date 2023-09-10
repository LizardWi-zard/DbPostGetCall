using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;

namespace DbPostGetCall
{
    public interface IGetLot
    {
        Task<string> GetCars();
     
        Task<string> GetTakenLot(int Id);
        
        Task<bool> RemoveCarFromLot(int parkingLot);
        
        Task<bool> PostCarLot(ParkingLot lot);
    }
}
