using Fiap.Web.Alunos.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Web.Alunos.Data.Contexts;

public class DatabaseContext : DbContext
{
    public DbSet<RepresentanteModel> Representantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RepresentanteModel>(entity =>
        {
            entity.ToTable("Representantes");
            entity.HasKey(e => e.RepresentanteId);
            entity.Property(e => e.NomeRepresentante).IsRequired();
            entity.HasIndex(e => e.Cpf).IsUnique();
        });
    }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    protected DatabaseContext()
    {
    }
    
    
}