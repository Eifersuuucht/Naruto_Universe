using Microsoft.AspNetCore.Mvc.Rendering;
using NarutoUniverseProject.Data;
using NarutoUniverseProject.Extensions;
using NarutoUniverseProject.Models.AbilityModels;
using NarutoUniverseProject.Models.PersonModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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

        public BoxOfAbilitySummaryViewModel GetAbilities()
        {
            String sql = "SELECT * FROM abilities;";
            //ICollection<AbilitySummaryViewModel> abilities = new List<AbilitySummaryViewModel>();
            BoxOfAbilitySummaryViewModel boxOfAbilities = new BoxOfAbilitySummaryViewModel();
            boxOfAbilities.ViewModels = new List<AbilitySummaryViewModel>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        boxOfAbilities.ViewModels.Add(new AbilitySummaryViewModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TimeToCast = reader.GetInt32(2)
                        });
                    }
                }
            }
            return boxOfAbilities;
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

        public Int32 GetScalarTimeToCast(String scalar)
        {
            String sql = String.Format("SELECT {0}(time_to_cast_sec) FROM abilities;", scalar);

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                object result = command.ExecuteScalar();

                return (Int32)(Int64)result;
            }
        }

        public List<SelectListItem> GetInfoForSort()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Value = "a.id",
                Text = "Id"
            });
            list.Add(new SelectListItem
            {
                Value = "a.name",
                Text = "Name"
            });
            list.Add(new SelectListItem
            {
                Value = "a.time_to_cast_sec",
                Text = "Time To Cast (seconds)"
            });
            return list;
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

        public void GetFilteredAbilities(BoxOfAbilitySummaryViewModel bindModel)
        {
            String sql = "SELECT a.id, a.name, a.time_to_cast_sec FROM abilities a";
            StringBuilder sqlBuild = new StringBuilder(sql);
            ICollection<AbilitySummaryViewModel> abilities = new List<AbilitySummaryViewModel>();

            Boolean isInCountries = bindModel.Countries.IsAnyBoolInItemList();
            Boolean isInPositions = bindModel.Positions.IsAnyBoolInItemList();
            Boolean isInStyles = bindModel.Styles.IsAnyBoolInItemList();
            Boolean isInPowerSources = bindModel.PowerSources.IsAnyBoolInItemList();
            Boolean isFirstFromOneTable = true;

            if (isInStyles)
            {
                sqlBuild.Append(" INNER JOIN styles s ON a.style_id = s.id");
            }
            if (isInPowerSources && isInStyles)
            {
                sqlBuild.Append(" INNER JOIN power_sources pws ON s.power_source_id = pws.id");
            }
            else if (isInPowerSources && !isInStyles)
            {
                sqlBuild.Append(" INNER JOIN styles s ON a.style_id = s.id INNER JOIN power_sources pws ON s.power_source_id = pws.id");
            }
            if (isInCountries)
            {
                sqlBuild.Append(" INNER JOIN person_ability pa ON a.id = pa.ability_id INNER JOIN persons p ON pa.person_id = p.id INNER JOIN countries c ON p.country_id = c.id");
            }
            if (isInPositions && isInCountries)
            {
                sqlBuild.Append(" INNER JOIN positions ps ON p.position_id = ps.id");
            }
            else if(isInPositions && !isInCountries)
            {
                sqlBuild.Append(" INNER JOIN person_ability pa ON a.id = pa.ability_id INNER JOIN persons p ON pa.person_id = p.id INNER JOIN positions ps ON p.position_id = ps.id");
            }
            

            sqlBuild.AppendFormat(" WHERE (a.name like '%{0}%' AND a.time_to_cast_sec >= {1} AND a.time_to_cast_sec <= {2}",
                bindModel.Name, bindModel.MinTimeToCast, bindModel.MaxTimeToCast);

            //if there is at least one true then we will dive into loop
            if (isInCountries || isInPositions || isInStyles || isInPowerSources)
            {
                if (isInCountries)
                {
                    foreach (var item in bindModel.Countries)
                    {
                        if (item.Value)
                        {
                            if (isFirstFromOneTable)
                            {
                                sqlBuild.Append(") AND(");
                                isFirstFromOneTable = false;
                            }
                            else
                            {
                                sqlBuild.Append(" OR");
                            }
                            sqlBuild.AppendFormat(" c.name = '{0}'", item.Key);
                        }
                    }
                    isFirstFromOneTable = true;
                }
                if (isInPositions)
                {
                    foreach (var item in bindModel.Positions)
                    {
                        if (item.Value)
                        {
                            if (isFirstFromOneTable)
                            {
                                sqlBuild.Append(") AND(");
                                isFirstFromOneTable = false;
                            }
                            else
                            {
                                sqlBuild.Append(" OR");
                            }
                            sqlBuild.AppendFormat(" ps.name = '{0}'", item.Key);
                        }
                    }
                    isFirstFromOneTable = true;
                }
                if (isInStyles)
                {
                    foreach (var item in bindModel.Styles)
                    {
                        if (item.Value)
                        {
                            if (isFirstFromOneTable)
                            {
                                sqlBuild.Append(") AND(");
                                isFirstFromOneTable = false;
                            }
                            else
                            {
                                sqlBuild.Append(" OR");
                            }
                            sqlBuild.AppendFormat(" s.name = '{0}'", item.Key);
                        }
                    }
                    isFirstFromOneTable = true;
                }
                if (isInPowerSources)
                {
                    foreach (var item in bindModel.PowerSources)
                    {
                        if (item.Value)
                        {
                            if (isFirstFromOneTable)
                            {
                                sqlBuild.Append(") AND(");
                                isFirstFromOneTable = false;
                            }
                            else
                            {
                                sqlBuild.Append(" OR");
                            }
                            sqlBuild.AppendFormat(" pws.name = '{0}'", item.Key);
                        }
                    }
                }
                isFirstFromOneTable = true;
            }

            sqlBuild.Append(")");


            sqlBuild.AppendFormat(" ORDER BY {0}", bindModel.Sorting);

            if (bindModel.Descending)
            {
                sqlBuild.Append(" DESC");

            }


            sqlBuild.Append(";");

            sql = sqlBuild.ToString();

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
            bindModel.ViewModels = abilities;

        }
    }
}
