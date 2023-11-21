using Microsoft.EntityFrameworkCore;
using System;
using TransportationArea.DBProviderService.Data;

namespace TransportationArea.DBProviderService
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();   
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=cornelius.db.elephantsql.com;Port=5432;Database=fuycmixn;Username=fuycmixn;Password=SeJmX-k3Jqj2MDhk3dvb4bY5PVlmvZBr");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(p => p.LoadingDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            modelBuilder.Entity<OrderStatus>()
              .Property(p => p.Start)
              .HasConversion
              (
                  src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                  dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
              );
            modelBuilder.Entity<OrderStatus>()
             .Property(p => p.End)
             .HasConversion
             (
                 src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                 dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
             );
            modelBuilder.Entity<CarRoute>()
            .Property(p => p.Start)
            .HasConversion
            (
                src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
            );
           modelBuilder.Entity<CarRoute>()
          .Property(p => p.End)
          .HasConversion
          (
              src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
              dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
          );
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LoadingPoints> LoadingPoints { get; set; }
        public DbSet<Area> Areas { get; set; }      

        public DbSet<GridOfArea> GridsOfArea { get; set; }

        public DbSet<Car> Cars { get; set; }

        public  DbSet<CarRoute> CarRoutes { get; set; }

        public DbSet<OrderStatus> OrdersStatus { get; set; }
        public DbSet<OrderRoute> OrderRoutes { get; set; }
    }

}
