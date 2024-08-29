using MinimalAPI.Dominio.Entidades;
using MinimalAPI.DTOs;

namespace MinimalAPI.Dominio.Interfaces;

public interface IAdministradorServico
{
    Administradores? Login(LoginDTO loginDTO);
}