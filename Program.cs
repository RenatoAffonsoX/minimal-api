using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContexto>(Options => 
{
    Options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")        
    );
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) => {
    if (loginDTO.Email == "email@email.com" && loginDTO.Senha == "123")
        return Results.Ok("Login efetuado");
    else
        return Results.Unauthorized();
});
app.Run();