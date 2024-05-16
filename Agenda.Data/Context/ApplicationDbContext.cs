using Agenda.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<Tarefa> Tarefas { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("tarefas");

            entity.HasIndex(e => e.UsuarioId)
                .HasName("FK_Agendas_usuarios");

            entity.Property(e => e.Id).HasColumnType("varchar(50)");

            entity.Property(e => e.Data).HasColumnType("date");

            entity.Property(e => e.DataConclusao).HasColumnType("date");

            entity.Property(e => e.PrevisaoTempo).HasColumnType("varchar(20)");

            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasColumnType("varchar(250)");

            entity.Property(e => e.UsuarioId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendas_usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnType("varchar(50)");

            entity.Property(e => e.Email).HasColumnType("varchar(100)");

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            entity.Property(e => e.Senha)
                .IsRequired()
                .HasColumnType("varchar(10)");
        });
    }
}
