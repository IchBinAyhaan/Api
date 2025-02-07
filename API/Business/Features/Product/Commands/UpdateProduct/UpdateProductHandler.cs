using AutoMapper;
using Business.Servicer.Producer;
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

namespace Business.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Response>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;

        public UpdateProductHandler(IProductReadRepository productReadRepository,
                                    IProductWriteRepository productWriteRepository,
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper,
                                    IProducerService producerService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producerService = producerService;
        }
        public async Task<Response> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetAsync(request.Id);
            if (product is null)
                throw new NotFoundException("Mehsul tapilmadi");

            var result = await new UpdateProductCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _mapper.Map(request, product);

            _productWriteRepository.Update(product);
            await _unitOfWork.CommitAsync();

            await _producerService.ProduceAsync("Update", product);
            return new Response
            {
                Message = "Mehsul ugurla yenilendi"
            };
        }
    }
}
