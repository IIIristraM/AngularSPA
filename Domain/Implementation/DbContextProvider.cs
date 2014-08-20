using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class DbContextProvider : IDbContextProvider
    {
        private bool _isInitialized;

        public DbContext CreateContext()
        {
            DbContext context = null;
            if (!_isInitialized)
            {
                context = Initialize<ApplicationDbContext>();
                SetSnapshotIsolation(context);

                _isInitialized = true;
            }           

            return context ?? new ApplicationDbContext();
        }

        private TContext Initialize<TContext>() where TContext: DbContext, new()
        {
            var context = new TContext();

            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
            else
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TContext>());
                context.Database.Initialize(true);
            }

            return context;
        }

        private void SetSnapshotIsolation(DbContext context)
        {
            bool isConnectionCloseNeeded = false;
            if (context.Database.Connection.State != ConnectionState.Open)
            {
                context.Database.Connection.Open();
                isConnectionCloseNeeded = true;
            }

            try
            {
                DbCommand command = context.Database.Connection.CreateCommand();
                command.CommandText = "SELECT db_name()";
                command.Transaction = null;

                object dbName = command.ExecuteScalar();

                command.CommandText = String.Format("ALTER DATABASE {0} SET ALLOW_SNAPSHOT_ISOLATION ON", dbName);
                command.Transaction = null;
                command.ExecuteNonQuery();

                command.CommandText = String.Format("ALTER DATABASE {0} SET READ_COMMITTED_SNAPSHOT ON", dbName);
                command.Transaction = null;
                command.ExecuteNonQuery();
            }
            finally
            {
                if (isConnectionCloseNeeded)
                {
                    context.Database.Connection.Close();
                }
            }
        }
    }
}
