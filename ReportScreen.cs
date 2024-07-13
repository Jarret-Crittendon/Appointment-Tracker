using Appointment_Tracker;
using Appointment_Tracker;
using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appointment_Tracker
{
    public partial class ReportScreen : Form
    {
        public ReportScreen()
        {
            InitializeComponent();

            tableSetting();

            

            var users = LoadBox("userId", "userName", "user");
            var customers = LoadBox("customerId", "customerName", "customer");

            

            comboBoxUser.DataSource = users;
            comboBoxUser.DisplayMember = "Name";
            comboBoxUser.ValueMember = "ID";

            comboBoxCustomer.DataSource = customers;
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.ValueMember = "ID";

            

            var months = new List<Pair>
            {
                new Pair(1, "January"),
                new Pair(2, "February"),
                new Pair(3, "March"),
                new Pair(4, "April"),
                new Pair(5, "May"),
                new Pair(6, "June"),
                new Pair(7, "July"),
                new Pair(8, "August"),
                new Pair(9, "September"),
                new Pair(10, "October"),
                new Pair(11, "November"),
                new Pair(12, "December")
            };

            comboBoxMonth.DataSource = months;
            comboBoxMonth.DisplayMember = "Name";
            comboBoxMonth.ValueMember = "ID";

            comboBoxUser.SelectedIndex = 0;
            comboBoxMonth.SelectedIndex = 0;
            comboBoxCustomer.SelectedIndex = 0;

            RefreshAppt();
        }

        private void RefreshAppt()
        {
            var item = comboBoxUser.SelectedItem as Pair;
            string sql = string.Format("SELECT title, start, end, location, appointmentId FROM appointment " +
                         "WHERE userId = {0};", item.ID);
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, DBHost.conn))
                {
                    dataGridViewUser.DataSource = null;
                    var set = new DataSet();
                    adapter.Fill(set);
                    dataGridViewUser.DataSource = set.Tables[0];
                }
            }
        }

        private void tableSetting()
        {
            dataGridViewUser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUser.ReadOnly = true;
            dataGridViewUser.MultiSelect = false;

            dataGridViewUser.AutoGenerateColumns = false;
            dataGridViewUser.ColumnCount = 5;
            dataGridViewUser.Columns[0].Name = "Title";
            dataGridViewUser.Columns["Title"].DataPropertyName = "title";
            dataGridViewUser.Columns[1].Name = "Start";
            dataGridViewUser.Columns["Start"].DataPropertyName = "start";
            dataGridViewUser.Columns[2].Name = "End";
            dataGridViewUser.Columns["End"].DataPropertyName = "end";
            dataGridViewUser.Columns[3].Name = "Location";
            dataGridViewUser.Columns["Location"].DataPropertyName = "location";
            dataGridViewUser.Columns[4].Name = "ID";
            dataGridViewUser.Columns["ID"].DataPropertyName = "appointmentId";
        }

        private List<Pair> LoadBox(string idField, string nameField, string table)
        {
            var list = new List<Pair>();
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = string.Format("SELECT {0}, {1} FROM {2};", idField, nameField, table);
                MySqlCommand query = new MySqlCommand(sql, connection);
                connection.Open();
                try
                {
                    MySqlDataReader reader = query.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new Pair((int)reader[idField], reader[nameField].ToString()));
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(query.CommandText);
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        private void comboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAppt();
        }

        private void buttonGenerateType_Click(object sender, EventArgs e)
        {
            if (textBoxType.Text == string.Empty)
            {
                MessageBox.Show("The Type textbox cannot be empty.");
                return;
            }
            int count = 0;
            var theMonth = comboBoxMonth.SelectedItem as Pair;
            var type = textBoxType.Text;
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                string sql = string.Format("SELECT * FROM appointment WHERE month(start) = {0};", theMonth.ID);
                MySqlCommand command = new MySqlCommand(sql, conn);
                using (var adapter = new MySqlDataAdapter(sql, DBHost.conn))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    // The lambda here iterates through each row of the returned table, finds the row with the selected customer ID,
                    // then adds that to a list. At the end of the computation, all elements in the list are counted, which is returned
                    //  to count. The lambda makes the code much more succint. Previously, I had longer code that used SQL count instead.
                    count = table.AsEnumerable().Where(
                        r => r["type"].ToString() == type).ToList().Count();
                }
            }

            var s = string.Format("{0}, {1}: {2}", type, theMonth.Name, count);
            MessageBox.Show(s);

        }

        private void buttonGenerateCustomer_Click(object sender, EventArgs e)
        {
            int count = 0;
            var customer = comboBoxCustomer.SelectedItem as Pair;
            string sql = string.Format("SELECT * FROM appointment;");
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, DBHost.conn))
                {                   
                    var set = new DataSet();
                    adapter.Fill(set);
                    // The lambda here iterates through each row of the returned table, finds the row with the selected customer ID,
                    // then adds that to a list. At the end of the computation, all elements in the list are counted, which is returned
                    //  to count. The lambda makes the code much more succint.
                    count = set.Tables[0].AsEnumerable().Where(
                        r => r["customerId"].ToString() == customer.ID.ToString()).ToList().Count();
                }
            }

            var s = string.Format("{0}: {1}", customer.Name, count);
            MessageBox.Show(s);
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
