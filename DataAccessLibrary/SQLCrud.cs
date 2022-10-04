using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary;

public class SQLCrud
{
	private readonly string _connectionString;
	private readonly SQLServerDataAccess _db = new();
	public SQLCrud(string connectionString)
	{
		_connectionString = connectionString;
	}

	public List<PersonModel> GetAllPeople()
	{
		string sql = @"select p.Id, p.FirstName, p.LastName from dbo.People p";
		var people =  _db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString);

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
        var person =  _db.LoadData<PersonModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

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
        // TODO: Implement CreatePerson crud method
        throw new NotImplementedException();
    }
}

