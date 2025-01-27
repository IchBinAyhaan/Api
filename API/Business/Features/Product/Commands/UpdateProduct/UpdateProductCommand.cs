using Business.Wrappers;
using Common.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Photo { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductType Type { get; set; }
    }
}
