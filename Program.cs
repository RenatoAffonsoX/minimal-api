using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Dominio.ModelViews;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(Options => 
{
    Options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")        
    );
});

var app = builder.Build();

app.MapGet("/", () => Results.Json(new Home()));

app.MapPost("/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if (administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login efetuado");
    else
        return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();


app.Run();