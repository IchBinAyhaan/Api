using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Product;
using Data.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Response>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductHandler(IProductReadRepository productReadRepository,
                                    IProductWriteRepository productWriteRepository,
                                    IUnitOfWork unitOfWork)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetAsync(request.Id);
            if (product is null)
                throw new NotFoundException("Mehsul tapilmadi");

            _productWriteRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return new Response()
            {
                Message = "Mehsul ugurla silindi"
            };
        }
    }
}
