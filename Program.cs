using MinimalAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) => {
    if (loginDTO.Email == "email@email.com" && loginDTO.Senha == "123")
        return Results.Ok("Login efetuado");
    else
        return Results.Unauthorized();
});
app.Run();