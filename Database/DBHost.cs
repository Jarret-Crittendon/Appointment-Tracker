using C969___Scheduler;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace C969_Task.Database
{
    public class DBHost
    {
        public static MySqlConnection conn { get; set; } = null;
        public static string ConStr { get; private set; } = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
        public static void StartConnection()
        {
            try
            {
                // get connection string
                string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                conn = new MySqlConnection(constr);

                // open the connection
                conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Close the connection
                conn?.Close();
            }
        }

        public static void InsertAppointment(Appointment newAppt)
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                string entry = "INSERT INTO appointment(customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                               "VALUES " +
                               "(@cusID, @userID, @title, @desc, @loc, @contact, @type, @url, @start, @end, @current, @user, @current, @user);"
;
                var insert = new MySqlCommand(entry, conn);
                conn.Open();
                insert.Parameters.AddWithValue("@cusID", newAppt.CustomerID);
                insert.Parameters.AddWithValue("@userID", newAppt.UserID);
                insert.Parameters.AddWithValue("@title", newAppt.Title);
                insert.Parameters.AddWithValue("@desc", newAppt.Description);
                insert.Parameters.AddWithValue("@loc", newAppt.Location);
                insert.Parameters.AddWithValue("@contact", newAppt.Contact);
                insert.Parameters.AddWithValue("@type", newAppt.Type);
                insert.Parameters.AddWithValue("@url", newAppt.URL);
                insert.Parameters.AddWithValue("@start", newAppt.Start);
                insert.Parameters.AddWithValue("@end", newAppt.End);
                insert.Parameters.AddWithValue("@current", newAppt.CreateDate);
                insert.Parameters.AddWithValue("@user", MainScreen.User);
                try
                {
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION, InsertAppointment():\n" + ex.Message);
                }
            }
        }

        public static void CloseConnection()
        {
            try
            {
                conn?.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
