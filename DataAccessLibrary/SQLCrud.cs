using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary;

public class SQLCrud : ISQLCrud
{
	private readonly string _connectionString;
	private readonly ISQLServerDataAccess _db = new SQLServerDataAccess();
	public SQLCrud(string connectionString)
	{
		_connectionString = connectionString;
	}

	public List<PersonModel> GetAllPeople()
	{
		string sql = @"select p.Id, p.FirstName, p.LastName from dbo.People p";
		var people = _db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString);

		people.ForEach(person =>
		{
			sql = @"select e.Id, e.EmployerName 
				from dbo.Employers e
				inner join dbo.People p
				on p.EmployerId = e.Id
				where p.Id = @Id";
			person.Employer = _db.LoadData<EmployerModel, dynamic>(sql, new { person.Id }, _connectionString).FirstOrDefault();

			sql = @"select a.* 
				from dbo.Addresses a
				inner join dbo.People p 
				on p.AddressId = a.Id
				where p.Id = @Id";

			person.Address = _db.LoadData<AddressModel, dynamic>(sql, new { person.Id }, _connectionString).FirstOrDefault();
		});

		return people;
	}

	public PersonModel? GetPersonById(int id)
	{
		string sql = @"select p.Id, p.FirstName, p.LastName from dbo.People p where p.Id = @Id";
		var person = _db.LoadData<PersonModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

		if (person is null)
		{
			return null;
		}

		sql = @"select e.Id, e.EmployerName 
				from dbo.Employers e
				inner join dbo.People p
				on p.EmployerId = e.Id
				where p.Id = @Id";

		person.Employer = _db.LoadData<EmployerModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

		sql = @"select a.* 
				from dbo.Addresses a
				inner join dbo.People p 
				on p.AddressId = a.Id
				where p.Id = @Id";

		person.Address = _db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

		return person;
	}

	public void CreatePerson(PersonModel person)
	{
		int AddressId = 0;
		int EmployerId = 0;

		// Save Employer to db
		if (person.Employer?.Id <= 0)
		{
			string sql = @"insert into dbo.Employers (EmployerName) values (@EmployerName);";
			_db.SaveData(sql, person.Employer ,_connectionString);
			// Get back id

			sql = @"select e.Id from dbo.Employers e where e.EmployerName = @EmployerName;";
			EmployerId = _db.LoadData<int, dynamic>(sql, new { person.Employer?.EmployerName }, _connectionString).First();
		}

        // Save Address to db
        if (person.Address?.Id <= 0)
        {
            string sql = @"insert into dbo.Addresses
						   (City, State, ZipCode) 
						   values 
						   (@City, @State, @ZipCode);";
			_db.SaveData(sql, new
			{
                person.Address?.City,
                person.Address?.State,
                person.Address?.ZipCode
            }, _connectionString);

            sql = @"select a.Id from dbo.Addresses a where City = @City and State = @State and ZipCode = @ZipCode;";
            AddressId = _db.LoadData<int, dynamic>(sql, new 
			{ 
				person.Address?.City,
                person.Address?.State,
                person.Address?.ZipCode
            }, _connectionString).First();
        }

        if (person.Id <= 0)
		{
			string sql = @"insert into dbo.People 
							(FirstName, LastName, AddressId, EmployerId)
							values
							(@FirstName, @LastName, @AddressId, @EmployerId);";
			_db.SaveData(sql, new 
			{ 
				person.FirstName,
				person.LastName,
				AddressId,
                EmployerId
            }, _connectionString);
		}

    }
}

