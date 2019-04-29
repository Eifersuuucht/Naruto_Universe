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
                command.CommandText = String.Format("SELECT p.id, p.name, p.age FROM persons p INNER JOIN person_ability pa ON p.id = pa.ability_id INNER JOIN abilities a ON pa.person_id = a.id WHERE a.id = {0};", id);
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
    }
}
