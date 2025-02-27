using GenericRepository;
using OzdamarDepo.Domain.Employees;

namespace OzdamarDepo.Domain.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Özel sorgular buraya eklenebilir
        Task<Employee?> GetByTCNoAsync(string tcNo);
    }
} 