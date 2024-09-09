using MinimalAPI.Dominio.Entidades;
using MinimalAPI.DTOs;

namespace MinimalAPI.Dominio.Interfaces;

public interface IAdministradorServico
{
    Administradores? Login(LoginDTO loginDTO);
    Administradores Incluir(Administradores administradores);
    Administradores? BuscarPorId(int id);
    Administradores Todos(int? pagina);
}