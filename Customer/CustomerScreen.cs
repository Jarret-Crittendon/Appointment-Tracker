using Appointment_Tracker;
using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969___Scheduler
{
    public partial class CustomerScreen : Form
    {
        public CustomerScreen()
        {
            InitializeComponent();

            tableSetting();

            refreshTable();
        }

        private void refreshTable()
        {
            string sql = "SELECT customer.customerName, address.address, address.address2, city.city, country.country, address.phone, customer.customerId FROM customer, address, city, country\r\nWHERE customer.addressId = address.addressId AND address.cityId = city.cityId AND city.countryId = country.countryId;";
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                using (var adapter = new MySqlDataAdapter(sql, conn))
                {
                    dataGridViewCustomer.DataSource = null;
                    var set = new DataSet();
                    adapter.Fill(set);
                    dataGridViewCustomer.DataSource = set.Tables[0];
                }
            }
        }

        private void tableSetting()
        {
            dataGridViewCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCustomer.ReadOnly = true;
            dataGridViewCustomer.MultiSelect = false;
            
            dataGridViewCustomer.AutoGenerateColumns = false;
            dataGridViewCustomer.ColumnCount = 7;
            dataGridViewCustomer.Columns[0].Name = "Name";
            dataGridViewCustomer.Columns["Name"].DataPropertyName = "customerName";
            dataGridViewCustomer.Columns[1].Name = "Address";
            dataGridViewCustomer.Columns["Address"].DataPropertyName = "address";
            dataGridViewCustomer.Columns[2].Name = "Address2";
            dataGridViewCustomer.Columns["Address2"].DataPropertyName = "address2";
            dataGridViewCustomer.Columns[3].Name = "City";
            dataGridViewCustomer.Columns["city"].DataPropertyName = "city";
            dataGridViewCustomer.Columns[4].Name = "Country";
            dataGridViewCustomer.Columns["Country"].DataPropertyName = "country";
            dataGridViewCustomer.Columns[5].Name = "Phone";
            dataGridViewCustomer.Columns["Phone"].DataPropertyName = "phone";
            dataGridViewCustomer.Columns[6].Name = "ID";
            dataGridViewCustomer.Columns["ID"].DataPropertyName = "customerId";

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            var selected = dataGridViewCustomer.SelectedRows[0];

            int col = (int)selected.Cells["ID"].Value;

            var modify = new ModifyCustomer(col);
            modify.ShowDialog();

            refreshTable();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var add = new AddCustomer();
            add.ShowDialog();
            refreshTable();
        }

        private void buttonDeleteCustomer_Click(object sender, EventArgs e)
        {
            var selected = dataGridViewCustomer.SelectedRows[0];
            int id = (int)selected.Cells["ID"].Value;

            string cusName = selected.Cells["Name"].Value.ToString(); ;
            
            DialogResult deleteCustomer = MessageBox.Show("Do you wanted to erase the customer, " + cusName + "?",
                         "Delete Customer", MessageBoxButtons.YesNo);

            switch (deleteCustomer)
            {
                case DialogResult.Yes:
                    using (var conn = new MySqlConnection(DBHost.ConStr))
                    {
                        string entry = "DELETE FROM customer WHERE customerId = @cusID;";
                        var delete = new MySqlCommand(entry, conn);
                        conn.Open();
                        delete.Parameters.AddWithValue("@cusID", id);
                        try
                        {
                            delete.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            if (ex.Number == 1451)
                            {
                                MessageBox.Show("This record cannot be deleted as it is still tied to at least one appointment.");
                            } else
                            {
                                MessageBox.Show("Error:\n" + ex.Message + " \n " + ex.Number);
                            }
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

            refreshTable();
        }
    }
}
