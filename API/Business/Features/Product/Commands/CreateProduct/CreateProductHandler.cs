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

namespace Business.Features.Product.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;
        public CreateProductHandler(IUnitOfWork unitOfWork,
            IProductWriteRepository productRepository,
            IProductReadRepository productReadRepository,
            IMapper mapper,
            IProducerService producerService)
        {
            _unitOfWork = unitOfWork;
            _productWriteRepository = productRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _producerService = producerService;
        }
        public async Task<Response> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = await new CreateProductCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var product = await _productReadRepository.GetByNameAsync(request.Name);
            if (product is not null)
                throw new ValidationException("Bu adda mehsul movcuddur");

            product = _mapper.Map<Common.Entities.Product>(request);

            await _productWriteRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            await _producerService.ProduceAsync("create", product);

            return new Response
            {
                Message = "Mehsul ugurla elave olundu"
            };
        }
    }
}
