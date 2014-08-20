using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class UnitOfWork : IUserManagerFactory, IUnitOfWork
    {
        private readonly IUnityContainer _unityContainer;

        private readonly IDbContextProvider _dbContextProvider;

        private readonly ITransaction _transaction;

        private DbContext _context;

        private bool _disposed;

        public UnitOfWork(
            IUnityContainer unityContainer,
            IDbContextProvider dbContextProvider)
        {
            _unityContainer = unityContainer;
            _dbContextProvider = dbContextProvider;

            _transaction = CreateWithContext<ITransaction>();      
        }

        ~UnitOfWork()
        {
            throw new InvalidOperationException("Object must be disposed");
        }

        public ITransaction GetTransaction()
        {
            return _transaction;
        }

        public UserManager<TUser> GetUserManager<TUser>() where TUser : class, IUser<string>
        {
            BeginTransactionIfNotStarted();

            var userStore = CreateWithContext<IUserStore<TUser>>();
            return new UserManager<TUser>(userStore);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            BeginTransactionIfNotStarted();

            return CreateWithContext<IRepository<TEntity>>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (!_disposed)
            {
                // наверняка завершем трансакцию
                _transaction.Dispose();
                Context.Dispose();
            }

            _disposed = true;
        }

        private DbContext Context
        {
            get
            {
                if (_context != null)
                {
                    return _context;
                }

                _context = _dbContextProvider.CreateContext();
                return _context;
            }
        }

        private void BeginTransactionIfNotStarted()
        {
            if (_transaction.State == TransactionState.NotStarted)
            {
                _transaction.BeginTransaction();
            }
            else if (_transaction.State != TransactionState.Started)
            {
                throw new InvalidOperationException("Trasnsaction has been closed");
            }
        }

        private TContract CreateWithContext<TContract>()
        {
            return _unityContainer.Resolve<TContract>(new ParameterOverride("context", Context));
        }
    }
}
