using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.Entidades;
using System;
using Microsoft.AspNetCore.Components; 

namespace MinimalAPI.Infraestrutura.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuracaoAppSettings;

    public DbContexto(IConfiguration configuracaoAppSettings)
    {
        configuracaoAppSettings = _configuracaoAppSettings;
    }

    public DbSet<Administradores> Administrador {get; set;} = default!;
    public DbSet<Veiculos> veiculo {get; set;} = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administradores>().HasData(
            new Administradores
            {
                Id = 1,
                Email = "adm@teste.com",
                Senha = "1234",
                Perfil = "ADM"
            }
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var stringConexao = _configuracaoAppSettings.GetConnectionString("SqlServer")?.ToString();

        if (!optionsBuilder.IsConfigured || !string.IsNullOrEmpty(stringConexao))
            optionsBuilder.UseSqlServer(stringConexao);
    }
    // SEED , FUNÇÃO PARA CADASTRAR O ADM INICIAL
}