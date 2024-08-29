using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.DTOs;
using MinimalAPI.Infraestrutura.Db;

namespace MinimalAPI.Dominio.Servicos;

public class VeiculosServico : IVeiculosServico
{
    private readonly DbContexto _contexto;

    public VeiculosServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public void Apagar(Veiculos veiculos)
    {
        _contexto.veiculo.Remove(veiculos);
        _contexto.SaveChanges();
    }

    public void Atualizar(Veiculos veiculos)
    {
        _contexto.veiculo.Update(veiculos);
        _contexto.SaveChanges();
    }

    public void Incluir(Veiculos veiculos)
    {
        _contexto.veiculo.Add(veiculos);
        _contexto.SaveChanges();
    }

    public Veiculos? BuscaPorID(int id)
    {
        return _contexto.veiculo.Where(v => v.Id ==id).FirstOrDefault();
    }

    public List<Veiculos> Todos(int pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.veiculo.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => v.Nome.ToLower().Contains(nome));
        }
        
        int itensPorPagina = 10;

        query = query.Skip((pagina -1) * itensPorPagina).Take(itensPorPagina);
        return query.ToList();
    }
}