using C969_Task.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969_Task
{
    public class Customer
    {
        public Customer() { }
        public Customer(Customer right)
        {
            ID = right.ID;
            Name = right.Name;
            AddressID = right.AddressID;
            Active = right.Active;
            CreateDate = right.CreateDate;
            CreatedBy = right.CreatedBy;
            LastUpdate = right.LastUpdate;
            LastUpdateBy = right.LastUpdateBy;
        }

        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AddressID { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        

        public void LoadCustomer(int cusID)
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy " +
                             "FROM customer WHERE customerId = @cusID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cusID", cusID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                ID = cusID;
                while (reader.Read())
                {
                    Name = reader["customerName"].ToString();
                    AddressID = (int)reader["addressId"];
                    Active = (bool)reader["active"];
                    CreateDate = (DateTime)reader["createDate"];
                    CreatedBy = reader["createdBy"].ToString();
                    LastUpdate = (DateTime)reader["lastUpdate"];
                    LastUpdateBy = reader["lastUpdateBy"].ToString();
                }
            }
        }

        public void UpdateCustomer()
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE customer " +
                               "SET customerName = @name, addressId = @addrID, lastUpdate = @current, lastUpdateBy = @user " +
                               "WHERE customerId = @cusID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@name", Name);
                update.Parameters.AddWithValue("@addrID", AddressID);
                update.Parameters.AddWithValue("@current", LastUpdate);
                update.Parameters.AddWithValue("@user", LastUpdateBy);
                update.Parameters.AddWithValue("@cusID", ID);

                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION, Customer.UpdateCustomer():\n" + ex.Message);
                }
            }
        }

        public static bool operator ==(Customer left, Customer right)
        {
            if (left.ID != right.ID) return false;
            else if (left.Name.Equals(right.Name)) return false;
            else if (left.AddressID != right.AddressID) return false;
            //else if (left.Active != right.Active) return false;
            else if (!left.CreateDate.Equals(right.CreateDate)) return false;
            else if (!left.CreatedBy.Equals(right.CreatedBy)) return false;

            return true;
        }

        public static bool operator !=(Customer left, Customer right)
        {
            return !(left == right);
        }
    }


}
