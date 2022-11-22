using Shared.Kernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IPaddlePrintRepo
    {
        Task<IEnumerable<IPaddlePrintModel>> GetPaddles(ScheduleType scheduleType, string effectiveDate, int dutyNumber);
    }
}