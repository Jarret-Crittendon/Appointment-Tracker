using C969___Scheduler;
using C969_Task.Database;
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

namespace C969_Task
{
    public partial class UpdateAppt : Form
    {
        Appointment targetAppt;

        public UpdateAppt(int id)
        {
            InitializeComponent();
            targetAppt = new Appointment();
            targetAppt.LoadAppt(id);

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

            LoadFields();  
        }

        private void LoadFields()
        {
            comboBoxCustomer.SelectedValue = targetAppt.CustomerID;
            comboBoxUser.SelectedValue = targetAppt.UserID;
            textBoxTitle.Text = targetAppt.Title;
            richTextBoxDesc.Text = targetAppt.Description;
            textBoxLocation.Text = targetAppt.Location;
            textBoxContact.Text = targetAppt.Contact;
            textBoxType.Text = targetAppt.Type;
            textBoxURL.Text = targetAppt.URL;
            dateTimeStart.Value = targetAppt.Start;
            dateTimeEnd.Value = targetAppt.End;
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
                var newAppt = new Appointment();
                newAppt.ID = targetAppt.ID;
                var item = comboBoxCustomer.SelectedItem as Pair;
                newAppt.CustomerID = item.ID;
                item = comboBoxUser.SelectedItem as Pair;
                newAppt.UserID = item.ID;
                newAppt.Title = textBoxTitle.Text;
                newAppt.Description = richTextBoxDesc.Text;
                newAppt.Location = textBoxLocation.Text;
                newAppt.Contact = textBoxContact.Text;
                newAppt.Type = textBoxType.Text;
                newAppt.URL = textBoxURL.Text;
                newAppt.Start = dateTimeStart.Value.ToUniversalTime();
                newAppt.End = dateTimeEnd.Value.ToUniversalTime();
                newAppt.CreateDate = targetAppt.CreateDate.ToUniversalTime();
                newAppt.CreatedBy = targetAppt.CreatedBy;
                newAppt.LastUpdate = DateTime.UtcNow;
                newAppt.LastUpdateBy = MainScreen.User;

                if (!newAppt.WithinBusinessHours())
                {
                    MessageBox.Show("Appointments must be scheduled between " + Appointment.Open.ToString() +
                        " and " + Appointment.Closed.ToString() + "\nStart: " + newAppt.Start.ToLocalTime().ToString());
                    return;
                }

                if (!newAppt.StartEarlierThanEnd())
                {
                    MessageBox.Show("Error: the start of the appointment is scheduled later than the end.");
                    return;
                }

                Pair overlap = newAppt.GetOverlaps();
                if (overlap != null)
                {
                    MessageBox.Show("This appointment overlaps with another appointment, [" + overlap.Name + "], Appt. ID: " + 
                        overlap.ID);
                    return;
                }

                if (newAppt != targetAppt)
                {
                    newAppt.UpdateAppointment();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No changes detected.");
                    return;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
