using MinimalAPI.Dominio.Enums;

namespace MinimalAPI.DTOs;

public class AdministradorDTO
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public Perfil? Perfil { get; set; } = default!;
}