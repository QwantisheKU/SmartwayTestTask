using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SmartwayTestTask.Dtos;
using SmartwayTestTask.Models;
using SmartwayTestTask.Repositories.Interfaces;
using SmartwayTestTask.Services.Interfaces;

namespace SmartwayTestTask.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;

		public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
		{
			_employeeRepository = employeeRepository;
			_mapper = mapper;
		}

		public async Task<dynamic> CreateEmployeeAsync(EmployeeDto employeeDto)
		{
			var employee = _mapper.Map<Employee>(employeeDto);

			var employeeId = await _employeeRepository.CreateEmployeeAsync(employee);

			// Или вернуть результат через DTO с employeeId внутри
			return new
			{
				employeeId
			};
		}

		public async Task<int> DeleteEmployeeAsync(int employeeId)
		{
			var result = await _employeeRepository.DeleteEmployeeAsync(employeeId);

			return result;
		}

		public async Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyIdAsync(int companyId)
		{
			var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);

			var mappedEmployees = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

			return mappedEmployees;
		}

		public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(string name, string phone)
		{
			var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(name, phone);

			var mappedEmployees = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

			return mappedEmployees;
		}

		public async Task<int> UpdateEmployeeAsync(int employeeId, JsonPatchDocument<Employee> employeeDto)
		{
			var result = await _employeeRepository.UpdateEmployeeAsync(employeeId, employeeDto);

			return result;
		}
	}
}
