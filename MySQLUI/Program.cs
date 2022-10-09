using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

var config = Configure();

ISQLCrud dataAccess = new MySQLCrud(GetConnectionString(config));

ReadAllPeople(dataAccess);

Console.WriteLine();
ReadPersonById(dataAccess, 1);
Console.WriteLine();

PersonModel christian = new()
{
    FirstName = "Christian",
    LastName = "Avery",
    Employer = new EmployerModel()
    {
        EmployerName = "Walmart"
    },
    Address = new AddressModel()
    {
        City = "Fort Wayne",
        State = "IN",
        ZipCode = "46773",
    }
};


AddPersonToDb(dataAccess, christian);

Console.ReadLine();

static void AddPersonToDb(ISQLCrud dataAccess, PersonModel personToAdd)
{
    dataAccess.CreatePerson(personToAdd);
    Console.WriteLine($"{personToAdd.FirstName} {personToAdd.LastName} added to database");
}

static void ReadPersonById(ISQLCrud dataAccess, int id)
{
    var person = dataAccess.GetPersonById(id);

    if (person is null)
    {
        Console.WriteLine("No Person");
    }
    else
    {
        Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");
        Console.WriteLine($"Employer {person.Employer?.Id}: {person.Employer?.EmployerName}");
        Console.WriteLine(
            $"Address {person.Address?.Id}: " +
            $"{person.Address?.City} " +
            $"{person.Address?.State} " +
            $"{person.Address?.ZipCode}");
    }
}

static void ReadAllPeople(ISQLCrud dataAccess)
{
    var listOfPeople = dataAccess.GetAllPeople();

    listOfPeople.ForEach(person =>
    {
        Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");
        Console.WriteLine($"Employer: {person.Employer?.EmployerName ?? "No Employer"}");
        Console.WriteLine($"City: {person.Address?.City ?? "No City"}");
        Console.WriteLine();
    });
}

static string GetConnectionString(IConfiguration config, string connectionStringName = "Default")
{
    return config.GetConnectionString(connectionStringName);
}

static IConfiguration Configure()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    return builder.Build();
}