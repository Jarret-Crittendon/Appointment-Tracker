using Appointment_Tracker.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Appointment_Tracker
{
    public class Address
    {
        public int ID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityID { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public Address() { }
        public Address(Address right)
        {
            this.ID = right.ID;
            this.Address1 = right.Address1;
            this.Address2 = right.Address2;
            this.CityID = right.CityID;
            this.PostalCode = right.PostalCode;
            this.Phone = right.Phone;
            this.CreateDate = right.CreateDate;
            this.CreatedBy = right.CreatedBy;
            this.LastUpdate = right.LastUpdate;
            this.LastUpdateBy = right.LastUpdateBy;
        }

        public void LoadAddress(int addrID)
        {
            using (var connection = new MySqlConnection(DBHost.ConStr))
            {
                string sql = "SELECT address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy " +
                             "FROM address WHERE addressId = @addrID;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@addrID", addrID);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                ID = addrID;
                while (reader.Read())
                {
                    Address1 = reader["address"].ToString();
                    Address2 = reader["address2"].ToString();
                    CityID = (int)reader["cityId"];
                    PostalCode = reader["postalCode"].ToString();
                    Phone = reader["phone"].ToString();
                    CreateDate = (DateTime)reader["createDate"];
                    CreatedBy = reader["createdBy"].ToString();
                    LastUpdate = (DateTime)reader["lastUpdate"];
                    LastUpdateBy = reader["lastUpdateBy"].ToString();
                }
            }
        }

        public void UpdateAddress()
        {
            using (var conn = new MySqlConnection(DBHost.ConStr))
            {
                // update entry for the postalCode
                string entry = "UPDATE address " +
                               "SET address = @address, address2 = @address2, cityId = @cityID, postalCode = @code, phone = @phone, lastUpdate = @current, lastUpdateBy = @user " +
                               "WHERE addressId = @addrID;";
                var update = new MySqlCommand(entry, conn);
                conn.Open();
                update.Parameters.AddWithValue("@address", Address1);
                update.Parameters.AddWithValue("@address2", Address2);
                update.Parameters.AddWithValue("@cityID", CityID);
                update.Parameters.AddWithValue("@code", PostalCode);
                update.Parameters.AddWithValue("@phone", Phone);
                update.Parameters.AddWithValue("@current", LastUpdate);
                update.Parameters.AddWithValue("@user", LastUpdateBy);
                update.Parameters.AddWithValue("@addrID", ID);

                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("EXCEPTION, Address.UpdateAddress():\n" + ex.Message);
                }
            }
        }

        public static bool operator ==(Address left, Address right)
        {
            if (left.ID != right.ID) return false;
            else if (left.Address1.Equals(right.Address1)) return false;
            else if (left.Address2.Equals(right.Address2)) return false;
            else if (left.CityID != right.CityID) return false;
            else if (!left.PostalCode.Equals(right.PostalCode)) return false;
            else if (!left.Phone.Equals(right.Phone)) return false;
            else if (!left.CreateDate.Equals(right.CreateDate)) return false;
            else if (!left.CreatedBy.Equals(right.CreatedBy)) return false;

            return true;
        }

        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }
    }
}