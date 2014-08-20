using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ITransaction : IDisposable
    {
        TransactionState State { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
