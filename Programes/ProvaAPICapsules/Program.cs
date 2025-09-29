using System;
using static System.Console;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using dam.models.api;

namespace dam.main;

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

                List<Capsule> capsules = JsonConvert.DeserializeObject<List<Capsule>>(response)!;

                WriteLine("Llista de càpsules: ");
                foreach (Capsule c in capsules)
                {
                    WriteLine($"Serial: {c.CapsuleSerial}, Status: {c.Status}, Original Launch: {c.OriginalLaunch}, Type: {c.Type}");
                    foreach (Mission m in c.CapsuleMissions!)
                    {
                        WriteLine($"Mission Name: {m.MissionName}, Flight: {m.Flight}");
                    }
                    WriteLine();
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}