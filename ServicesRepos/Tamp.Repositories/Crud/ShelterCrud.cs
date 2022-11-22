using Dapper;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Tamp.Repositories.Models;

namespace Tamp.Repositories.Crud
{
    public class ShelterCrud : IGenericCrud<IShelterModel, int>
    {
        protected IDbConnection _conn { get; }
        public ShelterCrud(IDbConnection conn) : base()
        {
            _conn = conn;
        }
        public int Insert(IShelterModel record)
        {
           Debug.WriteLine("Insert");
            var result = _conn.Insert(record);
            if (!result.HasValue)
                return 0;
            else
                return result.Value;
        }

        public async Task<int?> InsertAsync(IShelterModel record)
        {
           Debug.WriteLine("InsertAsync");
            return await _conn.InsertAsync(record);
            //if (!result.HasValue)
            //    return 0;
            //else
            //    return result.Value;
        }
        public int Delete(int key)
        {
            return _conn.Delete<IShelterModel>(key);
        }

        public IShelterModel GetOne(string value)
        {
            throw new NotImplementedException();
        }

        public IShelterModel GetOne(int key)
        {
           Debug.WriteLine("GetOne");
            return _conn.Get<ShelterModel>(key);
        }

        public IEnumerable<IShelterModel> GetAll()
        {
           Debug.WriteLine("GetAll");
            return _conn.GetList<ShelterModel>();
        }

        public void Update(IShelterModel record)
        {
            _conn.Update(record);
        }

        public void Dispose()
        {
            
        }
    }
}
