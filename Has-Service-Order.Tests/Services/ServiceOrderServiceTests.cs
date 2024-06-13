using AutoMapper;
using Moq;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;
using OsDsII.api.Repository.ServiceOrders;
using OsDsII.api.Services.ServiceOrders;

namespace Has_Service_Order.Tests.Services
{
    public class ServiceOrderServiceTests
    {
        private readonly Mock<IServiceOrderRepository> _mockSORepository;
        private readonly Mock<ICustomersRepository> _mockCustomersRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ServiceOrderService _service;

        public ServiceOrderServiceTests()
        {
            _mockSORepository = new Mock<IServiceOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCustomersRepository = new Mock<ICustomersRepository>();
            _service = new ServiceOrderService(_mockSORepository.Object, _mockCustomersRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Return_A_List_Of_ServiceOrders()
        {
            List<ServiceOrder> serviceOrders = new List<ServiceOrder>()
            {
                new ServiceOrder { Description = "Test 1", Price = 100.0, Status = StatusServiceOrder.OPEN, OpeningDate = DateTimeOffset.Now, FinishDate = DateTimeOffset.Now.AddDays(30), Comments = null },
                new ServiceOrder { Description = "Test 2", Price = 200.0, Status = StatusServiceOrder.FINISHED, OpeningDate = DateTimeOffset.Now, FinishDate = DateTimeOffset.Now, Comments = null },
                new ServiceOrder { Description = "Test 3", Price = 250.0, Status = StatusServiceOrder.CANCELED, OpeningDate = DateTimeOffset.Now, FinishDate = DateTimeOffset.Now, Comments = null }
            };

            _mockSORepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(serviceOrders);

            List<ServiceOrderDto> serviceOrdersDto = new List<ServiceOrderDto>()
            {
                new ServiceOrderDto(serviceOrders[0].Description, serviceOrders[0].Price, serviceOrders[0].Status, serviceOrders[0].OpeningDate, serviceOrders[0].FinishDate, null),
                new ServiceOrderDto(serviceOrders[1].Description, serviceOrders[1].Price, serviceOrders[1].Status, serviceOrders[1].OpeningDate, serviceOrders[1].FinishDate, null),
                new ServiceOrderDto(serviceOrders[2].Description, serviceOrders[2].Price, serviceOrders[2].Status, serviceOrders[2].OpeningDate, serviceOrders[2].FinishDate, null)
            };

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ServiceOrderDto>>(serviceOrders)).Returns(serviceOrdersDto);

            var result = await _service.GetAllAsync();
            Assert.Equal(serviceOrders.Count(), result.Count());

            for (int i = 0; i < serviceOrdersDto.Count; i++)
            {
                Assert.Equal(serviceOrdersDto[i].Description, result.ElementAt(i).Description);
                Assert.Equal(serviceOrdersDto[i].Price, result.ElementAt(i).Price);
                Assert.Equal(serviceOrdersDto[i].Status, result.ElementAt(i).Status);
                Assert.Equal(serviceOrdersDto[i].OpeningDate, result.ElementAt(i).OpeningDate);
                Assert.Equal(serviceOrdersDto[i].FinishDate, result.ElementAt(i).FinishDate);
            }
        }

        #region GetById
        [Fact]
        public async Task Should_Return_A_ServiceOrder_When_Exists()
        {
            int serviceOrderId = 1;
            ServiceOrder serviceOrder = new ServiceOrder { Id = serviceOrderId, Description = "Test 1", Price = 100.0, Status = StatusServiceOrder.OPEN, OpeningDate = DateTimeOffset.Now, FinishDate = DateTimeOffset.Now.AddDays(30), Comments = null };
            ServiceOrderDto serviceOrderDto = new ServiceOrderDto(serviceOrder.Description, serviceOrder.Price, serviceOrder.Status, serviceOrder.OpeningDate, serviceOrder.FinishDate, null);

            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync(serviceOrder);
            _mockMapper.Setup(mapper => mapper.Map<ServiceOrderDto>(serviceOrder)).Returns(serviceOrderDto);

            var result = await _service.GetByIdAsync(serviceOrderId);

            Assert.Equal(serviceOrderDto, result);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Getting_A_Null_ServiceOrder()
        {
            int serviceOrderId = 1;
            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync((ServiceOrder)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(serviceOrderId));
        }
        #endregion

        #region CreateAsync
        [Fact]
        public async Task Should_Create_A_New_Service_Order_When_Customer_Exists()
        {
            CreateServiceOrderDto createServiceOrderDto = new CreateServiceOrderDto("AAA", 100.00, 1);
            Customer customer = new Customer { Id = 1, Name = "Lucas" };
            ServiceOrder serviceOrder = new ServiceOrder { Description = "Test Order", Price = 100.0, Status = StatusServiceOrder.OPEN, OpeningDate = DateTimeOffset.Now, FinishDate = DateTimeOffset.Now.AddDays(30), CustomerId = 1 };
            NewServiceOrderDto newServiceOrderDto = new NewServiceOrderDto("Test Order", 100.0, StatusServiceOrder.OPEN, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(30), 1);

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(createServiceOrderDto.CustomerId)).ReturnsAsync(customer);
            _mockMapper.Setup(mapper => mapper.Map<ServiceOrder>(createServiceOrderDto)).Returns(serviceOrder);
            _mockSORepository.Setup(r => r.AddAsync(serviceOrder));
            _mockMapper.Setup(mapper => mapper.Map<NewServiceOrderDto>(serviceOrder)).Returns(newServiceOrderDto);

            var result = await _service.CreateAsync(createServiceOrderDto);

            Assert.Equal(newServiceOrderDto.Description, result.Description);
            Assert.Equal(newServiceOrderDto.Price, result.Price);
            Assert.Equal(newServiceOrderDto.Status, result.Status);
            Assert.Equal(newServiceOrderDto.OpeningDate, result.OpeningDate);
            Assert.Equal(newServiceOrderDto.FinishDate, result.FinishDate);
            Assert.Equal(newServiceOrderDto.CustomerId, result.CustomerId);

            _mockSORepository.Verify(r => r.AddAsync(serviceOrder), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_BadRequestException_When_Creating_A_Null_ServiceOrder()
        {
            await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(null));
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Creating_A_ServiceOrder_For_A_NonExistent_Customer()
        {
            CreateServiceOrderDto createServiceOrderDto = new CreateServiceOrderDto("AAA", 100.00, 1);

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(createServiceOrderDto.CustomerId)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(createServiceOrderDto));
        }
        #endregion

        #region FinishAsync
        [Fact]
        public async Task Should_Finish_ServiceOrder_When_ServiceOrder_Exists()
        {
            int serviceOrderId = 1;
            ServiceOrder serviceOrder = new ServiceOrder { Id = serviceOrderId, Description = "AAA", Status = StatusServiceOrder.OPEN };

            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync(serviceOrder);
            _mockSORepository.Setup(r => r.FinishAsync(serviceOrder));

            await _service.FinishAsync(serviceOrderId);

            Assert.Equal(StatusServiceOrder.FINISHED, serviceOrder.Status);
        }

        [Fact]
        public async Task Should_Throw_BadRequestException_When_Finishing_A_Null_ServiceOrder()
        {
            int serviceOrderId = 1;
            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync((ServiceOrder)null);

            await Assert.ThrowsAsync<BadRequestException>(() => _service.FinishAsync(serviceOrderId));
        }
        #endregion

        #region CancelAsync
        [Fact]
        public async Task Should_Cancel_ServiceOrder_When_ServiceOrder_Exists()
        {
            int serviceOrderId = 1;
            ServiceOrder serviceOrder = new ServiceOrder { Id = serviceOrderId, Description = "AAA", Status = StatusServiceOrder.OPEN };

            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync(serviceOrder);
            _mockSORepository.Setup(r => r.CancelAsync(serviceOrder));

            await _service.CancelAsync(serviceOrderId);

            Assert.Equal(StatusServiceOrder.CANCELED, serviceOrder.Status);
        }

        [Fact]
        public async Task Should_Throw_BadRequestException_When_Cancelling_A_Null_ServiceOrder()
        {
            int serviceOrderId = 1;
            _mockSORepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync((ServiceOrder)null);

            await Assert.ThrowsAsync<BadRequestException>(() => _service.CancelAsync(serviceOrderId));
        }
        #endregion
    }
}
