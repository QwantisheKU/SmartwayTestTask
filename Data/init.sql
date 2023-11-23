CREATE TABLE IF NOT EXISTS Department
(
	Id SERIAL PRIMARY KEY,
	Name Varchar(255),
	Phone Varchar(255)
);

CREATE TABLE IF NOT EXISTS Employee
(
    Id SERIAL PRIMARY KEY,
    Name Varchar(255),
    Surname Varchar(255),
	Phone Varchar(255),
	PassportType Varchar(255),
	PassportNumber Varchar(255),
	CompanyId INTEGER,
	DepartmentId INTEGER,
    FOREIGN KEY (DepartmentId) REFERENCES Department (Id) ON DELETE SET NULL
);