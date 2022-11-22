using Dapper;
using Microsoft.Extensions.Configuration;
using Tamp.Repositories.Models;
using Shared.Kernel.Enums;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Azure.Security.KeyVault.Secrets;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Tamp.Repositories
{
    public class ScheduleDBRepo : DbRepositoryBase, IScheduleDBRepo, IPaddlePrintRepo
    {
        public ScheduleDBRepo(IConfiguration config) : base(config, "ScheduleDB")
        {
        }

        public ScheduleDBRepo(string conn) : base(conn, "Microsoft.Data.SqlClient")
        {
        }

        protected override void DefineMappings()
        {
            //not needed
        }

        public new void Dispose()
        {
        }

        public async Task<IEnumerable<IPaddlePrintModel>> GetPaddles(

            ScheduleType scheduleType,
            string scheduleDate,
            int dutyNumber)
        {
            var sql = ReadCommandText("GetPaddles");

            var retval = await this.Connection.QueryAsync<PaddlePrintModel>(sql,
             new
             {
                 ScheduleTypeName = scheduleType.ToString(),
                 ScheduleDate = scheduleDate,
                 DutyNumber = dutyNumber.ToString()
             });

            return retval;
        }

        //public async Task<IEnumerable<int>> CheckDutyNumber(

        //    int dutyNumber)
        //{
        //    var retval = await this.Connection.QueryAsync<IEnumerable<int>>($"select count(1) from DutyPieces where DutyNumber = {dutyNumber} and ReportDate >= ValidFrom and @ReportDate <= ValidTo",
        //     new
        //     {
        //         ReportDate = DateTime.Now,
        //         DutyNumber = dutyNumber.ToString()
        //     });

        //    return retval.PickRandom();
        //}

        public async Task<IEnumerable<IRouteModel>> GetRoutes()
        {
            var query = @"
						SELECT r.RouteId AS RouteId
						From dbo.[Routes] r
						Group By r.RouteID
						";
            IEnumerable<IRouteModel> retval = await Connection.QueryAsync<RouteModel>(query);

            return retval;
        }

        public async Task<IEnumerable<int>> CheckDutyNumber(int dutyNumber)
        {
            string query = "select DutyNumber from DutyPieces where DutyNumber =  " + dutyNumber.ToString() + " and  '" + DateTime.Now.ToString("MM/dd/yyyy") + "' >= ValidFrom  and  '" + DateTime.Now.ToString("MM/dd/yyyy") + "' <= ValidTo";
            IEnumerable<int> retval = await Connection.QueryAsync<int>(query);

            return retval;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public string GetConnectionString()
        {
            return this.ConnectionString;
        }
    }
}