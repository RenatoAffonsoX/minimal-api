using MinimalAPI.Dominio.Enums;

namespace MinimalAPI.Dominio.ModelViews;

public record AdministradorModelView
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public Perfil Perfil { get; set; }
}