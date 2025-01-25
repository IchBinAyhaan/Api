using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Product
{
    public class ProductReadRepository : BaseReadRepository<Common.Entities.Product>, IProductReadRepository
    {
        private readonly AppDbContext _context;

        public ProductReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Common.Entities.Product> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }
    }
}
