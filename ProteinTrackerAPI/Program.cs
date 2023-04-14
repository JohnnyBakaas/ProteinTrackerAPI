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

        DB.UpdateUser(new User("john_smith", 1001, "password123", "Cutt", "male"));

        DB.DataDump();



        //FirebaseApp.Create();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

        app.MapGet("/users", () => new JsonResult(DB.Users))
             .WithName("GetUsers")
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
