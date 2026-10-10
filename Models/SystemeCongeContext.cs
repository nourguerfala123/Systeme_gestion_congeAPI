using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gestion_congeAPI.Models;

public partial class SystemeCongeContext : DbContext
{
    public SystemeCongeContext()
    {
    }

    public SystemeCongeContext(DbContextOptions<SystemeCongeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<Congee> Congees { get; set; }

    public virtual DbSet<Dept> Depts { get; set; }

    public virtual DbSet<Emp> Emps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=systemeConge;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compte>(entity =>
        {
            entity.HasKey(e => e.IdCompte).HasName("PK__Compte__C4BAD6C20247D4BF");

            entity.ToTable("Compte");

            entity.HasIndex(e => e.Email, "UQ__Compte__AB6E616479E91564").IsUnique();

            entity.HasIndex(e => e.IdUser, "UQ__Compte__D2D1463653F4BC27").IsUnique();

            entity.Property(e => e.IdCompte).HasColumnName("id_compte");
            entity.Property(e => e.DateCreation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");
            entity.Property(e => e.DerniereConnexion)
                .HasColumnType("datetime")
                .HasColumnName("derniereConnexion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.MotDePasse)
                .HasMaxLength(255)
                .HasColumnName("motDePasse");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasDefaultValueSql("('actif')")
                .HasColumnName("statut");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Compte)
                .HasForeignKey<Compte>(d => d.IdUser)
                .HasConstraintName("FK_Compte_Utilisateur");
        });

        modelBuilder.Entity<Congee>(entity =>
        {
            entity.HasKey(e => e.IdConge).HasName("PK__congee__75469DAB978EFB1A");

            entity.ToTable("congee");

            entity.Property(e => e.IdConge).HasColumnName("id_conge");
            entity.Property(e => e.DateDebut)
                .HasColumnType("date")
                .HasColumnName("date_debut");
            entity.Property(e => e.DateFin)
                .HasColumnType("date")
                .HasColumnName("date_fin");
            entity.Property(e => e.IdEmploye).HasColumnName("id_employe");
            entity.Property(e => e.Libelle)
                .HasMaxLength(50)
                .HasColumnName("libelle");
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasColumnName("statut");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.Congees)
                .HasForeignKey(d => d.IdEmploye)
                .HasConstraintName("FK__congee__id_emplo__6754599E");
        });

        modelBuilder.Entity<Dept>(entity =>
        {
            entity.HasKey(e => e.IdDept).HasName("PK__dept__900B8FB7E25BF299");

            entity.ToTable("dept");

            entity.Property(e => e.IdDept).HasColumnName("id_dept");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.NomDept)
                .HasMaxLength(50)
                .HasColumnName("nom_dept");
        });

        modelBuilder.Entity<Emp>(entity =>
        {
            entity.HasKey(e => e.IdEmploye).HasName("PK__emp__420CFD16F925E236");

            entity.ToTable("emp");

            entity.Property(e => e.IdEmploye)
                .ValueGeneratedNever()
                .HasColumnName("id_employe");
            entity.Property(e => e.DateEmbauche)
                .HasColumnType("date")
                .HasColumnName("dateEmbauche");
            entity.Property(e => e.IdDept).HasColumnName("id_dept");
            entity.Property(e => e.Matricule)
                .HasMaxLength(20)
                .HasColumnName("matricule");
            entity.Property(e => e.Poste)
                .HasMaxLength(50)
                .HasColumnName("poste");
            entity.Property(e => e.Salaire)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salaire");

            entity.HasOne(d => d.IdDeptNavigation).WithMany(p => p.Emps)
                .HasForeignKey(d => d.IdDept)
                .HasConstraintName("FK__emp__id_dept__6383C8BA");

            entity.HasOne(d => d.IdEmployeNavigation).WithOne(p => p.Emp)
                .HasForeignKey<Emp>(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__emp__id_employe__628FA481");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__user__D2D14637EE4360FF");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ__user__AB6E61642FADDC4E").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .HasColumnName("prenom");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
