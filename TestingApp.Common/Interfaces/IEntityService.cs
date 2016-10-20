using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Common
{
    public interface IEntityService : IDisposable
    {
        TResult GetService<TResult>()
            where TResult : IEntityService, new();

        Exception HandleExecutionException(Exception ex, DbContextTransaction transaction = null);

        void RollbackTransaction(DbContextTransaction transaction);
    }
}
