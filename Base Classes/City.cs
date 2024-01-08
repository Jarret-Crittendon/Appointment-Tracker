using C969_Task.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969_Task
{
    public class City
    {
        public City() { }
        public City(City right)
        {
            ID = right.ID;
            Name = right.Name;
            CountryID = right.CountryID;
            CreateDate = right.CreateDate;
            CreatedBy = right.CreatedBy;
            LastUpdate = right.LastUpdate;
            LastUpdateBy = right.LastUpdateBy;
        }

        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountryID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public void LoadCity(int cityID)
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy " +
                             "FROM city WHERE cityId = @cityID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cityID", cityID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                ID = cityID;
                while (reader.Read())
                {
                    Name = reader["city"].ToString();
                    CountryID = (int)reader["countryId"];
                    CreateDate = (DateTime)reader["createDate"];
                    CreatedBy = reader["createdBy"].ToString();
                    LastUpdate = (DateTime)reader["lastUpdate"];
                    LastUpdateBy = reader["lastUpdateBy"].ToString();
                }
            }
        }

        public void UpdateCity()
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE city " +
                               "SET city = @name, CountryId = @countryID, lastUpdate = @current, lastUpdateBy = @user " +
                               "WHERE cityId = @cityID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@name", Name);
                update.Parameters.AddWithValue("@countryID", CountryID);
                update.Parameters.AddWithValue("@current", LastUpdate);
                update.Parameters.AddWithValue("@user", LastUpdateBy);
                update.Parameters.AddWithValue("@cityID", ID);

                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION, City.UpdateCity():\n" + ex.Message);
                }
            }
        }

        public static bool operator ==(City left, City right)
        {
            if (left.ID != right.ID) return false;
            else if (left.Name.Equals(right.Name)) return false;
            else if (left.CountryID != right.CountryID) return false;
            else if (!left.CreateDate.Equals(right.CreateDate)) return false;
            else if (!left.CreatedBy.Equals(right.CreatedBy)) return false;

            return true;
        }

        public static bool operator !=(City left, City right)
        {
            return !(left == right);
        }
    }
}
