# Inserir producte

[Directori 07-API](https://github.com/projecteinf/EF/tree/main/07-API)

## SQL

```SQL

Use master;
-- drop database dbdemo;
-- create database dbdemo;
Use dbdemo;

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL ,    -- UNIQUE
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);

```
---

## Fitxer configuració

Fitxer: appSettings.json

```JSON
{
    "ConnectionStrings": {
        "DefaultConnection" : "Server=localhost;Database=dbdemo;User Id=sa;Password=Patata1234;TrustServerCertificate=true;Encrypt=false"
    }
}
```

## Fitxer de Projecte

&rarr; Cal indicar que és una aplicació Web


Fitxer: ADO.csproj

```XML

<Project Sdk="Microsoft.NET.Sdk.Web">  <!-- Utilitzar Sdk d'aplicació Web -->

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Data.SqlClient" Version="6.1.1" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.9" />
    <PackageReference Include="System.Data.SqlClient" Version="4.9.0" />
  </ItemGroup>

</Project>

```

---

## Programa principal

&rarr; Eliminació de la class Program.  

&rarr; Enfoc correcte per a projectes curts (mini web).  

&rarr; Utilitzem el port 5000, per defecte en aplicacions ASP.NET Core.  


Fitxer: Program.cs

```CSharp

using Microsoft.Extensions.Configuration;
using dbdemo.Services;
using dbdemo.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuració
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseConnection dbConn = new DatabaseConnection(connectionString);

WebApplication webApp = builder.Build();

// Registra els endpoints en un mètode separat
webApp.MapProductEndpoints(dbConn);

webApp.Run();

```

## DatabaseConnection

Fitxer: Services/DatabaseConnection.cs

```CSharp
using Microsoft.Data.SqlClient;
using static System.Console;
using dbdemo.Model;

namespace dbdemo.Services;

public class DatabaseConnection
{
    private readonly string _connectionString;
    public SqlConnection sqlConnection;
    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }
    public bool Open()
    {
        sqlConnection = new SqlConnection(_connectionString);

        try
        {
            sqlConnection.Open();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public void Close()
    {
        sqlConnection.Close();
    }
}
```

---

## End points o rutes de Product

Fitxer: EndPoints/Product.cs

```CSharp

using dbdemo.Repository;
using dbdemo.Services;
using dbdemo.Model;

namespace dbdemo.Endpoints;

public static class Endpoints
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /products
        app.MapGet("/products", () =>
        {
            List<ProductADO>  products = ProductADO.GetAll(dbConn);
            return Results.Ok(products);
        });

        // GET Product by id
        app.MapGet("/products/{id}", (Guid id) =>
        {
            ProductADO product = ProductADO.GetById(dbConn, id);

            return product is not null
                ? Results.Ok(product)
                : Results.NotFound(new { message = $"Product with Id {id} not found." });

            // if (product is not null)
            // {
            //     return Results.Ok(product);
            // }
            // else
            // {
            //     return Results.NotFound(new { message = $"Product with Id {id} not found." });
            // }
        });




        // POST /products
        app.MapPost("/products", (ProductRequest req) =>
        {
            ProductADO productADO = new ProductADO
            {
                Id = Guid.NewGuid(),
                Code = req.Code,
                Name = req.Name,
                Price = req.Price
            };

            productADO.Insert(dbConn);

            return Results.Created($"/products/{productADO.Id}", productADO);
        });
    }

   
}

public record ProductRequest(string Code, string Name, decimal Price);  // Com ha de llegir el POST

```

---

## ProductADO - Repository

Fitxer: Repository/ProductADO.cs

```CSharp

using Microsoft.Data.SqlClient;
using static System.Console;
using dbdemo.Services;

namespace dbdemo.Repository;

class ProductADO
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }

    public void Insert(DatabaseConnection dbConn)
    {

        dbConn.Open();

        string sql = @"INSERT INTO Products (Id, Code, Name, Price)
                        VALUES (@Id, @Code, @Name, @Price)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@Code", Code);
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Price", Price);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");
        dbConn.Close();
    }

    public static List<ProductADO> GetAll(DatabaseConnection dbConn)
    {
        List<ProductADO> products = new();

        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new ProductADO
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
        }

        dbConn.Close();
        return products;
    }

    public static ProductADO? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        ProductADO? product = null;

        if (reader.Read())
        {
            product = new ProductADO
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }

        dbConn.Close();
        return product;
    }
}


```

## Product - Model

&rarr; De moment no l'estem utilitzant.

Fitxer: Model/Product.cs

```CSharp

namespace dbdemo.Model;

public class Product
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

```

---

## Cos petició POST

```JSON
{
  "code": "P001",
  "name": "Ordinador portàtil",
  "price": 999.99
}

```