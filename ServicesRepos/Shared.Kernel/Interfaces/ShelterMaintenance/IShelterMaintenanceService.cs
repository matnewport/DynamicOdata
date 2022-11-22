using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IShelterMaintenanceService
    {
        Task<IEnumerable<IShelterModel>> GetShelters();
        IShelterModel GetShelter(int id);


    }
}