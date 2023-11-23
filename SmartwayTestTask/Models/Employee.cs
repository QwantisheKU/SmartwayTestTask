namespace SmartwayTestTask.Models
{
	public class Employee
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public string Phone { get; set; }

		public int CompanyId { get; set; }

		// Данные паспорта хранятся в Employee, но можно выделить в отдельную таблицу с документами
		public Passport? Passport { get; set; }

		public int? DepartmentId { get; set; }

		public Department? Department { get; set; }
	}
}
