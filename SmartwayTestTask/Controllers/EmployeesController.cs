using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SmartwayTestTask.Dtos;
using SmartwayTestTask.Models;
using SmartwayTestTask.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SmartwayTestTask.Controllers
{
	[Route("v1")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;

		public EmployeesController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		[HttpPost("employees")]
		public async Task<ActionResult<dynamic>> CreateEmployee(EmployeeDto employeeDto)
		{
			var employee = await _employeeService.CreateEmployeeAsync(employeeDto);

			if (employee == null)
			{
				return BadRequest();
			}

			return StatusCode(StatusCodes.Status201Created, employee);
		}

		[HttpPatch("employees/{employeeId}")]
		public async Task<ActionResult> UpdateEmployee(int employeeId, [FromBody] JsonPatchDocument<Employee> employeeDto)
		{
			var result = await _employeeService.UpdateEmployeeAsync(employeeId, employeeDto);

			if (result == 0)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpGet("employees-per-company")]
		public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByCompanyId([Required] int companyId)
		{
			var employees = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

			if (employees == null || !employees.Any())
			{
				return NotFound();
			}

			return Ok(employees);
		}

		[HttpGet("employees-per-department")]
		public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment([Required] string name, [Required] string phone)
		{
			var employees = await _employeeService.GetEmployeesByDepartmentAsync(name, phone);

			if (employees == null || !employees.Any())
			{
				return NotFound();
			}

			return Ok(employees);
		}

		[HttpDelete("employees/{employeeId}")]
		public async Task<ActionResult> DeleteEmployee(int employeeId)
		{
			var result = await _employeeService.DeleteEmployeeAsync(employeeId);

			if (result == 0)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
