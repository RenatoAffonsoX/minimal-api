using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Dominio.ModelViews;
using MinimalAPI.Dominio.Entidades;

#region BUILDER
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(Options => 
{
    Options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")        
    );
});

var app = builder.Build();

#endregion

#region HOME e LOGIN 

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if (administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login efetuado");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");

#endregion

#region VEICULOS

ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao();

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("Verificar o campo Nome");
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("Verificar o campo marca");
    if (veiculoDTO.Nome.Length < 4)
        validacao.Mensagens.Add("Verificar o campo");
    
    return validacao;
}
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) => {
    
    var validacao = validaDTO(veiculoDTO);
    
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
    
    var veiculo = new Veiculos{
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculosServico.Incluir(veiculo);
    
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) => {
    
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) => {
    
    var veiculo = veiculosServico.BuscaPorID(id);

    if (veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) => {
    
    var veiculo = veiculosServico.BuscaPorID(id);
    var validacao = validaDTO(veiculoDTO);

    if (veiculo == null) return Results.NotFound();
    
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculosServico.Atualizar(veiculo);
    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) => {
    
    var veiculo = veiculosServico.BuscaPorID(id);

    if (veiculo == null) return Results.NotFound();

    return Results.NoContent();

}).WithTags("Veiculos");

#endregion

app.UseSwagger();
app.UseSwaggerUI();


app.Run();