using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    // TODO: Fnish class
    public class MySQLCrud : ISQLCrud
    {
        private readonly string _connectionString;
        private readonly ISQLDataAccess _db = new MySQLDataAccess();

        public MySQLCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreatePerson(PersonModel person)
        {
            throw new NotImplementedException();
        }

        public List<PersonModel> GetAllPeople()
        {
            throw new NotImplementedException();
        }

        public PersonModel? GetPersonById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
