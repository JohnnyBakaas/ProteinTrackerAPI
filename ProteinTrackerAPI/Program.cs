using Microsoft.AspNetCore.Mvc;
using ProteinTrackerAPI.Model;
using System.Security.Cryptography;
using System.Text;

internal class Program
{

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DB dataBase = new DB();
        DB.ConectData();
        DB.DataDump();

        DB.UpdateUser(new User("john_smith", 1001, "password123", -5, "male"));

        DB.DataDump();

        // Aktiv ødelegging starter

        Console.WriteLine(SesionToken.TokenStringToId("kake"));

        // Aktiv ødelegging slutter


        //FirebaseApp.Create();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
            Console.WriteLine(tokenFromClient);
            Console.WriteLine(SesionToken.TokenStringToId(tokenFromClient));
            if (DB.Users.Any(user => user.Id == SesionToken.TokenStringToId(tokenFromClient)))
            {
                foreach (var user in DB.Users)
                {
                    if (user.Id == SesionToken.TokenStringToId(tokenFromClient)) return new JsonResult(user);
                }
            }
            return new JsonResult("Ugyldig");
        })
             .WithName("GetUser")
             .WithOpenApi();

        app.MapGet("/login", (string userName, string password) =>
        {
            if (DB.Users.Any(user => user.ValidateUsernameAndPasword(userName, password)))
            {
                var rightUser = DB.Users.First(user => user.ValidateUsernameAndPasword(userName, password));
                var token = new SesionToken(rightUser.Id);
                Console.WriteLine(token.TokenString);
                return new JsonResult(token);
            }
            return new JsonResult(null);
        }).WithName("GetLogin")
             .WithOpenApi();

        app.MapPost("/addFood", (Food newFood, string tokenFromClient) =>
        {
            Console.WriteLine();
            Console.WriteLine("Token from client");
            Console.WriteLine(tokenFromClient);
            Console.WriteLine("--------------------");
            if (newFood.UserId != SesionToken.TokenStringToId(tokenFromClient)) return Results.BadRequest("Invalid token.");
            if (newFood == null) return Results.BadRequest("Invalid food object.");

            DB.AddFood(new Food(newFood.Name, newFood.Kcal, newFood.Protein, newFood.ConsumptionDateTime, newFood.UserId));
            DB.ConectData();
            return Results.Ok("Food added successfully.");
        })
            .WithName("AddFood")
            .WithOpenApi();


        app.MapPost("/addWeight", (string weight, string coment, string tokenFromClient) =>
        {
            try
            {
                DB.AddWeight(new Weight(int.Parse(weight), coment, SesionToken.TokenStringToId(tokenFromClient), DateTime.Now));
                DB.ConectData();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        })
            .WithName("addWeight")
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

/*
    Sesion tokens plan:
        Klient lagrer Token i local storage
        Server lagrer Token i RAM

    Log in sekvens
        Klient sender inn bruker navn og passord til server
        Server sjekker bruker navn og passord opp mot DB
        Hvis brukernavn eller passord er feil
            Send "Feil passord" til klient
        Hvis alt stemmer lagres en token i server ram
        Server sender TOKEN til klient
        Klient sender TOKEN til server ved alle sequests til server
 */