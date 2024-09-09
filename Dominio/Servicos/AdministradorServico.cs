using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;

namespace MinimalAPI.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;

    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administradores? BuscarPorId(int id)
    {
        return _contexto.Administrador.Where(v => v.Id == id).FirstOrDefault();
    }

    public Administradores Incluir(Administradores administradores)
    {
        _contexto.Administrador.Add(administradores);
        _contexto.SaveChanges();

        return administradores;
    }

    public Administradores? Login(LoginDTO loginDTO)
    {
         return _contexto.Administrador.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
    }

    public List<Administradores> Todos(int? pagina)
    {
        var query = _contexto.Administrador.AsQueryable();
        int itensPorPagina = 10;

        if (pagina != null)
            query = query.Skip(((int) pagina - 1) * itensPorPagina).Take(itensPorPagina);
        
        return query.ToList();
    }

    Administradores IAdministradorServico.Todos(int? pagina)
    {
        throw new NotImplementedException();
    }
}