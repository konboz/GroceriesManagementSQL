using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;


namespace GroceriesManagementSQL
{
    public class Basket
    {
        public List<Product> groceries = new List<Product>();

        private const string filenamejson = @"Groceries.json";

        public void SaveToJson()
        {
            List<Product> groceries = new List<Product>();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {

                    string s = "Server =localhost; Database = GroceryStore; Integrated Security=SSPI;Persist Security Info=False;";


                    conn.ConnectionString = s;
                    conn.Open();
                    // Create the command
                    SqlCommand command = new SqlCommand("SELECT [Id] ,[Name], [Price], [Category] FROM[dbo].[Basket]", conn);
                    // Add the parameters
                    command.Parameters.Add(new SqlParameter("0", 1));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groceries.Add(new Product(int.Parse(reader[0].ToString()), reader[1].ToString(),
                            double.Parse(reader[2].ToString()), reader[3].ToString()));
                        }

                        string jsonData = JsonConvert.SerializeObject(groceries);
                        //write string to file
                        File.WriteAllText(filenamejson, jsonData);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void InsertToDb()
        {
            List<Product> groceries = new List<Product>();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    string s3 = "Server =localhost; Database = GroceryStore; Integrated Security=SSPI;Persist Security Info=False;";


                    conn.ConnectionString = s3;
                    conn.Open();
                    string data = File.ReadAllText(filenamejson);
                    groceries = JsonConvert.DeserializeObject<List<Product>>(data);


                    foreach (Product t in groceries)
                    {

                        SqlCommand InsertCommand = new SqlCommand("INSERT INTO basket VALUES (@0, @1, @2)", conn);
                        InsertCommand.Parameters.Add(new SqlParameter("0", t.Name));
                        InsertCommand.Parameters.Add(new SqlParameter("1", t.Price));
                        InsertCommand.Parameters.Add(new SqlParameter("2", t.Category));
                        int c = InsertCommand.ExecuteNonQuery();
                        Console.WriteLine(c);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
