using Dapper;
using Data;
using Microsoft.AspNetCore.JsonPatch;
using SmartwayTestTask.Models;
using SmartwayTestTask.Repositories.Interfaces;

namespace SmartwayTestTask.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly DbContext _context;

		public EmployeeRepository(DbContext context)
		{
			_context = context;
		}

		public async Task<int> CreateEmployeeAsync(Employee employee)
		{
			// Лучше искать по departmentId, но изначально его нет, поэтому ищем существующий департамент по name и phone
			var queryDepartment = "SELECT * FROM department WHERE Name=@Name and Phone=@Phone";
			
			/*
				Проверяем, существует ли переданный департамент, 
				если существует, то привязываем его к создаваемому работнику (предварительно создать департамент)
			*/
			Department? department;
			using (var connection = _context.CreateConnection())
			{
				department = await connection.QueryFirstOrDefaultAsync<Department>(queryDepartment, 
					new { 
						employee.Department?.Name, 
						employee.Department?.Phone 
					});
			}
			employee.DepartmentId = department?.Id != 0 ? department?.Id : null;

			var queryEmployee = """
				INSERT INTO employee (Name, Surname, Phone, PassportType, PassportNumber, CompanyId, DepartmentId)
				VALUES (@Name, @Surname, @Phone, @PassportType, @PassportNumber, @CompanyId, @DepartmentId)
				RETURNING Id
			""";

			var parameters = new
			{
				employee.Name,
				employee.Surname,
				employee.Phone,
				PassportType = employee.Passport?.Type,
				PassportNumber = employee.Passport?.Number,
				employee.CompanyId,
				employee.DepartmentId
			};
			Employee? dbEmployee;
			using (var connection = _context.CreateConnection())
			{
				dbEmployee = await connection.QueryFirstOrDefaultAsync<Employee>(queryEmployee, parameters);
			}

			if (dbEmployee == null)
			{
				return 0;
			}

			return dbEmployee.Id;
		}

		public async Task<int> DeleteEmployeeAsync(int employeeId)
		{
			var query = "DELETE FROM employee WHERE Id = @Id";

			var rowsAffected = 0;
			using (var connection = _context.CreateConnection())
			{
				rowsAffected = await connection.ExecuteAsync(query, new { Id = employeeId });
			}

			return rowsAffected;
		}

		public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
		{
			var query = "SELECT * FROM employee e LEFT JOIN department d ON e.DepartmentId = d.Id WHERE e.CompanyId = @CompanyId";
			var passportQuery = "SELECT PassportType AS Type, PassportNumber AS Number FROM employee e WHERE e.CompanyId = @CompanyId";

			using (var connection = _context.CreateConnection())
			{
				var employees = await connection.QueryAsync<Employee, Department, Employee>(query, 
					map: (e, d) => { 
						e.Department = d;
						e.DepartmentId = d.Id; 
						return e; 
					}, 
					splitOn: "DepartmentId", 
					param: new { companyId }
				);
				var passports = await connection.QueryAsync<Passport>(passportQuery, new { companyId });

				var employeesList = employees.ToList();
				var passportsList = passports.ToList();
				employeesList = PopulateEmployeesWithPassports(employeesList, passportsList);

				return employeesList;
			}
		}

		public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string name, string phone)
		{
			var query = "SELECT * FROM employee e LEFT JOIN department d ON e.DepartmentId = d.Id WHERE d.Name = @Name and d.Phone = @Phone";
			var passportQuery = """
			SELECT PassportType AS Type, PassportNumber AS Number FROM employee e 
			LEFT JOIN department d ON e.DepartmentId = d.Id WHERE d.Name = @Name and d.Phone = @Phone
			""";

			using (var connection = _context.CreateConnection())
			{
				var employees = await connection.QueryAsync<Employee, Department, Employee>(query, 
					map: (e, d) => 
					{ 
						e.Department = d; 
						e.DepartmentId = d.Id; 
						return e; 
					}, 
					splitOn: "DepartmentId", 
					param: new { Name = name, Phone = phone });
				var passports = await connection.QueryAsync<Passport>(passportQuery, new { Name = name, Phone = phone });

				var employeesList = employees.ToList();
				var passportsList = passports.ToList();
				employeesList = PopulateEmployeesWithPassports(employeesList, passportsList);

				return employeesList;
			}
		}

		public async Task<int> UpdateEmployeeAsync(int employeeId, JsonPatchDocument<Employee> employeeDto)
		{
			var querySelect = "SELECT * FROM employee WHERE Id = @Id";

			Employee? employee;
			using (var connection = _context.CreateConnection())
			{
				employee = await connection.QueryFirstOrDefaultAsync<Employee>(querySelect, new { Id = employeeId });
			}

			if (employee == null)
			{
				return 0;
			}

			employeeDto.ApplyTo(employee);

			var queryUpdate = """
				UPDATE employee SET Name=@Name, Surname=@Surname, Phone=@Phone, CompanyId=@CompanyId, 
				PassportType=@PassportType, PassportNumber=@PassportNumber WHERE Id = @Id
			""";

			var parameters = new
			{
				employee.Name,
				employee.Surname,
				employee.Phone,
				employee.CompanyId,
				PassportType = employee.Passport?.Type,
				PassportNumber = employee.Passport?.Number,
				employee.Id
			};
			var rowsAffected = 0;
			using (var connection = _context.CreateConnection())
			{
				rowsAffected = await connection.ExecuteAsync(queryUpdate, parameters);
			}

			return rowsAffected;
		}

		// Привязываем паспорта к работникам, в случае расширения можно сделать generic
		private List<Employee> PopulateEmployeesWithPassports(List<Employee> employees, List<Passport> passports)
		{
			if (employees.Count == passports.Count)
			{
				for (int i = 0; i < employees.Count; i++)
				{
					employees[i].Passport = passports[i];
				}
			}

			return employees;
		}
	}
}
