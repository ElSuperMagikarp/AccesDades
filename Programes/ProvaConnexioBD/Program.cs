using System;
using Microsoft.Data.SqlClient;
using static System.Console;

class Program {
    static void Main()
    {
        string connectionString = "Server=localhost;Database=dbdemo;User Id=sa;Password=Patata1234;TrustServerCertificate=true;Encrypt=false";

        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            WriteLine("Connexió oberta correctament!");
        }
        catch (Exception ex)
        {
            WriteLine("Error a la connexió: " + ex.Message);
        }
        connection.Close();
    }
}