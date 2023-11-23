using AutoMapper;
using SmartwayTestTask.Dtos;
using SmartwayTestTask.Models;

namespace SmartwayTestTask.Mapper
{
	public class EntityMapper : Profile
	{
		public EntityMapper()
		{
			// Employee
			CreateMap<Employee, EmployeeDto>().ReverseMap();

			// Department
			CreateMap<Department, DepartmentDto>().ReverseMap();
		}
	}
}
