using Appointment_Tracker.Database;
using Appointment_Tracker;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969___Scheduler
{
    public partial class LoginScreen : Form
    {
        Logger logger = new Logger();

        public LoginScreen()
        {
            InitializeComponent();
            DetectGerman();
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {
            labelCountry.Text = GetCountry();
        }

        public string GetCountry()
        {
            return RegionInfo.CurrentRegion.DisplayName;
        }

        public void DetectGerman()
        {
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de")
            {
                labelUsername.Text = "Nutzername";
                labelUsername.Location = new Point(30, 26);
                labelPassword.Text = "Passwort";
                buttonSubmit.Text = "Senden";
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            Submit();
        }

        private void Submit()
        {
            var user = textBoxUsername.Text;
            var password = textBoxPassword.Text;

            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT password, userId FROM user WHERE userName = @User;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                // TODO: handle MySqlException for Open()
                connection.Open();
                command.Parameters.AddWithValue("@User", user);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (password.Equals(reader["password"].ToString()))
                        {
                            var id = (int)reader["userId"];
                            var main = new MainScreen(user, id);
                            logger.LogSuccess(user);
                            this.Hide();
                            main.ShowDialog();
                            this.Close();
                        }
                    }
                }
            }
            logger.LogError(user);

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de")
            {
                MessageBox.Show("Benutzername oder Passwort stimmen nicht mit unseren Aufzeichnungen überein.");
            }
            else
            {
                MessageBox.Show("Username or password does not match.");
            }
        }

        private void textBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Submit();
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Submit();
            }
        }
    }
}
