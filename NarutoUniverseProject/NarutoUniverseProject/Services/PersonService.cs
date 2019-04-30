using Microsoft.AspNetCore.Mvc.Rendering;
using NarutoUniverseProject.Data;
using NarutoUniverseProject.Models.AbilityModels;
using NarutoUniverseProject.Models.PersonModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            String sql = String.Format("INSERT INTO persons(name, age, position_id, country_id) VALUES('{0}', {1}, {2}, {3});",
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
                Int32 result = -1;
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }

                return result;
            }
        }

        public List<SelectListItem> GetSelectListItems(String table)
        {
            String sql = String.Format("SELECT * FROM {0};", table);
            List<SelectListItem> items = new List<SelectListItem>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Value = reader.GetInt32(0).ToString(),
                            Text = reader.GetString(1),
                        });
                    }
                }
            }
            return items;
        }

        public PersonUpdateBindModel GetPersonForUpdate(Int32 id)
        {
            String sql = String.Format("SELECT p.id, p.name, p.age, ps.id, c.id FROM persons p INNER JOIN positions ps ON p.position_id = ps.id INNER JOIN countries c on p.country_id = c.id WHERE p.id = {0};", id);
            PersonUpdateBindModel personForUpdate = new PersonUpdateBindModel();

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        personForUpdate.Id = reader.GetInt32(0);
                        personForUpdate.Name = reader.GetString(1);
                        personForUpdate.Age = reader.GetInt32(2);
                        personForUpdate.PositionId = reader.GetInt32(3);
                        personForUpdate.CountryId = reader.GetInt32(4);
                    }
                }
                else
                {
                    return null;
                }
 
            }
            return personForUpdate;
        }

        public void UpdatePerson(PersonUpdateBindModel bindModel)
        {
            String sql = String.Format("UPDATE persons SET name = '{0}', age = {1}, position_id = {2}, country_id = {3} WHERE id = {4};",
                bindModel.Name, bindModel.Age, bindModel.PositionId, bindModel.CountryId, bindModel.Id);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }

        public void DeletePerson(Int32 id)
        {
            String sql = String.Format("DELETE FROM persons WHERE id = {0};", id);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }

        public PersonAddAbilityViewModel GetAbilitiesForAdding(Int32 id)
        {
            String sql = String.Format("SELECT * FROM abilities WHERE id NOT IN (SELECT ability_id FROM person_ability WHERE person_id = {0}) ; ", id);
            PersonAddAbilityViewModel objForAdding = new PersonAddAbilityViewModel();
            objForAdding.Id = id;

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                objForAdding.AbilitiesForAdding = new List<AbilitySummaryViewModel>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objForAdding.AbilitiesForAdding.Add(new AbilitySummaryViewModel{
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TimeToCast = reader.GetInt32(2)});
                    }
                }
                return objForAdding;
            }
        }

        public void AddAbility(PersonAddAbilityBindModel bindModel)
        {
            String sql = String.Format("INSERT INTO person_ability(person_id, ability_id) VALUES({0}, {1});",
                bindModel.Id, bindModel.AbilityId);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }

        public void DeleteAbilityOfPerson(Int32 id, Int32 personId)
        {
            String sql = String.Format("DELETE FROM person_ability WHERE ability_id = {0} AND person_id = {1};", id, personId);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }

        public Filter
    }
}
