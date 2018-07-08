using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project1.Data
{
    public partial class Project1DBContext : DbContext
    {
        public Project1DBContext()
        {
        }

        public Project1DBContext(DbContextOptions<Project1DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizza> Pizza { get; set; }
        public virtual DbSet<PizzaOrders> PizzaOrders { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:toledo-del-valle-1806.database.windows.net,1433;Initial Catalog=Project1DB;Persist Security Info=False;User ID=rolov3;Password=Tequilas27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations>(entity =>
            {
                entity.ToTable("Locations", "Project1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("Orders", "Project1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Orders__Location__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserID__4D94879B");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("Pizza", "Project1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Pizza)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Pizza__OrderID__52593CB8");
            });

            modelBuilder.Entity<PizzaOrders>(entity =>
            {
                entity.ToTable("PizzaOrders", "Project1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PizzaOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__PizzaOrde__Order__5629CD9C");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.PizzaOrders)
                    .HasForeignKey(d => d.PizzaId)
                    .HasConstraintName("FK__PizzaOrde__Pizza__571DF1D5");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users", "Project1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__LocationI__59FA5E80");
            });
        }
    }
}
