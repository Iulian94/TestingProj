using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestingApp.Common;
using TestingApp.DA;

namespace TestingApp.BL
{
    public abstract class EntityServiceBase : IEntityService 
    {
        #region Properties

        private DbContext _mainContext;

        protected DbContext MainContext
        {
            get
            {
                if (_mainContext != null)
                    _mainContext = new TestContext();
                return _mainContext;
            }
            private set
            {
                _mainContext = value;
            }
        }

        protected IEntityService MainService
        {
            get
            {
                return this;
            }
        }

        private List<IEntityService> _entityServices { get; set; }

        private bool _isDisposed { get; set; }

        private bool _isCurrentlyDisposing { get; set; }

        #endregion

        #region Interface Implementations

        public TResult GetService<TResult>()
            where TResult : IEntityService, new()
        {
            if(MainService.GetType() == typeof(TResult))
            {
                return (TResult)MainService;
            }

            var entityService = (TResult)_entityServices.FirstOrDefault(c => c.GetType() == typeof(TResult));

            if (entityService != null)
            {
                return entityService;
            }

            entityService = new TResult();

            var entityServiceBase = entityService as EntityServiceBase;

            if(entityServiceBase != null)
            {
                entityServiceBase._mainContext = _mainContext;
                
                entityServiceBase._entityServices.AddRange(_entityServices);
                entityServiceBase._entityServices.Add(this);
            }

            _entityServices.Add(entityService);

            return entityService;
        }
        
        public Exception HandleExecutionException(Exception ex, DbContextTransaction transaction = null)
        {
            if(transaction != null)
            {
                RollbackTransaction(transaction);
            }

            //Log exception to db?
            //Rethrow custom exception

            return ex;
        }

        public void RollbackTransaction(DbContextTransaction transaction)
        {
            transaction.Rollback();
        }

        #region Disposable object implementation

        protected void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if(_mainContext != null)
                    {
                        _mainContext.Dispose();
                    }

                    foreach(var entityService in _entityServices)
                    {
                        var entityServiceBase = entityService as EntityServiceBase;
                        if (!entityServiceBase._isCurrentlyDisposing)
                        {
                            entityServiceBase.Dispose();
                        }
                    }

                    _entityServices.Clear();
                }

                _mainContext = null;
                _isDisposed = true;
            }
        }
        
        public void Dispose()
        {
            _isCurrentlyDisposing = true;

            Dispose(_isCurrentlyDisposing);
        }

        #endregion

        #endregion

        #region Private or Protected

        protected EntityServiceBase()
        {
            _isDisposed = false;
            _entityServices = new List<IEntityService>();
        }

        protected TResult ExecuteInTransaction<TResult>(Func<TResult> method)
        {
            using (var transaction = MainContext.Database.BeginTransaction())
            {
                try
                {
                    var result = method();
                    MainContext.SaveChanges();
                    transaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    throw HandleExecutionException(ex, transaction);
                }
            }
        }

        protected void ExecuteInTransaction(Action method)
        {
            using (var transaction = MainContext.Database.BeginTransaction())
            {
                try
                {
                    method();
                    transaction.Commit();
                    MainContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw HandleExecutionException(ex, transaction);
                }
            }
        }

        protected TResult ExecuteWithoutTransaction<TResult>(Func<TResult> method)
        {
            try
            {
                var result = method();

                MainContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw HandleExecutionException(ex);
            }
        }

        protected void ExecuteWithoutTransaction(Action method)
        {
            try
            {
                method();

                MainContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw HandleExecutionException(ex);
            }
        }

        #endregion
    }
}
