using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Product
{
    public interface IProductReadRepository : IBaseReadRepository<Common.Entities.Product>
    {
        Task<Common.Entities.Product> GetByNameAsync(string name);
    }
}
