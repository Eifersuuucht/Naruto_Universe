using NarutoUniverseProject.Data;
using NarutoUniverseProject.Models.AbilityModels;
using NarutoUniverseProject.Models.PersonModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Services
{
    public class PersonService
    {
        private readonly ConnectionString _connString;

        public PersonService(ConnectionString connString)
        {
            _connString = connString;
        }

        public ICollection<PersonSummaryViewModel> GetPersons()
        {
            String sql = "SELECT * FROM persons;";
            ICollection<PersonSummaryViewModel> persons = new List<PersonSummaryViewModel>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        persons.Add(new PersonSummaryViewModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2)
                        });
                    }
                }
            }
            return persons;
        }

        public PersonDetailedViewModel GetPerson(Int32 id)
        {
            String sql = String.Format("SELECT p.id, p.name, p.age, ps.name, c.name FROM persons p INNER JOIN positions ps ON p.position_id = ps.id INNER JOIN countries c on p.country_id = c.id WHERE p.id = {0};", id);
            PersonDetailedViewModel person = new PersonDetailedViewModel();

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        person.Id = reader.GetInt32(0);
                        person.Name = reader.GetString(1);
                        person.Age = reader.GetInt32(2);
                        person.Position = reader.GetString(3);
                        person.Country = reader.GetString(4);
                    }
                }
                else
                {
                    return null;
                }
                reader.Close();
                command.CommandText = String.Format("SELECT a.id, a.name, a.time_to_cast_sec FROM abilities a INNER JOIN person_ability pa ON a.id = pa.ability_id INNER JOIN persons p ON pa.person_id = p.id WHERE p.id = {0};", id);
                reader = command.ExecuteReader();

                person.Abilities = new List<AbilitySummaryViewModel>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        person.Abilities.Add(new AbilitySummaryViewModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TimeToCast = reader.GetInt32(2)
                        });
                    }
                }
            }
            return person;
        }


        public Int32 CreatePerson(PersonCreateBindModel bindModel)
        {
            String sql = String.Format("INSERT INTO persons(name, age, position_id, country_id) VALUES({0}, {1}, {2}, {3});",
                bindModel.Name, bindModel.Age, bindModel.PositionId, bindModel.CountryId);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 added = command.ExecuteNonQuery();

                if (added == 0)
                {
                    return -1;
                }

                command.CommandText = String.Format("SELECT id FROM persons ORDER BY id DESC LIMIT 1;");
                return (Int32)command.ExecuteScalar();
            }
        }
    }
}
