using Microsoft.AspNetCore.Mvc.Rendering;
using NarutoUniverseProject.Data;
using NarutoUniverseProject.Models.OtherModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Services
{
    public class OtherService
    {
        private readonly ConnectionString _connString;

        public OtherService(ConnectionString connString)
        {
            _connString = connString;
        }

        public OtherViewModel GetOtherTables()
        {
            String sql = "SELECT s.id, s.name, pws.name FROM styles s INNER JOIN power_sources pws ON s.power_source_id = pws.id;";
            OtherViewModel viewModel = new OtherViewModel();
            viewModel.Countries = GetListOfItems("countries");
            viewModel.PowerSources = GetListOfItems("power_sources");
            viewModel.Positions = GetListOfItems("positions");
            viewModel.Styles = new List<Style>();

            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        viewModel.Styles.Add(new Style
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            PowerSource = reader.GetString(2)
                        });
                    }
                }
            }
            return viewModel;
        }

        public List<Other> GetListOfItems(String table)
        {
            String sql = String.Format("SELECT * FROM {0};", table);
            List<Other> items = new List<Other>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        items.Add(new Other
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                    }
                }
            }
            return items;
        }

    }
}
