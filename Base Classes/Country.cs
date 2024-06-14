using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker
{
    public class Country
    {
        public Country() { }
        public Country(Country right)
        {
            ID = right.ID;
            Name = right.Name;
            CreateDate = right.CreateDate;
            CreatedBy = right.CreatedBy;
            LastUpdate = right.LastUpdate;
            LastUpdateBy = right.LastUpdateBy;
        }

        public int ID { get; set; }
        public string Name {  get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public void LoadCountry(int countryID)
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT country, createDate, createdBy, lastUpdate, lastUpdateBy " +
                             "FROM country WHERE countryId = @countryID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@countryID", countryID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                ID = countryID;
                while (reader.Read())
                {
                    Name = reader["country"].ToString();
                    CreateDate = (DateTime)reader["createDate"];
                    CreatedBy = reader["createdBy"].ToString();
                    LastUpdate = (DateTime)reader["lastUpdate"];
                    LastUpdateBy = reader["lastUpdateBy"].ToString();
                }
            }
        }

        public void UpdateCountry()
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE country " +
                               "SET country = @name, lastUpdate = @current, lastUpdateBy = @user " +
                               "WHERE country = @countryID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@name", Name);
                update.Parameters.AddWithValue("@current", LastUpdate);
                update.Parameters.AddWithValue("@user", LastUpdateBy);
                update.Parameters.AddWithValue("@countryID", ID);

                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION, Country.UpdateCountry():\n" + ex.Message);
                }
            }
        }

        public static bool operator ==(Country left, Country right)
        {
            if (left.ID != right.ID) return false;
            else if (left.Name.Equals(right.Name)) return false;
            else if (!left.CreateDate.Equals(right.CreateDate)) return false;
            else if (!left.CreatedBy.Equals(right.CreatedBy)) return false;

            return true;
        }

        public static bool operator !=(Country left, Country right)
        {
            return !(left == right);
        }
    }
}
