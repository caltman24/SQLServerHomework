using Microsoft.Extensions.Configuration;
using DataAccessLibrary;
using DataAccessLibrary.Models;

var config = Configure();
SQLCrud dataAccess = new(GetConnectionString(config));



ReadAllPeople(dataAccess);

Console.WriteLine();

ReadPersonById(dataAccess, 1);

// TODO: Implement CreatePerson crud method
PersonModel personToAdd = new()
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

dataAccess.CreatePerson(personToAdd);



Console.ReadLine();

static void ReadPersonById(SQLCrud dataAccess, int id)
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

static void ReadAllPeople(SQLCrud dataAccess)
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