using Microsoft.AspNetCore.JsonPatch;
using SmartwayTestTask.Dtos;
using SmartwayTestTask.Models;

namespace SmartwayTestTask.Services.Interfaces
{
	public interface IEmployeeService
	{
		Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyIdAsync(int companyId);

		Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(string name, string phone);

		Task<dynamic> CreateEmployeeAsync(EmployeeDto employeeDto);

		Task<int> UpdateEmployeeAsync(int employeeId, JsonPatchDocument<Employee> employeeDto);

		Task<int> DeleteEmployeeAsync(int employeeId);
	}
}
