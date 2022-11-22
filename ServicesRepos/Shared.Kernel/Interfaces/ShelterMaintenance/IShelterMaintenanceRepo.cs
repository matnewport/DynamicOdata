using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IShelterMaintenanceRepo
    {


        Task<IEnumerable<IShelterModel>> GetShelters();
        IGenericCrud<IShelterModel, int> GetShelterCrud();
    }
}