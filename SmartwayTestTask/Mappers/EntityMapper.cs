using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
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

			// Employee Patch
			CreateMap<JsonPatchDocument<Employee>, JsonPatchDocument<EmployeeDto>>().ReverseMap();
			CreateMap<Operation<Employee>, Operation<EmployeeDto>>().ReverseMap();

			// Department
			CreateMap<Department, DepartmentDto>().ReverseMap();
		}
	}
}
