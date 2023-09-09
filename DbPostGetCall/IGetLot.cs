using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;

namespace DbPostGetCall
{
    public interface IGetLot
    {
          //Task<ParkingLot> GetTakenLot(int Id);

          Task<string[]> GetCarModels();
          Task<bool> RemoveCarFromLot(int parkingLot);
          Task<bool> PostCarLot();
    }
}
