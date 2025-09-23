using System;
using static System.Console;

using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://api.spacexdata.com/v3/capsules";

            try
            {
                string response = await client.GetStringAsync(url);
                Console.WriteLine("Resposta de l'API:");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la connexió: " + ex.Message);
            }
        }
    }
}