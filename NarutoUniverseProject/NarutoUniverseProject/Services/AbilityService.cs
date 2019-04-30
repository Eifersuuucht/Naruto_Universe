using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AbilityService
    {
        private readonly ConnectionString _connString;

        public AbilityService(ConnectionString connString)
        {
            _connString = connString;
        }

        public ICollection<AbilitySummaryViewModel> GetAbilities()
        {
            String sql = "SELECT * FROM abilities;";
            ICollection<AbilitySummaryViewModel> abilities = new List<AbilitySummaryViewModel>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        abilities.Add(new AbilitySummaryViewModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TimeToCast = reader.GetInt32(2)
                        });
                    }
                }
            }
            return abilities;
        }

        public AbilityDetailedViewModel GetAbility(Int32 id)
        {
            String sql = String.Format("SELECT a.id, a.name, a.time_to_cast_sec, s.name, pws.name  FROM abilities a INNER JOIN styles s ON a.style_id = s.id INNER JOIN power_sources pws ON s.power_source_id = pws.id WHERE a.id = {0};", id);
            AbilityDetailedViewModel ability = new AbilityDetailedViewModel();

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ability.Id = reader.GetInt32(0);
                        ability.Name = reader.GetString(1);
                        ability.TimeToCast = reader.GetInt32(2);
                        ability.Style = reader.GetString(3);
                        ability.PowerSource = reader.GetString(4);
                    }
                }
                else
                {
                    return null;
                }
                reader.Close();
                command.CommandText = String.Format("SELECT p.id, p.name, p.age FROM persons p INNER JOIN person_ability pa ON p.id = pa.person_id INNER JOIN abilities a ON pa.ability_id = a.id WHERE a.id = {0};", id);
                reader = command.ExecuteReader();

                ability.Persons = new List<PersonSummaryViewModel>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ability.Persons.Add(new PersonSummaryViewModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2)
                        });
                    }
                }
                return ability;
            }
        }

        public Int32 CreateAbility(AbilityCreateBindModel bindModel)
        {
            String sql = String.Format("INSERT INTO abilities(name, time_to_cast_sec, style_id) VALUES('{0}', {1}, {2});",
                bindModel.Name, bindModel.TimeToCast, bindModel.StyleId);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 added = command.ExecuteNonQuery();

                if (added == 0)
                {
                    return -1;
                }

                command.CommandText = String.Format("SELECT id FROM abilities ORDER BY id DESC LIMIT 1;");
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


        public AbilityUpdateBindModel GetAbilityForUpdate(Int32 id)
        {
            String sql = String.Format("SELECT a.id, a.name, a.time_to_cast_sec, s.id FROM abilities a INNER JOIN styles s ON a.style_id = s.id WHERE a.id = {0};", id);
            AbilityUpdateBindModel abilityForUpdate = new AbilityUpdateBindModel();

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        abilityForUpdate.Id = reader.GetInt32(0);
                        abilityForUpdate.Name = reader.GetString(1);
                        abilityForUpdate.TimeToCast = reader.GetInt32(2);
                        abilityForUpdate.StyleId = reader.GetInt32(3);
                    }
                }
                else
                {
                    return null;
                }

            }
            return abilityForUpdate;
        }

        public void UpdateAbility(AbilityUpdateBindModel bindModel)
        {
            String sql = String.Format("UPDATE abilities SET name = '{0}', time_to_cast_sec = {1}, style_id = {2} WHERE id = {3};",
                bindModel.Name, bindModel.TimeToCast, bindModel.StyleId, bindModel.Id);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }

        public void DeleteAbility(Int32 id)
        {
            String sql = String.Format("DELETE FROM abilities WHERE id = {0};", id);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                Int32 updated = command.ExecuteNonQuery();

            }
        }
    }
}
