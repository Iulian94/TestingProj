using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestingApp.BE;

namespace TestingApp.BL
{
    public class CategoryService : EntityServiceBase
    {
        public void SaveCategory(Product model)
        {
            ExecuteInTransaction(() => _SaveCategory(model));
        }

        public void _SaveCategory(Product model)
        {
            var service = GetService<ProductService>();
        }
    }
}
