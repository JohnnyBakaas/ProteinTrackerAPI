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
            int userId = SessionToken.TokenStringToId(tokenFromClient);

            if (userId == -1)
            {
                return new JsonResult("Ugyldig");
            }

            var user = DB.Users.FirstOrDefault(u => u.Id == userId);
            return new JsonResult(user);
        })
            .WithName("GetUser")
            .WithOpenApi();


        app.MapGet("/login", (string userName, string password) =>
        {
            var rightUser = DB.Users.FirstOrDefault(user => user.ValidateUsernameAndPasword(userName, password));

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
                DB.AddWeight(new Weight(int.Parse(weight), coment, SessionToken.TokenStringToId(tokenFromClient), DateTime.Now));
                DB.ConectData();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        })
            .WithName("addWeight")
            .WithOpenApi();


        app.MapPost("/updateUser", (string token, User user) =>
        {
            try
            {
                if (user.Id == SessionToken.TokenStringToId(token))
                {
                    DB.UpdateUser(user);
                    DB.ConectData();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); };
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

