using Dapper;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GenericCrud
{
 

    public abstract class GenericCrud<TRecord, TId> : IDisposable, IGenericCrud<TRecord, TId> where TRecord : class, IHasId<TId>
        where TId : struct
    {
        protected GenericCrud(IDbConnection conn)
        {
            Conn = conn;
        }


        protected IDbConnection Conn { get; }

        public int Insert(TRecord record)
        {
            Debug.WriteLine("Insert");
            var result = Conn.Insert(record);
            if (!result.HasValue)
                return 0;
            else
                return result.Value;
        }

        public async System.Threading.Tasks.Task<int?> InsertAsync(TRecord record)
        {
            Debug.WriteLine("InsertAsync");
            return await Conn.InsertAsync(record);
            //if (!result.HasValue)
            //    return 0;
            //else
            //    return result.Value;
        }
        public int Delete(TId key)
        {
            return Conn.Delete<TRecord>(key);
        }

        public abstract TRecord GetOne(string value);

        public TRecord GetOne(TId key)
        {
            Debug.WriteLine("GetOne");
            return Conn.Get<TRecord>(key);
        }

        public IEnumerable<TRecord> GetAll()
        {
            Debug.WriteLine("GetAll");
            return Conn.GetList<TRecord>();
        }

        public void Update(TRecord record)
        {
            Conn.Update(record);
        }

        public abstract void Dispose();
    }
}