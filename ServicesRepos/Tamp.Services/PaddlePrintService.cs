using Microsoft.Extensions.Configuration;
using Tamp.Repositories;
using Shared.Kernel.Enums;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Tamp.Services
{
    public sealed class PaddlePrintService : IPaddlePrintService
    {
        private IScheduleDBRepo repo;

        public PaddlePrintService(IConfiguration _config)
        {
            repo = new ScheduleDBRepo(_config);
        }

        public PaddlePrintService(string conn)
        {
            repo = new ScheduleDBRepo(conn);
        }

        public string GetPaddlePrintHtml(IEnumerable<IPaddlePrintModel> paddles)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IPaddlePrintModel>> GetPaddles(ScheduleType scheduleType, DateTime effectiveDate, int dutyNumber)
        {
            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new InterfaceContractResolver(typeof(IPaddlePrintModel))
            //};
            IEnumerable<IPaddlePrintModel> result = null;
            try
            {
                result = await repo.GetPaddles(scheduleType, effectiveDate.ToString("yyyyMMdd"), dutyNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\r\n{this.repo.GetConnectionString()}\r\n{Environment.UserName}");
            }
            //JsonConvert.SerializeObject(result, settings);
            return result;
        }

        public async Task<IEnumerable<int>> CheckDutyNumber(int dutyNumber)
        {
            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new InterfaceContractResolver(typeof(IPaddlePrintModel))
            //};
            IEnumerable<int> result = null;
            try
            {
                result = await repo.CheckDutyNumber(dutyNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\r\n{this.repo.GetConnectionString()}\r\n{Environment.UserName}");
            }
            //JsonConvert.SerializeObject(result, settings);
            return result;
        }
    }
}