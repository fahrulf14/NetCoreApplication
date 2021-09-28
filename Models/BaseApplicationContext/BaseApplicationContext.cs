using Microsoft.EntityFrameworkCore;
using NUNA.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class BaseApplicationContext : DbContext
    {
        private readonly string _user;
        public BaseApplicationContext()
        {
        }

        public BaseApplicationContext(DbContextOptions<BaseApplicationContext> options, UserService userService)
            : base(options)
        {
            _user = userService.GetUser();
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoleMenus> AspNetRoleMenus { get; set; }
        public virtual DbSet<AspNetRolePermissions> AspNetRolePermissions { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserMenus> AspNetUserMenus { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserPermissions> AspNetUserPermissions { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ID_Provinsi> ID_Provinsi { get; set; }
        public virtual DbSet<ID_KabupatenKota> ID_KabupatenKota { get; set; }
        public virtual DbSet<ID_Kecamatan> ID_Kecamatan { get; set; }
        public virtual DbSet<ID_Kelurahan> ID_Kelurahan { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MS_Position> MS_Position { get; set; }
        public virtual DbSet<Officer> Officer { get; set; }
        public virtual DbSet<Personals> Personals { get; set; }
        public virtual DbSet<RF_IDType> RF_IDType { get; set; }
        public virtual DbSet<TR_Address> TR_Addresse { get; set; }
        public virtual DbSet<TR_IDNumber> TR_IDNumber { get; set; }
        public virtual DbSet<UserSetting> UserSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoleMenus>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.Menu });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRolePermissions>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.Permission });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .IsUnique();
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserMenus>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Menu });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserPermissions>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Permission });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail);

                entity.HasIndex(e => e.NormalizedUserName)
                    .IsUnique();
            });

            modelBuilder.Entity<ID_KabupatenKota>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.ID_Provinsi)
                      .WithMany(e => e.ID_KabupatenKota)
                      .HasForeignKey(e => e.ProvinsiId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ID_Kecamatan>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.ID_KabupatenKota)
                      .WithMany(e => e.ID_Kecamatan)
                      .HasForeignKey(e => e.KabupatenKotaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ID_Kelurahan>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.ID_Kecamatan)
                      .WithMany(e => e.ID_Kelurahan)
                      .HasForeignKey(e => e.KecamatanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<MS_Position>(entity =>
            {
                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<Officer>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.MS_Position)
                      .WithMany(e => e.Officer)
                      .HasForeignKey(e => e.PositionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Personals>(entity =>
            {
                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<RF_IDType>(entity =>
            {
                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<TR_Address>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.Personals)
                      .WithMany(e => e.TR_Address)
                      .HasForeignKey(e => e.PersonalId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ID_Provinsi)
                      .WithMany(e => e.TR_Address)
                      .HasForeignKey(e => e.ProvinsiId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ID_KabupatenKota)
                      .WithMany(e => e.TR_Address)
                      .HasForeignKey(e => e.KabupatenKotaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ID_Kecamatan)
                      .WithMany(e => e.TR_Address)
                      .HasForeignKey(e => e.KecamatanId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ID_Kelurahan)
                      .WithMany(e => e.TR_Address)
                      .HasForeignKey(e => e.KelurahanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TR_IDNumber>(entity =>
            {
                entity.Property(e => e.Id);
                entity.HasOne(e => e.RF_IDType)
                      .WithMany(e => e.TR_IDNumber)
                      .HasForeignKey(e => e.TypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is BaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreationTime = now;
                            entity.CreationUser = _user;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreationTime).IsModified = false;
                            Entry(entity).Property(x => x.CreationUser).IsModified = false;
                            entity.ModificationTime = now;
                            entity.ModificationUser = _user;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is BaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreationTime = now;
                            entity.CreationUser = _user;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreationTime).IsModified = false;
                            Entry(entity).Property(x => x.CreationUser).IsModified = false;
                            entity.ModificationTime = now;
                            entity.ModificationUser = _user;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
