using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;
using OsDsII.api.Repository.ServiceOrders;
using OsDsII.api.Services.ServiceOrders;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IServiceOrderRepository _serviceOrderRepository; //IOC (Inversion of Control)
        private readonly IServiceOrderService _serviceOrderService;
        private readonly ICustomersRepository _customerRepository;

        public ServiceOrdersController(IServiceOrderRepository serviceOrderRepository, ICustomersRepository customerRepository, IServiceOrderService serviceOrderService)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _customerRepository = customerRepository;
            _serviceOrderService = serviceOrderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllServiceOrderAsync()
        {
            try
            {
                List<ServiceOrderDto> serviceOrders = await _serviceOrderService.GetAllAsync();
                return Ok(serviceOrders);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServiceOrderById(int id)
        {
            try
            {
                ServiceOrderDto serviceOrder = await _serviceOrderService.GetByIdAsync(id);
                return Ok(serviceOrder);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateServiceOrderAsync(CreateServiceOrderDto serviceOrder)
        {
            try
            {
                await _serviceOrderService.CreateAsync(serviceOrder);
                return Created(nameof(ServiceOrdersController), serviceOrder);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }

        }

        [HttpPut("{id}/status/finish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinishServiceOrderAsync(int id)
        {
            try
            {
                await _serviceOrderService.FinishAsync(id);
                return NoContent();

            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }

        [HttpPut("{id}/status/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelServiceOrder(int id)
        {
            try
            {
                await _serviceOrderService.CancelAsync(id);
                return NoContent();
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }
    }
}