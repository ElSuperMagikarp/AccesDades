> *Dimecres 17/09/2025*

# Pool de connexions

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