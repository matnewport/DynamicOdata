using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Kernel.Interfaces
{
    public interface IHasId<T>
       where T : struct
    {
        T Id { get; }
    }
}
