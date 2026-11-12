
using Microsoft.EntityFrameworkCore;
using Refugio.API.Models;

namespace Refugio.API.Data;

public class RefugioContext : DbContext
{
    public RefugioContext(DbContextOptions<RefugioContext> options)
        : base(options)
    {
    }

    public DbSet<Ponto> Pontos => Set<Ponto>();
    public DbSet<Recurso> Recursos => Set<Recurso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ponto>(entity =>
        {
            entity.ToTable("NET_PONTO");

            entity.HasKey(p => p.IdPonto);

            entity.Property(p => p.IdPonto)
                .HasColumnName("ID_PONTO");

            entity.Property(p => p.CodigoPublico)
                .HasColumnName("CODIGO_PUBLICO")
                .HasMaxLength(7)
                .IsRequired();

            entity.HasIndex(p => p.CodigoPublico)
                .IsUnique();

            entity.Property(p => p.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(110)
                .IsRequired();

            entity.Property(p => p.Bairro)
                .HasColumnName("BAIRRO")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(p => p.HorarioTexto)
                .HasColumnName("HORARIO_TEXTO")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(p => p.Status)
                .HasColumnName("STATUS")
                .HasMaxLength(8)
                .HasDefaultValue("ATIVO")
                .IsRequired();
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.ToTable("NET_RECURSO");

            entity.HasKey(r => r.IdRecurso);

            entity.Property(r => r.IdRecurso)
                .HasColumnName("ID_RECURSO");

            entity.Property(r => r.IdPonto)
                .HasColumnName("ID_PONTO")
                .IsRequired();

            entity.Property(r => r.Tipo)
                .HasColumnName("TIPO")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(r => r.Disponivel)
                .HasColumnName("DISPONIVEL")
                 .HasConversion(
                 valor => valor ? "S" : "N",
                 valor => valor == "S"
                        )
                        .HasColumnType("CHAR(1)")
                        .IsRequired();


            entity.HasIndex(r => new { r.IdPonto, r.Tipo })
                .IsUnique();

            entity.HasOne(r => r.Ponto)
                .WithMany(p => p.Recursos)
                .HasForeignKey(r => r.IdPonto)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
