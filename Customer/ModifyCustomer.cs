using C969___Scheduler;
using C969_Task.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969_Task
{
    public partial class ModifyCustomer : Form
    {
        private Customer customer { get; set; } = new Customer();
        private Address address { get; set; } = new Address();
        private City city { get; set; } = new City();
        private Country country { get; set; } = new Country();

        public ModifyCustomer(int id)
        {
            InitializeComponent();

            customer.ID = id;

            GetIDs();

            LoadFields();
        }

        public void LoadFields()
        {
            textBoxName.Text = customer.Name;
            textBoxAddress.Text = address.Address1;
            textBoxAddress2.Text = address.Address2;
            textBoxCity.Text = city.Name;
            textBoxPostalCode.Text = address.PostalCode;
            textBoxPhone.Text = address.Phone;
            textBoxCountry.Text = country.Name;
            textBoxPhone.Text = address.Phone;
        }

        private void GetIDs()
        {
            customer.LoadCustomer(customer.ID);
            address.LoadAddress(customer.AddressID);
            city.LoadCity(address.CityID);
            country.LoadCountry(city.CountryID);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            if (!AreFieldsEmpty())
            {
                UpdateCustomer();
                this.Close();
            }
        }

        private void CheckCustomer(Customer cust, int addrID)
        {
            if (textBoxName.Text != customer.Name)
            {
                cust.Name = textBoxName.Text;
                cust.AddressID = addrID;               
            }
        }

        private void CheckAddress(Address addr, int cityID)  //Change to just update
        {
            addr.CityID = cityID;
            if (textBoxAddress.Text != address.Address1 || textBoxAddress2.Text != address.Address2)
            {
                using (var connection = new MySqlConnection(DBHost.ConStr))
                {
                    // check if address+address2 in textbox is already in address table, using the stored city id
                    string query = "SELECT address, address2, addressId, cityId, postalCode FROM address " +
                                   "WHERE address = @address AND address2 = @address2 AND cityId = @cityID;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@address", textBoxAddress.Text);
                    command.Parameters.AddWithValue("@address2", textBoxAddress2.Text);
                    command.Parameters.AddWithValue("@cityID", cityID);
                    MySqlDataReader reader = command.ExecuteReader();


                    // if address+address2+city id in textbox is not in address table
                    if (reader.HasRows == false)
                    {
                        addr.Address1 = textBoxAddress.Text;
                        addr.Address2 = textBoxAddress2.Text;
                        
                        
                    }

                    // if address+address2+city id IS in address table,
                    else
                    {
                        while (reader.Read())
                        {
                            // store address
                            addr.Address1 = reader["address"].ToString();

                            // store address2
                            addr.Address2 = reader["address2"].ToString();

                            // store address id
                            addr.ID = (int)reader["addressId"];
                        }

                        addr.CityID = cityID;
                    }

                    
                }
            }

            addr.Phone = textBoxPhone.Text;
            addr.PostalCode = textBoxPostalCode.Text;
        }

        private void CheckNewCity(City cty, int countryID)
        {
            if (textBoxCity.Text != cty.Name)
            {
                using (var connection = new MySqlConnection(DBHost.ConStr))
                {
                    // check if the city+country id in textbox is already in city table
                    string query = "SELECT city, cityId FROM city " +
                                   "WHERE city = @city AND countryId = @countryID;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@city", textBoxCity.Text);
                    command.Parameters.AddWithValue("@countryID", countryID);
                    MySqlDataReader reader = command.ExecuteReader();


                    // if city+address id in textbox is not in city table,
                    if (reader.HasRows == false)
                    {
                        string new_city = textBoxCity.Text;

                        using (var conn = new MySqlConnection(DBHost.ConStr))
                        {
                            // insert entry for the city
                            string entry = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES " +
                                           "(@city, @countryID, @current, @user, @current, @user);";
                            var insert = new MySqlCommand(entry, conn);
                            conn.Open();
                            insert.Parameters.AddWithValue("@city", new_city);
                            insert.Parameters.AddWithValue("@countryID", countryID);
                            insert.Parameters.AddWithValue("@current", DateTime.UtcNow);
                            insert.Parameters.AddWithValue("@user", MainScreen.User);
                            try
                            {
                                insert.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("EXCEPTION, UpdateCity():\n" + ex.Message);
                            }
                        }

                        // store new city name (
                        cty.Name = new_city;

                        // store new city id
                        using (var conn = new MySqlConnection(DBHost.ConStr))
                        {
                            string search = "SELECT cityId FROM city " +
                                            "WHERE city = @city AND countryId = @countryID;";
                            var insert = new MySqlCommand(search, conn);
                            conn.Open();
                            insert.Parameters.AddWithValue("@city", new_city);
                            insert.Parameters.AddWithValue("@countryID", countryID);
                            var readerID = insert.ExecuteReader();
                            while (readerID.Read())
                            {
                                cty.ID = (int)readerID["cityId"];
                            }
                            cty.CountryID = countryID;
                        }
                    }

                    // if city+address id IS in city table,
                    else
                    {
                        while (reader.Read())
                        {
                            // store city name
                            cty.Name = reader["city"].ToString();

                            // store city id
                            cty.ID = (int)reader["cityId"];
                        }

                        cty.CountryID = countryID;
                    }
                }
            }
        }

        private void CheckNewCountry(Country cntry)
        {
            if (textBoxCountry.Text != cntry.Name)
            {
                using (var connection = new MySqlConnection(DBHost.ConStr))
                {
                    // check if the country in textbox is already in Country table
                    string query = "SELECT country, countryId FROM country " +
                                   "WHERE country = @Country;";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@Country", textBoxCountry.Text);
                    MySqlDataReader reader = command.ExecuteReader();


                    // if country in textbox is not in country table,
                    if (reader.HasRows == false)
                    {
                        string new_country = textBoxCountry.Text;

                        using (var conn = new MySqlConnection(DBHost.ConStr))
                        {
                            // insert entry for the country
                            string entry = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES " +
                                           "(@country, @current, @user, @current, @user);";
                            var insert = new MySqlCommand(entry, conn);
                            conn.Open();
                            insert.Parameters.AddWithValue("@country", new_country);
                            insert.Parameters.AddWithValue("@current", DateTime.UtcNow);
                            insert.Parameters.AddWithValue("@user", MainScreen.User);
                            try
                            {
                                insert.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("EXCEPTION, UpdateCountry():\n" + ex.Message);
                            } 
                        }

                        // store new country name (
                        cntry.Name = new_country;

                        // store new country id
                        using (var conn = new MySqlConnection(DBHost.ConStr))
                        {
                            // insert entry for the country
                            string search = "SELECT countryId FROM country " +
                                            "WHERE country = @country;";
                            var insert = new MySqlCommand(search, conn);
                            conn.Open();
                            insert.Parameters.AddWithValue("@country", cntry.Name);
                            var readerID = insert.ExecuteReader();
                            while (readerID.Read())
                            {
                                cntry.ID = (int)readerID["countryId"];
                            }
                        }
                    }

                    // if country IS in country table,
                    else
                    {
                        while (reader.Read())
                        {
                            // store country name
                            cntry.Name = reader["country"].ToString();

                            // store country id
                            cntry.ID = (int)reader["countryId"];
                        }
                    }
                }
            }
        }

        private bool AreFieldsEmpty()
        {
            bool check = false;
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                check = true;
                MessageBox.Show("The Name field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxAddress.Text))
            {
                check = true;
                MessageBox.Show("The Address field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxCity.Text))
            {
                check = true;
                MessageBox.Show("The City field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxCountry.Text))
            {
                check = true;
                MessageBox.Show("The Country field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxPostalCode.Text))
            {
                check = true;
                MessageBox.Show("The Postal Code field cannot be empty.");
            }

            else if (string.IsNullOrEmpty(textBoxPhone.Text))
            {
                check = true;
                MessageBox.Show("The Phone Number field cannot be empty.");
            }

            return check;
        }

        private void UpdateCustomer()
        {
            CheckNewCountry(country);
            CheckNewCity(city, country.ID);
            CheckAddress(address, city.ID);
            CheckCustomer(customer, address.ID);
            
            address.UpdateAddress();
            customer.UpdateCustomer();
        }
    }
}

    
