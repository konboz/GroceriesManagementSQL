using System;

namespace GroceriesManagementSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            Basket conn = new Basket();
            conn.SaveToJson();
            conn.InsertToDb();

            Console.ReadLine();
        }
    }
}
