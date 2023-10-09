using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManager.Models;

namespace EmployeeManager.Services
{
    public interface IApiService
    {
        Task<Result<List<Employee>>> GetEmployees();
        Task<Result<List<Employee>>> SearchUsersByName(string name);
        Task<Result<Employee>> CreateUser(Employee newUser);
        Task<Result<bool>> DeleteUser(int userId);
    }
}
