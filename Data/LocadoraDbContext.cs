using Locadora.Models;
using Locadora.Data;
using Locadora.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Data;

public class LocadoraDbContext : DbContext
{
    public LocadoraDbContext(DbContextOptions<LocadoraDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações adicionais, como chaves primárias e relacionamentos, podem ser definidas aqui
    }

}
