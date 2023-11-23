using Microsoft.AspNetCore.JsonPatch;
using SmartwayTestTask.Models;

namespace SmartwayTestTask.Repositories.Interfaces
{
	public interface IEmployeeRepository
	{
		Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId);

		Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string name, string phone);

		Task<int> CreateEmployeeAsync(Employee employee);

		Task<int> UpdateEmployeeAsync(int employeeId, JsonPatchDocument<Employee> employeeDto);

		Task<int> DeleteEmployeeAsync(int employeeId);
	}
}
