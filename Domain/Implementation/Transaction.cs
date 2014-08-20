using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class Transaction : ITransaction
    {
        private readonly DbContext _context;

        private DbContextTransaction _transaction;

        private bool _disposed;

        public Transaction(DbContext context)
        {
            _context = context;
            State = TransactionState.NotStarted;
        }

        public TransactionState State { get; private set; }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction(IsolationLevel.Snapshot);
            State = TransactionState.Started;
        }

        public void Commit()
        {
            if (State == TransactionState.Started)
            {
                _transaction.Commit();
                State = TransactionState.Commited;
            }
        }

        public void Rollback()
        {
            if (State == TransactionState.Started)
            {
                _transaction.Rollback();
                State = TransactionState.RolledBack;
            }
        }

        public void Dispose()
        {
            if (!_disposed && _transaction != null)
            {
                ResolveTransaction();
                _transaction.Dispose();
                _transaction = null;
            }

            _disposed = true;
        }

        private void ResolveTransaction()
        {
            if (State == TransactionState.Started)
            {
                Rollback();

                throw new InvalidOperationException("Transaction must be resolved before disposing");
            }
        }
    }
}
