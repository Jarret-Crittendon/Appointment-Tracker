using C969_Task.Database;
using C969_Task;
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

namespace C969___Scheduler
{
    public partial class AddAppt : Form
    {
        public AddAppt()
        {
            InitializeComponent();
            var customers = LoadBox("customerId", "customerName", "customer");
            var users = LoadBox("userId", "userName", "user");
            

            comboBoxCustomer.DataSource = customers;
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.ValueMember = "ID";

            comboBoxUser.DataSource = users;
            comboBoxUser.DisplayMember = "Name";
            comboBoxUser.ValueMember = "ID";

            dateTimeStart.Format = DateTimePickerFormat.Custom;
            dateTimeStart.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            dateTimeEnd.Format = DateTimePickerFormat.Custom;
            dateTimeEnd.CustomFormat = "yyyy/MM/dd HH:mm:ss";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                        list.Add(new Pair( (int)reader[idField], reader[nameField].ToString() ));
                    }
                } catch (MySqlException ex)
                {
                    MessageBox.Show(query.CommandText);
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        private bool AreFieldsEmpty()
        {
            bool check = false;
            if (string.IsNullOrEmpty(textBoxTitle.Text))
            {
                check = true;
                MessageBox.Show("The Title field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(richTextBoxDesc.Text))
            {
                check = true;
                MessageBox.Show("The Description field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxLocation.Text))
            {
                check = true;
                MessageBox.Show("The Location field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxContact.Text))
            {
                check = true;
                MessageBox.Show("The Contact field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxType.Text))
            {
                check = true;
                MessageBox.Show("The Type field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxURL.Text))
            {
                check = true;
                MessageBox.Show("The URL field cannot be empty.");
            }

            return check;
        }




        private void buttonSave_Click(object sender, EventArgs e)
        {
            
            if (!AreFieldsEmpty())
            {
                var appt = new Appointment();
                var item = comboBoxCustomer.SelectedItem as Pair;
                appt.CustomerID = item.ID;
                item = comboBoxUser.SelectedItem as Pair;
                appt.UserID = item.ID;
                var selectedUser = new Pair(item.ID, item.Name);
                appt.Title = textBoxTitle.Text;
                appt.Description = richTextBoxDesc.Text;
                appt.Location = textBoxLocation.Text;
                appt.Contact = textBoxContact.Text;
                appt.Type = textBoxType.Text;
                appt.URL = textBoxURL.Text;
                appt.Start = dateTimeStart.Value.ToUniversalTime();
                appt.End = dateTimeEnd.Value.ToUniversalTime(); 
                appt.CreateDate = DateTime.UtcNow;
                appt.CreatedBy = MainScreen.User;
                appt.LastUpdate = DateTime.UtcNow;
                appt.LastUpdateBy = MainScreen.User;

                if (!appt.WithinBusinessHours())
                {
                    MessageBox.Show("Appointments must be scheduled between " + Appointment.Open.ToString() +
                        " and " + Appointment.Closed.ToString());
                    return;      
                }

                if (!appt.StartEarlierThanEnd())
                {
                    MessageBox.Show("Error: the start of the appointment is scheduled later than the end.");
                    return;
                }

                Pair overlap = appt.GetOverlaps();
                if (overlap != null)
                {
                    MessageBox.Show("This appointment overlaps with another appointment, [" + overlap.Name + "], Appt. ID: " +
                        overlap.ID);
                    return;
                }

                DBHost.InsertAppointment(appt);
                
                this.Close();
            }
        }
    }
}
