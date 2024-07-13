using Appointment_Tracker;
using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Appointment_Tracker
{
    public partial class MainScreen : Form
    {

        public static User currentUser;
        public static int UserID {  get; set; }

        public static string Username {  get; private set; }
        public MainScreen(User user, int id)
        {
            InitializeComponent();

            tableSetting();
            RefreshAppt();

            currentUser = user;
            UserID = id;

            ImpendingAppointments();
        }

        private void RefreshAppt()
        {
            string sql = "SELECT title, start, end, location, appointmentId FROM appointment ";
            if (radioButtonMonth.Checked)
            {
                sql += "WHERE month(start) = " + DateTime.Now.Month + ";";
            }
            else if (radioButtonWeek.Checked)
            {
                sql += "WHERE start BETWEEN date_add(now(), interval(1-dayofweek(now())) day) AND date_add(now(), interval(7-dayofweek(now())) day);";
            }
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, DBHost.conn))
                {
                    dataGridViewCalendar.DataSource = null;
                    var set = new DataSet();
                    adapter.Fill(set);
                    dataGridViewCalendar.DataSource = set.Tables[0];
                }
            }
        }

        private void tableSetting()
        {
            dataGridViewCalendar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCalendar.ReadOnly = true;
            dataGridViewCalendar.MultiSelect = false;

            dataGridViewCalendar.AutoGenerateColumns = false;
            dataGridViewCalendar.ColumnCount = 5;
            dataGridViewCalendar.Columns[0].Name = "Title";
            dataGridViewCalendar.Columns["Title"].DataPropertyName = "title";
            dataGridViewCalendar.Columns[1].Name = "Start";
            dataGridViewCalendar.Columns["Start"].DataPropertyName = "start";
            dataGridViewCalendar.Columns[2].Name = "End";
            dataGridViewCalendar.Columns["End"].DataPropertyName = "end";
            dataGridViewCalendar.Columns[3].Name = "Location";
            dataGridViewCalendar.Columns["Location"].DataPropertyName = "location";
            dataGridViewCalendar.Columns[4].Name = "ID";
            dataGridViewCalendar.Columns["ID"].DataPropertyName = "appointmentId";
        }

        private void radioButtonMonth_CheckedChanged(object sender, System.EventArgs e)
        {
            RefreshAppt();
        }

        private void radioButtonWeek_CheckedChanged(object sender, System.EventArgs e)
        {
            string sql = "SELECT title, start, end, location, appointmentId FROM appointment " +
                         "WHERE start BETWEEN date_add(now(), interval(1-dayofweek(now())) day) " +
                         "AND date_add(now(), interval(7-dayofweek(now())) day);"
            ;
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, conn))
                {
                    var set = new DataSet();
                    adapter.Fill(set);
                    dataGridViewCalendar.DataSource = set.Tables[0];
                }
            }
        }

        private void buttonManage_Click(object sender, System.EventArgs e)
        {
            var customer = new CustomerScreen();
            customer.ShowDialog();

            RefreshAppt();
        }

        private void buttonAddAppt_Click(object sender, System.EventArgs e)
        {
            var add = new AddAppt();
            add.ShowDialog();

            RefreshAppt();
        }

        private void buttonUpdateAppt_Click(object sender, System.EventArgs e)
        {
            try
            {
                var selected = dataGridViewCalendar.SelectedRows[0];
                int id = (int)selected.Cells["ID"].Value;
                var update = new UpdateAppt(id);
                update.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            RefreshAppt();
        }

        private void buttonDeleteAppt_Click(object sender, EventArgs e)
        {
            var selected = dataGridViewCalendar.SelectedRows[0];
            int id = (int)selected.Cells["ID"].Value;

            string title = selected.Cells["Title"].Value.ToString(); ;

            DialogResult deleteAppt = MessageBox.Show("Do you want to erase the appointment, " + title + "?",
                         "Delete Appointment", MessageBoxButtons.YesNo);

            switch (deleteAppt)
            {
                case DialogResult.Yes:
                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        string entry = "DELETE FROM appointment WHERE appointmentId = @apptID;";
                        var delete = new MySqlCommand(entry, conn);
                        conn.Open();
                        delete.Parameters.AddWithValue("@apptID", id);
                        try
                        {
                            delete.ExecuteNonQuery();
                            RefreshAppt();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("EXCEPTION:\n" + ex.Message);
                        }
                    }
                    break;

                case DialogResult.No:
                    break;
            }

        }

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM appointment;";
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, conn))
                {
                    var set = new DataSet();
                    adapter.Fill(set);
                    dataGridViewCalendar.DataSource = set.Tables[0];
                }
            }
        }

        private void dataGridViewCalendar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is DateTime dt)
            {
                e.Value = dt.ToLocalTime();
            }
        }

        private void ImpendingAppointments()
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT title, start " +
                             "FROM appointment WHERE userId = @userID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userID", MainScreen.UserID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var apptTitle = reader["title"].ToString();
                    var apptStart = (DateTime)reader["start"];
                    apptStart = apptStart.ToLocalTime();
                    var zeroMinutes = new TimeSpan(0, 0, 0);
                    var fifteenMinutes = new TimeSpan(0, 15, 0);
                    // subtract apptStart time from DateTime.Now time, if value is less than 15, alert user
                    var compare = apptStart.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
                    
                    if (compare >= zeroMinutes && compare <= fifteenMinutes)
                    {
                        var s = string.Format("{0} has an appointment soon!\n[{1}] starting at {2}", MainScreen.currentUser.Username, apptTitle, apptStart);
                        MessageBox.Show(s);
                    } 
                }
            }
        }

        private void buttonReports_Click(object sender, EventArgs e)
        {
            var reports = new ReportScreen();
            reports.ShowDialog();
        }
    }
}