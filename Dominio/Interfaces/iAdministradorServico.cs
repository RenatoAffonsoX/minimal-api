using MinimalAPI.Dominio.Entidades;
using MinimalAPI.DTOs;

namespace MinimalAPI.Dominio.Interfaces;

public interface iAdministradorServico
{
    Administradores? Login(LoginDTO loginDTO);
}