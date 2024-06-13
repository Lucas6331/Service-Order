using AutoMapper;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;
using OsDsII.api.Repository.ServiceOrders;

namespace OsDsII.api.Services.ServiceOrders
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IMapper _mapper;

        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository, ICustomersRepository customersRepository, IMapper mapper)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _customersRepository = customersRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceOrderDto>> GetAllAsync()
        {
            List<ServiceOrder> serviceOrders = await _serviceOrderRepository.GetAllAsync();
            var serviceOrdersDto = _mapper.Map<List<ServiceOrderDto>>(serviceOrders);

            return serviceOrdersDto;
        }

        public async Task<ServiceOrderDto> GetByIdAsync(int id)
        {
            ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder is null)
            {
                throw new NotFoundException("Service order not found");
            }

            var serviceOrderDto = _mapper.Map<ServiceOrderDto>(serviceOrder);
            return serviceOrderDto;
        }

        public async Task<NewServiceOrderDto> CreateAsync(CreateServiceOrderDto serviceOrder)
        {
            if (serviceOrder is null)
            {
                throw new BadRequestException("Service order cannot be null");
            }

            Customer customer = await _customersRepository.GetByIdAsync(serviceOrder.CustomerId);

            if (customer is null)
            {
                throw new NotFoundException("Customer not found");
            }

            var mappedServiceOrder = _mapper.Map<ServiceOrder>(serviceOrder);
            await _serviceOrderRepository.AddAsync(mappedServiceOrder);

            return _mapper.Map<NewServiceOrderDto>(mappedServiceOrder);
        }

        public async Task FinishAsync(int id)
        {
            ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder is null)
            {
                throw new BadRequestException("Service order cannot be null");
            }

            serviceOrder.FinishOS();
            await _serviceOrderRepository.FinishAsync(serviceOrder);
        }

        public async Task CancelAsync(int id)
        {
            ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder is null)
            {
                throw new BadRequestException("Service order cannot be null");
            }

            serviceOrder.Cancel();
            await _serviceOrderRepository.CancelAsync(serviceOrder);
        }
    }
}
