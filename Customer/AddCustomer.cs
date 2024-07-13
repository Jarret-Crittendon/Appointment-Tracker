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
    public partial class AddCustomer : Form
    {
        public int CustomerID { get; set; }
        public int AddressID { get; set; }
        public int CityID { get; set; }
        public int CountryID { get; set; }
        private string Country { get; set; }
        private string City { get; set; }
        private string Address { get; set; }
        private string Address2 { get; set; }
        private string Customer { get; set; }
        private string PostalCode { get; set; }
        private string Phone { get; set; }


        public AddCustomer()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InsertCustomer()
        {
            InsertCountry();
            InsertCity();
            InsertAddress();

            Customer = textBoxName.Text;
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                string entry = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES " +
                               "(@name, @addrID, @active, @current, @user, @current, @user);";
                var insert = new MySqlCommand(entry, conn);
                conn.Open();
                insert.Parameters.AddWithValue("@name", Customer);
                insert.Parameters.AddWithValue("@addrID", AddressID);
                insert.Parameters.AddWithValue("@active", 1);
                insert.Parameters.AddWithValue("@current", DateTime.UtcNow);
                insert.Parameters.AddWithValue("@user", MainScreen.currentUser.Username);

                try
                {
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION:\n" + ex.Message);
                }
            }
        }

        private void InsertAddress()
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                // check if address+address2, phone, postalcode in textbox is already in address table, using the stored city id
                string query = "SELECT address, address2, addressId, cityId, phone, postalCode FROM address " +
                               "WHERE address = @address AND address2 = @address2 AND cityId = @cityID;";
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@address", textBoxAddress.Text);
                command.Parameters.AddWithValue("@address2", textBoxAddress2.Text);
                command.Parameters.AddWithValue("@cityID", CityID);
                MySqlDataReader reader = command.ExecuteReader();


                // if address+address2+city id in textbox is not in address table
                if (reader.HasRows == false)
                {
                    string new_addr = textBoxAddress.Text;
                    string new_addr2 = textBoxAddress2.Text;

                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        // insert entry for the city
                        Phone = textBoxPhone.Text;
                        PostalCode = textBoxPostalCode.Text;
                        string entry = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES " +
                                       "(@address, @address2, @cityID, @code, @phone, @current, @user, @current, @user);";
                        var insert = new MySqlCommand(entry, conn);
                        conn.Open();
                        insert.Parameters.AddWithValue("@address", new_addr);
                        insert.Parameters.AddWithValue("@address2", new_addr2);
                        insert.Parameters.AddWithValue("@cityID", CityID);
                        insert.Parameters.AddWithValue("@code", PostalCode);
                        insert.Parameters.AddWithValue("@phone", Phone);
                        insert.Parameters.AddWithValue("@current", DateTime.UtcNow);
                        insert.Parameters.AddWithValue("@user", MainScreen.currentUser.Username);
                        try
                        {
                            insert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("EXCEPTION:\n" + ex.Message);
                        }
                    }

                    // store new address
                    Address = new_addr;
                    // store new address2
                    Address2 = new_addr2;

                    // store new address id
                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        string search = "SELECT addressId FROM address " +
                                        "WHERE address = @address AND address2 = @address2 AND cityId = @cityID;";
                        var getID = new MySqlCommand(search, conn);
                        conn.Open();
                        getID.Parameters.AddWithValue("@address", Address);
                        getID.Parameters.AddWithValue("@address2", Address2);
                        getID.Parameters.AddWithValue("@cityID", CityID);
                        var readerID = getID.ExecuteReader();
                        while (readerID.Read())
                        {
                            AddressID = (int)readerID["addressId"];
                        }
                    }
                }

                // if address+address2+city id IS in address table,
                else
                {
                    string registeredPhone = null;
                    string registeredCode = null;
                    while (reader.Read())
                    {
                        // store address
                        Address = reader["address"].ToString();

                        // store address2
                        Address2 = reader["address2"].ToString();

                        // store address id
                        AddressID = (int)reader["addressId"];

                        registeredPhone = reader["phone"].ToString();
                        if (!string.IsNullOrEmpty(textBoxPhone.Text) && registeredPhone != textBoxPhone.Text)
                        {
                            DialogResult differentNumber = MessageBox.Show(
                                "The entry for this address has a different phone number.  Would you like to replace that number with " + textBoxPhone.Text + "?",
                                "Different Number Registered", MessageBoxButtons.YesNo);

                            switch(differentNumber)
                            {
                                case DialogResult.Yes:
                                    UpdatePhone(textBoxPhone.Text);
                                    Phone = textBoxPhone.Text;
                                    break;

                                case DialogResult.No:
                                    Phone = registeredPhone;
                                    break;
                            }
                        }

                        registeredCode = reader["postalCode"].ToString();
                        if (!string.IsNullOrEmpty(textBoxPostalCode.Text) && registeredCode != textBoxPostalCode.Text)
                        {
                            DialogResult differentCode = MessageBox.Show(
                                "The entry for this address has a different postal code.  Would you like to replace that code with " + textBoxPostalCode.Text + "?",
                                "Different Number Registered", MessageBoxButtons.YesNo);

                            switch (differentCode)
                            {
                                case DialogResult.Yes:
                                    UpdatePostalCode(textBoxPostalCode.Text);
                                    PostalCode = textBoxPostalCode.Text;
                                    break;

                                case DialogResult.No:
                                    Phone = registeredPhone;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePhone(string phone)
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE address " +
                               "SET phone = @phone " +
                               "WHERE addressId = @addressID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@phone", phone);
                update.Parameters.AddWithValue("@addressID", AddressID);
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

        private void UpdatePostalCode(string postalCode)
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE address " +
                               "SET postalCode = @code " +
                               "WHERE addressId = @addressID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@code", postalCode);
                update.Parameters.AddWithValue("@addressID", AddressID);
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

        private void InsertCountry()
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
                        insert.Parameters.AddWithValue("@user", MainScreen.currentUser.Username);

                        try
                        {
                            insert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("EXCEPTION, " + new_country + ", " + MainScreen.currentUser.Username + ":\n" + ex.Message);
                        }
                    }

                    // store new country name (
                    Country = new_country;

                    // store new country id
                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        // insert entry for the country
                        string search = "SELECT countryId FROM country " +
                                        "WHERE country = @country;";
                        var getID = new MySqlCommand(search, conn);
                        conn.Open();
                        getID.Parameters.AddWithValue("@country", Country);
                        var readerID = getID.ExecuteReader();
                        while (readerID.Read())
                        {
                            CountryID = (int)readerID["countryId"];
                        }
                    }
                }

                // if country IS in country table,
                else
                {
                    while (reader.Read())
                    {
                        // store country name
                        Country = reader["country"].ToString();

                        // store country id
                        CountryID = (int)reader["countryId"];
                    }
                }
            }
        }

        private void InsertCity()
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                // check if the city+country id in textbox is already in city table
                string query = "SELECT city, cityId FROM city " +
                               "WHERE city = @city AND countryId = @countryID;";
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@city", textBoxCity.Text);
                command.Parameters.AddWithValue("@countryID", CountryID);
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
                        insert.Parameters.AddWithValue("@countryID", CountryID);
                        insert.Parameters.AddWithValue("@current", DateTime.UtcNow);
                        insert.Parameters.AddWithValue("@user", MainScreen.currentUser.Username);
                        try
                        {
                            insert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("EXCEPTION:\n" + ex.Message);
                        }
                    }

                    // store new city name (
                    City = new_city;

                    // store new city id
                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        string search = "SELECT cityId FROM city " +
                                        "WHERE city = @city AND countryId = @countryID;";
                        var getID = new MySqlCommand(search, conn);
                        conn.Open();
                        getID.Parameters.AddWithValue("@city", new_city);
                        getID.Parameters.AddWithValue("@countryID", CountryID);
                        var readerID = getID.ExecuteReader();
                        while (readerID.Read())
                        {
                            CityID = (int)readerID["cityId"];
                        }
                    }
                }

                // if city+address id IS in city table,
                else
                {
                    while (reader.Read())
                    {
                        // store city name
                        City = reader["city"].ToString();

                        // store city id
                        CityID = (int)reader["cityId"];
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!AreFieldsEmpty())
            {
                InsertCustomer();
                this.Close();
            }
        }
    }
}
