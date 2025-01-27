using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Product
{
    public interface IProductWriteRepository : IBaseWriteRepository<Common.Entities.Product>
    {
    }
}
