using Shared.Kernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IScheduleDBRepo
    {
        Task<IEnumerable<IRouteModel>> GetRoutes();

        Task<IEnumerable<IPaddlePrintModel>> GetPaddles(ScheduleType scheduleType, string effectiveDate, int dutyNumber);

        Task<IEnumerable<int>> CheckDutyNumber(int dutyNumber);

        string GetConnectionString();
    }
}