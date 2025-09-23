> *Dilluns 22/09/2025*

# Conceptes API

## Comunicació entre aplicacions
- Fitxers
- RPC *(Remote Procedure Call)*
- Sockets i protocols propis
    - Permet que varies persones vegin informació en temps real
- AJAX *(Asynchronous JavaScript and XML)*
    - Aplicacions web canvien informació sobre HTTP(s) - Protocol Standard.
    - Permet canviar una part de la pàgina web.
    - Pots fer una part de la pàgina dinàmica mentre tens una altre estàtica
- APIs
    - El canvi de paradigma: APIs sobre HTTP


## Definició REST

***!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! INACABAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!***

<br>

## Principis bàsics REST

### Uniform Interface
Els recursos es manipulen sempre de la mateixa forma. <br>
Un cop feta la petició al server, pot haver dos opcions:

1. Et torna l'informació demanada en format **JSON**:
    - Retorna **200** -> Tot OK
2. Dona error:
    - Retorna **404** -> Not found
    - Retorna **500** -> Internal server error
    
### Layered System
Entre client i servidor hi ha multiples capes.

La de Cache és un server intermediari que guarda les peticions i les seves respostes per treura-li carrega al servidor principal si es repeteix una petició.

### Code on Demand (Opcional)
El servidor li envia codi al usuari perque l'executi

<br>

## Arquitectura REST

Cada recurs té un URL únic <br>
Representacions: JSON, XML. *(JSON com a estàndard)* <br>

Operacions sobre recursos **CRUD**:
- `GET` -> Read
- `POST` -> Create
- `PUT/PATCH` -> Update
- `DELETE` -> Delete

### Ruta - Definició

En una API REST, cada recurs (usuari, comanda, producte...) s'identifica amb un URI únic. <br>
El path és la part de l'URI que defineix quina col·lecció o recurs concret vol accedir.

URL (ruta) -> Verb
- La ruta (URL) diu **QUE** vols manipular.
- El verb HTTP diu **QUE** vols fer amb aquest recurs.

Exemple:
- `GET /users/5` -> obtenir usuari 5
- `DELETE /users/5` -> eliminar usuari 5
- `POST /users/5` -> actualitzar usuari 5

<br>

## Disseny API

### Paràmetres

Paràmetres PATH

- Formen part de la ruta de la URL
- S'utilitzen per identificar un recurs concret
- Exemples:
    - `/users/`
    - `/users/(id)` => `/users/5`
    - `/users/(id)/orders` => `/users/5/orders`

        \* "(id)" és variable


Paràmetres QUERY

- S'afegeixen al final de la URL després del *?*
- S'utilitzen per filtrar, ordenar o modificar la resposta, no per identificar un recurs únic
- Exemples:
    - `/users?sort=desc&limit=10`
    - `/users/5/orders?status=pending`

### Path vs Query parameters

Podem utilitzar `/users/5/orders?status=pending` o `/users/5/orders/pending`

**Avantatges** de *Query* parameters:
- *Flexible:* Pots posar `status=pending`, `status=paid`

***!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! INACABAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!***

<br>

## Autenticació i seguretat

### 1. API Key

Clau *(string)* que el client envia amb cada petició.
Com funciona:
- El proveïdor et dona una clau única. (Per exemple: `123abcXYZ`)
- Cada cop que truques l'API, envies una clau. (Normalment al header `Authorization` o com a `?apikey=`)
- Si el client no l'envia, no pot utilitzar l'API

Pros:
- Molt fàcil d'implementar
Contres:
- No autentifica un usuari real, només qui te la clau. Bàsicament si es filtra, et poden robar l'identitat

### 2. Token Based: JWT (JSON Web Token)

Token *(String xifrat)* generat després de fer login *(usuari + contrasenya)*
Com funciona:
1. L'usuari s'autentica al servidor *(amb credencials)*
2. El servidor genera un **JWT** signat
3. El client guarda aquest token i el passa al header en cada petició
    ```

    Authorization: Bearer <token>

    ```
4. El servidor valida el token sense haver de consultar la DB cada cop

Caracteristiques:
- El token expira
***!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! INACABAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!***

### 3. OAuth2

Protocol d'autorització delegada
Com funciona *(simplificat)*:
1. Un usuari es valida en un proveïdor de confiança *(Google, Github...)*
2. El proveïdor dona un token al client per accedir a l'API

Pros:
- *Molt segur:* Evita guardar contrasenyes en apps terceres

Contres:
- És el més complex d'implementar


<br>
<br>


# Part Pràctica

## Aplicació mètode GET amb API d'exemple

Exemple: https://api.spacexdata.com/v4/launches/latest

Documentació: https://docs.spacexdata.com/

Informació sobre endpoint Capsules: https://docs.spacexdata.com/#4376c913-2589-4afd-a5f2-80ab8adc3fd0
- https://api.spacexdata.com/v3/capsules
- https://api.spacexdata.com/v3/capsules?capsule_serial=C101

Exercici *(a través del navegador)*
- Obtenir la informació de tots els Dragon
- Obtenir la informació del segon Dragon (obtingut del llistat anterior)

<br>

## Obtenir la informació de totes les capsules - Consola sense classes

```C#
using System;
using static System.Console;

using System.Net.Http;
using System.Threading.Tasks;

class Program {
    static async Task Main() {
        using (HttpClient client = new HttpClient()) {
            string url = "https://api.spacexdata.com/v3/capsules";

            try {
                string response = await client.GetStringAsync(url);
                Console.WriteLine("Resposta de l'API:");
                Console.WriteLine(response);
            } catch (Exception ex) {
                Console.WriteLine("Error en la connexió: " + ex.Message);
            }
        }
    }
}
```