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

        public ICollection<Position> GetPositions()
        {
            String sql = "SELECT * FROM positions;";
            ICollection<Position> positions = new List<Position>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        positions.Add(new Position
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                    }
                }
            }
            return positions;
        }

        public ICollection<Country> GetCountries()
        {
            String sql = "SELECT * FROM countries;";
            ICollection<Country> countries = new List<Country>();
            using (SQLiteConnection connection = new SQLiteConnection(_connString.Value))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        countries.Add(new Country
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                    }
                }
            }
            return countries;
        }
    }
}
