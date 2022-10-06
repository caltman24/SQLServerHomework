using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ISQLCrud
    {
        void CreatePerson(PersonModel person);
        List<PersonModel> GetAllPeople();
        PersonModel? GetPersonById(int id);
    }
}