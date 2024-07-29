using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context
{


	public class BaseContextProvider : DbContext
	{

		public BaseContextProvider(DbContextOptions<BaseContextProvider> options) : base(options)
		{

		}
		public DbSet<Pessoa> Pessoas { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }

		public DbSet<Tasks> Tasks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configurações adicionais do modelBuilder

			modelBuilder.Entity<UserRole>()
				.HasKey(ur => new { ur.UserId, ur.RoleId });

			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.User)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.UserId);

			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.Role)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.RoleId);

			modelBuilder.Entity<Pessoa>(entity => {
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Nome)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.Nome_Complemento)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.Rg)
					.HasMaxLength(20);

				entity.Property(e => e.Cpf)
					.HasMaxLength(11);

				entity.Property(e => e.Email)
					.HasMaxLength(255);

				entity.Property(e => e.Telefone)
					.HasMaxLength(20);

				entity.Property(e => e.Data_Alteracao)
					.HasColumnType("datetime");

				entity.Property(e => e.Data_Cadastro)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<Tasks>(entity =>
			{
				entity.HasKey(t => t.Id);

				entity.Property(t => t.Title)
					.IsRequired()
					.HasMaxLength(256);

				entity.Property(t => t.Status)
					.IsRequired()
					.HasMaxLength(50);

				entity.HasOne(t => t.User);
				
			});

		}
	}
}

