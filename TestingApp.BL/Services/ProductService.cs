using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestingApp.BE;

namespace TestingApp.BL
{
    public class ProductService : EntityServiceBase
    {
        public void SaveProduct(Product model)
        {
            ExecuteInTransaction(() => _SaveProduct(model));
        }

        public void _SaveProduct(Product model)
        {
            var service = GetService<CategoryService>();
        }
    }
}
