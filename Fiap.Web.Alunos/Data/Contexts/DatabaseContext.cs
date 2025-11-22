using Fiap.Web.Alunos.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Web.Alunos.Data.Contexts;

public class DatabaseContext : DbContext
{
    public virtual DbSet<RepresentanteModel> Representantes { get; set; }
    public virtual DbSet<ClienteModel> Clientes { get; set; }
    public virtual DbSet<PedidoModel> Pedidos { get; set; }
    public virtual DbSet<ProdutoModel> Produtos { get; set; }
    public virtual DbSet<FornecedorModel> Fornecedores { get; set; }
    public virtual DbSet<LojaModel> Lojas { get; set; }
    public virtual DbSet<PedidoProdutoModel> PedidoProdutos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RepresentanteModel>(entity =>
        {
            entity.ToTable("Representantes");
            entity.HasKey(e => e.RepresentanteId);
            entity.Property(e => e.NomeRepresentante).IsRequired();
            entity.HasIndex(e => e.Cpf).IsUnique();
        });

        modelBuilder.Entity<ClienteModel>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.ClienteId);
            entity.Property(e => e.Nome).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.DataNascimento).HasColumnType("date");
            entity.Property(e => e.Observacao).HasMaxLength(500);
            entity.HasOne(e => e.Representante)
                .WithMany()
                .HasForeignKey(e => e.RepresentanteId)
                .IsRequired();
        });

        modelBuilder.Entity<PedidoModel>(entity =>
        {
            entity.ToTable("Pedidos");
            entity.HasKey(p => p.PedidoId);
            entity.Property(p => p.DataPedido).HasColumnType("date");
            entity.HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId);
            entity.HasMany(p => p.PedidoProdutos)
                .WithOne(pp => pp.Pedido)
                .HasForeignKey(pp => pp.PedidoId);

        });
        modelBuilder.Entity<ProdutoModel>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(p => p.ProdutoId);
            entity.Property(p => p.Nome).IsRequired();
            entity.Property(p => p.Descricao);
            entity.Property(p => p.Preco).HasColumnType("decimal(18,2)");
            entity.HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId);
        });
        modelBuilder.Entity<FornecedorModel>(entity =>
        {
            entity.ToTable("Fornecedores");
            entity.HasKey(f => f.FornecedorId);
            entity.Property(f => f.Nome).IsRequired();
        });
        modelBuilder.Entity<LojaModel>(entity =>
        {
            entity.ToTable("Lojas");
            entity.HasKey(l => l.LojaId);
            entity.Property(l => l.Nome).IsRequired();
            entity.Property(l => l.Endereco);
            entity.HasMany(l => l.Pedidos)
                .WithOne(p => p.Loja)
                .HasForeignKey(p => p.LojaId);
        });

    modelBuilder.Entity<PedidoProdutoModel>(entity =>
    {
        entity.HasKey(pp => new { pp.PedidoId, pp.ProdutoId });
        entity.HasOne(pp => pp.Pedido)
            .WithMany(p => p.PedidoProdutos)
            .HasForeignKey(pp => pp.PedidoId);
        entity.HasOne(pp => pp.Produto)
            .WithMany(p => p.PedidoProdutos)
            .HasForeignKey(pp => pp.ProdutoId);
    });
    }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    protected DatabaseContext()
    {
    }
    
    
}