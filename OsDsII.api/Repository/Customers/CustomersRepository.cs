using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

namespace OsDsII.api.Repository.Customers
{
    public sealed class CustomersRepository : ICustomersRepository
    {
        private readonly DataContext _dataContext;

        public CustomersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dataContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _dataContext.Customers.AddAsync(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _dataContext.Customers.Remove(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _dataContext.Customers.Update(customer);
            await _dataContext.SaveChangesAsync();
        }

    }
}
