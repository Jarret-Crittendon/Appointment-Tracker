using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker
{
    public class Appointment
    {
        public int? ID { get; set; }
        public int? CustomerID { get; set; }
        public int? UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public static TimeSpan Open = new TimeSpan(8, 0, 0);

        public static TimeSpan Closed = new TimeSpan(20, 0, 0);

        public static bool operator ==(Appointment left, Appointment right)
        {
            if (left.ID != right.ID) return false;
            else if (left.CustomerID != right.CustomerID) return false;
            else if (left.UserID != right.UserID) return false;
            else if (!left.Title.Equals(right.Title)) return false;
            else if (!left.Description.Equals(right.Description)) return false;
            else if (!left.Location.Equals(right.Location)) return false;
            else if (!left.Contact.Equals(right.Contact)) return false;
            else if (!left.Type.Equals(right.Type)) return false;
            else if (!left.URL.Equals(right.URL)) return false;
            else if (!left.Start.Equals(right.Start)) return false;
            else if (!left.End.Equals(right.End)) return false;
            else if (!left.CreateDate.Equals(right.CreateDate)) return false;
            else if (!left.CreatedBy.Equals(right.CreatedBy)) return false;

            return true;
        }

        public static bool operator !=(Appointment left, Appointment right)
        {
            return !(left == right);
        }

        public void UpdateAppointment()
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE appointment " +
                               "SET customerId = @cusID, userId = @userID, title = @title, description = @desc, location = @loc, contact = @contact, type = @type, url = @url, start = @start, end = @end, lastUpdate = @current, lastUpdateBy = @user " +
                               "WHERE appointmentId = @apptID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@cusID", CustomerID);
                update.Parameters.AddWithValue("@userID", UserID);
                update.Parameters.AddWithValue("@title", Title);
                update.Parameters.AddWithValue("@desc", Description);
                update.Parameters.AddWithValue("@loc", Location);
                update.Parameters.AddWithValue("@contact", Contact);
                update.Parameters.AddWithValue("@type", Type);
                update.Parameters.AddWithValue("@url", URL);
                update.Parameters.AddWithValue("@start", Start);
                update.Parameters.AddWithValue("@end", End);
                update.Parameters.AddWithValue("@current", LastUpdate);
                update.Parameters.AddWithValue("@user", LastUpdateBy);
                update.Parameters.AddWithValue("@apptID", ID);

                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION:\n" + ex.Message);
                }
            }
        }

        public void LoadAppt(int apptID)
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy " +
                             "FROM appointment WHERE appointmentId = @apptID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@apptID", apptID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                ID = apptID;
                while (reader.Read())
                {
                    CustomerID = (int)reader["customerId"];
                    UserID = (int)reader["userId"];
                    Title = reader["title"].ToString();
                    Description = reader["description"].ToString();
                    Location = reader["location"].ToString();
                    Contact = reader["contact"].ToString();
                    Type = reader["type"].ToString();
                    URL = reader["url"].ToString();
                    Start = (DateTime)reader["start"];
                    Start = Start.ToLocalTime();
                    End = (DateTime)reader["end"];
                    End = End.ToLocalTime();
                    CreateDate = (DateTime)reader["createDate"];
                    CreateDate = CreateDate.ToLocalTime();
                    CreatedBy = reader["createdBy"].ToString();
                    LastUpdate = (DateTime)reader["createDate"];
                    LastUpdate = LastUpdate.ToLocalTime();
                    LastUpdateBy = reader["createdBy"].ToString();
                }
            }
        }

        public bool WithinBusinessHours()
        {
            if (Start.ToLocalTime().TimeOfDay < Open ) { return false; }
            else if (Start.ToLocalTime().TimeOfDay > Closed ) { return false; }
            else if (End.ToLocalTime().TimeOfDay < Open ) { return false; }
            else if (End.ToLocalTime().TimeOfDay  > Closed ) { return false; }

            return true;
        }

        public bool StartEarlierThanEnd()
        {
            if (Start > End) { return false; }

            return true;
        }

        public Pair GetOverlaps()
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT appointmentId, title, start, end " +
                             "FROM appointment WHERE userId = @userID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userID", UserID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var apptID = (int)reader["appointmentId"];           
                    var apptTitle  = reader["title"].ToString();
                    var apptStart = (DateTime)reader["start"];
                    apptStart = apptStart.ToLocalTime();
                    var apptEnd = (DateTime)reader["end"];
                    apptEnd = apptEnd.ToLocalTime();
                    
                    var currentStart = Start.ToLocalTime();
                    var currentEnd = End.ToLocalTime();
                    if (currentStart >= apptStart &&  currentStart <= apptEnd)
                    {
                        if (apptID != ID)
                        {
                            return new Pair(apptID, apptTitle);
                        }
                    }

                    if (currentEnd >= apptStart && currentEnd <= apptEnd)
                    {
                        if (apptID != ID)
                        {
                            return new Pair(apptID, apptTitle);
                        }
                    }
                }
            }

            return null;
        }
    }
}
