﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Software_2_Rykeem.Database
{
    public class Connection
    {
        public static MySqlConnection conn { get; set; }

        public static void startConnection()
        {

            try
            {
                string connect = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                conn = new MySqlConnection(connect);

                //opens the connection
                conn.Open();
                MessageBox.Show("Connection Successfull");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void stopConnection()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
                conn = null;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static bool Login(string username, string password)
        {
            try
            {
                string sql = "SELECT * FROM user WHERE userName = @username AND password = @password";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        return true;
                    else
                    {
                        return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public static void CustomerDatabase(DataGridView datagrid)
        {
            try
            {
                string sql = @"SELECT customerId,address.addressId,city.cityId,country.countryId, customerName, address, phone, city, country
                    FROM address, customer , city, country
                    WHERE  address.addressId = customer.addressId AND address.cityId = city.cityId AND city.countryId = country.countryId ";

               MySqlDataAdapter data = new MySqlDataAdapter(sql, conn);
                DataTable dataTable = new DataTable();
                data.Fill(dataTable);
                datagrid.DataSource = dataTable;

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void AppointmentDatabase(DataGridView datagrid)
        {
            try
            {
                string sql = @"SELECT appointmentId, appointment.customerId, userId, appointment.type, appointment.start, appointment.end 
                              FROM appointment, customer 
                              WHERE customer.customerId = appointment.customerId";

                MySqlDataAdapter data = new MySqlDataAdapter(sql, conn);
                DataTable dataTable = new DataTable();
                data.Fill(dataTable);
                datagrid.DataSource = dataTable;

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void UserIDComboBox(ComboBox comboBox)
        {
            try
            {
                string sql = @"SELECT userId FROM user";
                using (MySqlCommand datax = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataAdapter data = new MySqlDataAdapter(sql, conn))
                    {
                        DataTable dataTable = new DataTable();
                        data.Fill(dataTable);
                        comboBox.DataSource = dataTable;
                        comboBox.DisplayMember = "userId";


                    } 
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void CustomerIDComboBox(ComboBox comboBox)
        {
            try
            {
                string sql = @"SELECT customerId FROM customer";
                using (MySqlCommand datax = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataAdapter data = new MySqlDataAdapter(sql, conn))
                    {
                        DataTable dataTable = new DataTable();
                        data.Fill(dataTable);
                        comboBox.DataSource = dataTable;
                        comboBox.DisplayMember = "customerId";
                    }
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void ModifyCustomer(string customerid ,string addressid  ,string cityid, string countryid, string name, string address, string number, string city, string country)
        {
            try
            {
              



                string customer = @"UPDATE customer SET customerName = @name WHERE customerId = @customerid"; 
                string addressX = @"UPDATE address SET address.address = @address, phone = @number WHERE address.addressId = @addressid"; 
                string cityX = @"UPDATE city SET city.city = @city WHERE city.cityId = @cityid"; 
                string countryX = @"UPDATE country SET country.country = @country WHERE country.countryId = @countryid "; 


                using (MySqlCommand data = new MySqlCommand(customer, conn))
                {
                    data.Parameters.AddWithValue("@name", name);
                    data.Parameters.AddWithValue("@customerId", customerid);
                    data.ExecuteNonQuery();

                }
                

                using (MySqlCommand data = new MySqlCommand(addressX, conn))
                {
                    data.Parameters.AddWithValue("@address", address);
                    data.Parameters.AddWithValue("@number", number);
                    data.Parameters.AddWithValue("@addressId", addressid);
                    data.ExecuteNonQuery ();
                
                }

                using (MySqlCommand data = new MySqlCommand(cityX, conn))
                {
                    data.Parameters.AddWithValue("@city", city);
                    data.Parameters.AddWithValue("@cityId", cityid);
                    data.ExecuteNonQuery();

                }

                using (MySqlCommand data = new MySqlCommand(countryX, conn))
                {
                    data.Parameters.AddWithValue("@country", country);
                    data.Parameters.AddWithValue("@countryId", countryid);
                    data.ExecuteNonQuery();

                }





            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteCustomer(string customerid, string addressid, string cityid, string countryid, string name, string address, string number, string city, string country)
        {

            


            try
            {
                string customer = @"DELETE FROM customer  WHERE customerId = @customerid";
                string addressX = @"DELETE FROM address WHERE addressId = @addressid;";
                string cityX = @"DELETE FROM city WHERE cityId = @cityid";
                string countryX = @"DELETE FROM country WHERE countryId = @countryid";








                using (MySqlCommand data = new MySqlCommand(customer, conn))//1
                {

                    data.Parameters.AddWithValue("@customerId", customerid);
                    data.ExecuteNonQuery();

                }


                using (MySqlCommand data = new MySqlCommand(addressX, conn))//2
                {

                    data.Parameters.AddWithValue("@addressId", addressid);
                    data.ExecuteNonQuery();

                }

                using (MySqlCommand data = new MySqlCommand(cityX, conn)) //3
                {

                    data.Parameters.AddWithValue("@cityId", cityid);
                    data.ExecuteNonQuery();

                }

                using (MySqlCommand data = new MySqlCommand(countryX, conn)) // 4
                {

                    data.Parameters.AddWithValue("@countryId", countryid);
                    data.ExecuteNonQuery();
                }






            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static void AppointmentAdd(string customerId, string userId, string type, DateTime start, DateTime end)
        {
            try
            {
                string sql = @"INSERT INTO appointment(customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                               VALUES(@customerId,@userId,'not needed', 'not needed', 'not needed','not needed',@type,'not needed',@start,@end,NOW(),'test',NOW(),'test')";


                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@customerId",customerId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@start", start);
                    command.Parameters.AddWithValue("@end", end);
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

            public static void CustomerAdd(string name, string address, string number, string city, string country)
        {
            try
            {
                string sql1 = "INSERT INTO customer(customerName,addressId,active, createDate,createdBy,lastUpdate,lastUpdateBy) VALUES(@name,@addressId,1,NOW(),'test', NOW(),'test')";
                string sql2 = "INSERT INTO address(address,address2,cityId, postalcode, phone, createDate,createdBy,lastUpdate,lastUpdateBy) VALUES(@address,'',@cityId,'11111', @number, NOW(),'test',NOW(),'test')";
                string sql3 = "INSERT INTO city(city,countryId, createDate,createdBy,lastUpdate,lastUpdateBy) VALUES(@city,@countryId, NOW(), 'test', NOW(), 'test')";  
                string sql4 = "INSERT INTO country(country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES(@country, NOW(), 'test', NOW(), 'test')";
               


                using (MySqlCommand command4 = new MySqlCommand(sql4, conn)) 
                {
                    command4.Parameters.AddWithValue("@country", country); //4
                    command4.ExecuteNonQuery();
                }



                string CountryId = "SELECT LAST_INSERT_ID()"; //getting country ID to add to city 
                int countryId;

                using (MySqlCommand command4_5 = new MySqlCommand(CountryId, conn)) // starting with Country because its independent 
                {
                    countryId = Convert.ToInt32(command4_5.ExecuteScalar()); // gets country Id into int 
                }




                string CityId = "SELECT LAST_INSERT_ID()"; //getting city ID to add to address 
                int cityId;

                using (MySqlCommand command3 = new MySqlCommand(sql3, conn))
                {
                    command3.Parameters.AddWithValue("@city", city);// 3
                    command3.Parameters.AddWithValue("@countryId", countryId); // adds countryid to city table 
                    command3.ExecuteNonQuery();
                };

                using (MySqlCommand command3_5 = new MySqlCommand(CityId, conn)) 
                {
                    cityId = Convert.ToInt32(command3_5.ExecuteScalar()); // gets city Id
                }





                using (MySqlCommand command2 = new MySqlCommand(sql2, conn))
                {
                    command2.Parameters.AddWithValue("@address", address);
                    command2.Parameters.AddWithValue("@number", number);// adds city id to address table
                    command2.Parameters.AddWithValue("@cityId", cityId);
                    command2.ExecuteNonQuery();
                };


                string AddressId = "SELECT LAST_INSERT_ID()";
                int addressId;

                using (MySqlCommand command2_5 = new MySqlCommand(AddressId, conn))
                {
                    addressId = Convert.ToInt32(command2_5.ExecuteScalar()); // gets address Id
                }








                using (MySqlCommand command1 = new MySqlCommand(sql1, conn))
                {
                    command1.Parameters.AddWithValue("@name", name);/// adds address id to customer 
                    command1.Parameters.AddWithValue("@addressId", addressId);
                    command1.ExecuteNonQuery();
                };

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

    }
}
