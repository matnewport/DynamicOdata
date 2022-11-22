using Shared.Kernel.Enums;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.MockServices
{
    internal class PaddlePrintService : IPaddlePrintService
    {
        public Task<IEnumerable<IPaddlePrintModel>> GetPaddlePrint(PickType pickType, DayOfWeek dayOfWeek, int dutyNumber)
        {
            throw new NotImplementedException();
        }

        public string GetPaddlePrintHtml(IEnumerable<IPaddlePrintModel> paddles)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPaddlePrintModel>> GetPaddles(ScheduleType scheduleType, DateTime effectiveDate, int dutyNumber)
        {
            throw new NotImplementedException();
        }
    }
}
