using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IRandomTripService
    {
        Task<IEnumerable<IRouteModel>> GetRoutes();


    }
}