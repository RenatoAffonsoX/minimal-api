using MinimalAPI.Dominio.Entidades;
using MinimalAPI.DTOs;

namespace MinimalAPI.Dominio.Interfaces;

public interface IVeiculosServico
{
    List<Veiculos> Todos(int pagina = 1, string? nome = null, string? marca = null);

    Veiculos? BuscaPorID(int id);
    void Incluir(Veiculos veiculos);
    void Atualizar(Veiculos veiculos);
    void Apagar(Veiculos veiculos);
}