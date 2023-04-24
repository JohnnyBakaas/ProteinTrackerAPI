using Microsoft.AspNetCore.Mvc;
using ProteinTrackerAPI.Model;
using System.Security.Cryptography;
using System.Text;

internal class Program
{

    private static void Main(string[] args)
    {
        /*
        // Kobler til MySQL
        string connectionString = "server=localhost;port=3306;user=root;password=;database=protein_app";
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        // Koblet til MySQL

        string query = "SELECT * FROM users";

        using MySqlCommand command = new MySqlCommand(query, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["column1"]} - {reader["column2"]}");
        }
        */

        DB.ConectDBToMySQL();
        DB.LoginUser("Johnny", "admin");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();


        app.UseCors("AllowAllOrigins");


        app.MapGet("/users", () => new JsonResult(DB.Users))
             .WithName("GetUsers")
             .WithOpenApi();


        app.MapGet("/user", (string tokenFromClient) =>
        {
            DB.ConectData();
            Console.WriteLine("/user: " + tokenFromClient);

            var user = DB.GetUserFromToken(tokenFromClient);

            if (user == null)
            {
                return new JsonResult("Ugyldig");
            }

            return new JsonResult(user);
        })
            .WithName("GetUser")
            .WithOpenApi();


        app.MapGet("/login", (string userName, string password) =>
        {
            var rightUser = DB.LoginUser(userName, password);

            if (rightUser != null)
            {
                var token = new SessionToken(rightUser.Id);
                Console.WriteLine(token.TokenString);
                return new JsonResult(token);
            }

            return new JsonResult(null);
        })
            .WithName("GetLogin")
            .WithOpenApi();


        app.MapPost("/addFood", (Food newFood, string tokenFromClient) =>
        {
            if (newFood.UserId != SessionToken.TokenStringToId(tokenFromClient))
            {
                return Results.BadRequest("Invalid token.");
            }
            if (newFood == null)
            {
                return Results.BadRequest("Invalid food object.");
            }

            DB.AddFoodToSQL(newFood);

            // Endre kode under

            return Results.Ok("Food added successfully.");
        })
            .WithName("AddFood")
            .WithOpenApi();


        app.MapPost("/addWeight", (string weight, string coment, string tokenFromClient) =>
        {
            try
            {
                DB.AddWeightToSQL(weight, coment, tokenFromClient);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        })
            .WithName("addWeight")
            .WithOpenApi();


        app.MapPost("/updateUser", (string token, int KcalDelta) =>
        {
            Console.WriteLine($"Prøver å oppdatere bruker - {token}");
            DB.UpdateKcalDeltaInSQL(KcalDelta, token);
        })
            .WithOpenApi();



        app.Run();
    }

    static string sha256(string randomString)
    {
        using SHA256 crypt = SHA256.Create();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
        foreach (byte theByte in crypto)
        {
            hash += theByte.ToString("x2");
        }
        return hash;
    }
}

