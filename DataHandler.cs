using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: SHOULD HAVE AN OPTION TO CHECK IF RULE EXCISTS IN DATABASE, THIS SHOULD BE OPTIONAL, BECAUSE NOT EVERY DATA IS UNIQUE.
namespace LoginModule
{
    class DataHandler
    {
        //usage: 1 insert value: string datainsert = dataInsert.InsertData(table:"UserDetails", where:"name", where2:"NoRule", value: username, value2: "No Value");
        //usage: 2 insert 2 values:  string datainsert = dataInsert.InsertData(table:"UserDetails", where:"name", where2:"password", value: username, value2: password);
        public string InsertData(string table, string where, string where2, string value, string value2)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=bermdingetje\\sqlexpress;Initial Catalog=LoginModule;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    if (where2 == "NoRule")
                    {
                        command.CommandText = "INSERT into " + table + " (" + where + ") VALUES (@value)";
                        command.Parameters.AddWithValue("@value", value);
                    }
                    else
                    {
                        command.CommandText = "INSERT into " + table + " (" + where + ", " + where2 + ") VALUES (@value, @value2)";
                        command.Parameters.AddWithValue("@value", value);
                        command.Parameters.AddWithValue("@value2", value2);
                    }
                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string error = ("Exception Occre while creating table:" + ex.Message + "\t" + ex.GetType());
                        return error;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "added!";
                }
            }
        }
        public string SelectData(string table, string pointer, string where, string value)
        {
            if (table == string.Empty)
            { return "0"; }
            else
            {
                return Select(table, pointer, where, value);
            }
        }
        private string Select(string table, string pointer, string where, string value)
        {

            using (SqlConnection connection = new SqlConnection("Data Source=bermdingetje\\sqlexpress;Initial Catalog=LoginModule;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = ("SELECT " + where + " FROM " + table + " WHERE " + where + "= @value");
                    command.Parameters.AddWithValue("@value", value);
                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string Wusername = (string.Format("{0}", reader[where]));
                                return Wusername;
                            }
                            else
                            {
                                return "No Values selected.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = ("Exception Occre while creating table:" + ex.Message + "\t" + ex.GetType());
                        return error;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
