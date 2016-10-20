using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TestingApp.BL;
using TestingApp.Common;

namespace TestingApp.UI
{
    public abstract class EntityController<T> : BaseController
        where T: IEntityService, new()
    {
        private T _mainService;

        protected T MainService
        {
            get
            {
                if (_mainService == null)
                    _mainService = new T();
                return _mainService;
            }
        }
    }
}