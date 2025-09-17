> *Dimecres 17/09/2025*

# Pool de conexions

Una base de dades és costosa

Un pool de connexions té 3 parts:

---

### Connect

`SqlConnection con = new SQLConnection(connectionString)` -> Crea una instància de la conexió

- **SQLConnection** és una classe
- **con** és el nom de l'instancia de la classe
- **connectionString** és el string per conectarse a la *Base de dades* que conté:
    - **user**: Habitualment *"sa"* (Super Administrator)
    - **password**
    - **port**: Opcional

---

### Open

`com.Open();` -> Fa la connexió

***Aquesta*** és l'operació costosa <br>
*Aquí* també és el moment en que es verifica si es pot conectar o no (Utilitzant el *connectionString*) <br>

Pots fer varios `com.Open()` i es guardarán múltiples conexions en una sola instància

---

### Close

`com.Close();` -> Tenca la connexió

---

# Prova Connexió BD

Un petit programa per provar a connectar a una base de dades

```C#
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
        }
        catch (Exception ex)
        {
            WriteLine("Error a la connexió: " + ex.Message);
        }
        WriteLine("Connexió oberta correctament!");
        connection.Close();
    }
}
```