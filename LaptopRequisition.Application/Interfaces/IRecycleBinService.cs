using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IRecycleBinService
    {
        Task CleanUpRecycleBinAsync();
    }
}