using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Kernel.Interfaces
{
    public interface IGenericCrud<TRecord, TId>
      where TRecord : class, IHasId<TId>
      where TId : struct
    {
        int Delete(TId key);
        void Dispose();
        IEnumerable<TRecord> GetAll();
        TRecord GetOne(string value);
        TRecord GetOne(TId key);
        int Insert(TRecord record);
        Task<int?> InsertAsync(TRecord record);
        void Update(TRecord record);
    }
}
