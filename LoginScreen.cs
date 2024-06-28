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
using Appointment_Tracker.Base_Classes.Subclasses;

namespace C969___Scheduler
{
    public partial class LoginScreen : Form
    {
        Logger logger = new Logger();
        Dictionary<int, string> Admins = new Dictionary<int, string>();
        Dictionary<int, string> Techs = new Dictionary<int, string>();

        enum UserLevel
        {
            Normal,
            Technician,
            Admin
        }

        public LoginScreen()
        {
            InitializeComponent();
            DetectGerman();
            Admins.Add(3, "djeeta");
            Techs.Add(2, "gran");
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
            User currentUser;
            UserLevel level = UserLevel.Normal; 

            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT * FROM user WHERE userName = @User;";
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
                            foreach (var entry in Techs)
                            {
                                if (entry.Key == (int)reader["userID"] &&
                                    entry.Value == reader["username"].ToString())
                                {
                                    level = UserLevel.Technician;
                                }
                            }

                            foreach (var entry in Admins)
                            {
                                if (entry.Key == (int)reader["userID"] &&
                                    entry.Value == reader["username"].ToString())
                                {
                                    level = UserLevel.Admin;
                                }
                            }

                            var tempUser = new User
                            {
                                ID = (int)reader["userID"],
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Active = (reader["active"] as bool?).GetValueOrDefault(),
                                CreateDate = Convert.ToDateTime(reader["createDate"].ToString()),
                                CreatedBy = reader["createdBy"].ToString(),
                                LastUpdate = Convert.ToDateTime(reader["lastUpdate"]),
                                LastUpdatedBy = reader["lastUpdateBy"].ToString(),                              
                            };


                            if (level == UserLevel.Admin)
                            {
                                currentUser = new Admin(tempUser);
                            }
                            else if (level == UserLevel.Technician)
                            {
                                currentUser = new TechSupport(tempUser);
                            }
                            else
                            {
                                currentUser = tempUser;
                            }
                            

                            var id = (int)reader["userId"];
                            var main = new MainScreen(currentUser, id);
                            logger.LogSuccess(currentUser);
                            this.Hide();
                            main.ShowDialog();
                            this.Close();
                        } else
                        {
                            break;
                        }
                    }

                    return;
                }
            }

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de")
            {
                MessageBox.Show("Benutzername oder Passwort stimmen nicht mit unseren Aufzeichnungen überein.");
            }
            else
            {
                MessageBox.Show("Username or password does not match.");
            }

            logger.LogError(user);
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
