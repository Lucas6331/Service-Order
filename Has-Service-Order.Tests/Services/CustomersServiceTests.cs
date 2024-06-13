using AutoMapper;
using Moq;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;
using OsDsII.api.Services.Customers;

namespace Has_Service_Order.Tests.Services
{
    public class CustomersServiceTests
    {
        private readonly Mock<ICustomersRepository> _mockCustomersRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomersService _service;

        public CustomersServiceTests()
        {
            _mockCustomersRepository = new Mock<ICustomersRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CustomersService(_mockCustomersRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Return_A_List_Of_Customers()
        {
            // gera uma lista estática de customersDto
            List<Customer> customers = new List<Customer>()
            {
                new Customer { Name = "Lucas", Email = "gmail@gmail.com" , Phone = "11999999999", ServiceOrders = null },
                new Customer { Name = "Silva", Email = "gmail2@gmail.com", Phone = "11900000000", ServiceOrders = null }

            };

            List<CustomerDto> customersDto = new List<CustomerDto>()
            {
                new CustomerDto ( customers[0].Name, customers[0].Email, customers[0].Phone, null),
                new CustomerDto ( customers[1].Name, customers[1].Email, customers[1].Phone, null),
            };

            _mockCustomersRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(customers);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerDto>>(customers)).Returns(customersDto);

            var result = await _service.GetAllAsync();

            Assert.Equal(customers.Count(), result.Count());

            // Verificação de igualdade entre Customer e CustomerDto
            for (int i = 0; i < customersDto.Count; i++)
            {
                Assert.Equal(customersDto[i].Name, result.ElementAt(i).Name);
                Assert.Equal(customersDto[i].Email, result.ElementAt(i).Email);
                Assert.Equal(customersDto[i].Phone, result.ElementAt(i).Phone);
            }

        }

        #region GetById
        [Fact]
        public async Task Should_Return_CustomerById_When_Customer_Exists()
        {
            int customerId = 1;
            Customer customer = new Customer { Id = customerId, Name = "Lucas", Email = "gmail@gmail.com", Phone = "11999999999", ServiceOrders = null };
            CustomerDto customerDto = new CustomerDto(customer.Name, customer.Email, customer.Phone, null);

            _mockCustomersRepository.Setup(repository => repository.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerDto>(customer)).Returns(customerDto);

            var result = await _service.GetByIdAsync(customerId);

            Assert.Equal(customerDto, result);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Getting_NonExistent_Customer_ById()
        {
            int customerId = 1;

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(customerId));

        }
        #endregion

        #region CreateAsync
        [Fact]
        public async Task Should_Create_New_Customer()
        {
            CreateCustomerDto newCustomer = new CreateCustomerDto("Lucas", "gmail@gmail.com", "11999999999");
            Customer customer = new Customer { Name = newCustomer.Name, Email = newCustomer.Email, Phone = newCustomer.Phone };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(newCustomer)).Returns(customer);
            _mockCustomersRepository.Setup(r => r.GetCustomerByEmailAsync(customer.Email)).ReturnsAsync((Customer)null);
            _mockCustomersRepository.Setup(r => r.AddCustomerAsync(customer));

            await _service.CreateAsync(newCustomer);

            Assert.Equal(newCustomer.Name, customer.Name);
            Assert.Equal(newCustomer.Phone, customer.Phone);
            Assert.Equal(newCustomer.Email, customer.Email);

            _mockCustomersRepository.Verify(r => r.AddCustomerAsync(It.Is<Customer>(c =>
                c.Name == newCustomer.Name &&
                c.Email == newCustomer.Email &&
                c.Phone == newCustomer.Phone)), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_ConflictException_When_Creating_A_Existing_Customer()
        {
            Customer existingCustomer = new Customer { Name = "Lucas", Email = "gmail@gmail.com", Phone = "11999999999" };
            CreateCustomerDto customerDto = new CreateCustomerDto("AAA", "gmail@gmail.com", "11987654321");

            //customerExists is not Equal as new Customer 
            Customer newCustomer = new Customer { Name = customerDto.Name, Email = customerDto.Email, Phone = customerDto.Phone };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(customerDto)).Returns(newCustomer);
            _mockCustomersRepository.Setup(r => r.GetCustomerByEmailAsync(existingCustomer.Email)).ReturnsAsync(existingCustomer);

            await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(customerDto));
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task Should_Delete_A_Existent_Customer()
        {
            int customerId = 1;
            Customer customer = new Customer { Id = customerId, Name = "Lucas", Email = "gmail@gmail.com", Phone = "11999999999", ServiceOrders = null };

            _mockCustomersRepository.Setup(repository => repository.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockCustomersRepository.Setup(r => r.DeleteCustomerAsync(customer));

            await _service.DeleteAsync(customerId);

            _mockCustomersRepository.Verify(r => r.DeleteCustomerAsync(customer), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Excluding_A_NonExistent_Customer()
        {
            int customerId = 1;

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(customerId));
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task Should_Update_A_Existent_Customer()
        {
            int customerId = 1;
            Customer existingCustomer = new Customer { Id = customerId, Name = "Name", Email = "wrongemail@email.com", Phone = "11911111111", ServiceOrders = null };
            CreateCustomerDto updatedCustomer = new CreateCustomerDto("Lucas", "updatedemail@email.com", "11999999999");

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);

            await _service.UpdateAsync(customerId, updatedCustomer);

            Assert.Equal(updatedCustomer.Name, existingCustomer.Name);
            Assert.Equal(updatedCustomer.Email, existingCustomer.Email);
            Assert.Equal(updatedCustomer.Phone, existingCustomer.Phone);
            _mockCustomersRepository.Verify(r => r.UpdateCustomerAsync(existingCustomer), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Updating_A_NonExistent_Customer()
        {
            int customerId = 1;

            _mockCustomersRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(customerId, null));
        }
        #endregion
    }
}
