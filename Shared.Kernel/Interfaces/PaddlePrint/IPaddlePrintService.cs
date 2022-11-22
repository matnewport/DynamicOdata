using Shared.Kernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IPaddlePrintService
    {
        Task<IEnumerable<IPaddlePrintModel>> GetPaddles(ScheduleType scheduleType, DateTime effectiveDate, int dutyNumber);

        string GetPaddlePrintHtml(IEnumerable<IPaddlePrintModel> paddles);
        Task<IEnumerable<int>> CheckDutyNumber(int dutyNumber);
    }
}