using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Dominio.ModelViews;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Enums;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

#region BUILDER
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key))
    return;

builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters{
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

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

#endregion

#region Administradores
 
app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if (administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login efetuado");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) => {
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("Email não pode ser Vazio");
    if (string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("Senha não pode ser Vazio");
    if (administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil não pode ser Vazio");

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest();

    var administrador = new Administradores
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
    };
    
    administradorServico.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}", administrador);
}).WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) => {
    var adms = new List<AdministradorModelView>();
    var administrador = administradorServico.Todos(pagina);

    foreach (var adm in administrador)
    {
        adms.Add(new AdministradorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }
    return Results.Ok(administradorServico.Todos(pagina));
}).WithTags("Administradores");

app.MapPost("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) => {
    var administrador = administradorServico.BuscarPorId(id);
    if (administrador == null) return Results.NotFound();
    return Results.Ok(administrador);
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