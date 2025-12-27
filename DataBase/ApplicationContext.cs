using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Testovoe.Model;

namespace Testovoe.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Parameters> parameters { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=testBD;Username=postgres;Password=3894");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameters>(entity =>
            {
                entity.ToTable("Parameters");
                entity.Property(e => e.ParamId)
                    .HasColumnName("paramid");

                entity.Property(e => e.Speed_2_1000)
                    .HasColumnName("speed_2_1000")
                    .HasColumnType("real");

                entity.Property(e => e.Speed_10_1000)
                    .HasColumnName("speed_10_1000")
                    .HasColumnType("real");

                entity.Property(e => e.Accel_2_1000)
                    .HasColumnName("accel_2_1000")
                    .HasColumnType("real");

                entity.Property(e => e.Accel_10_1000)
                    .HasColumnName("accel_10_1000")
                    .HasColumnType("real");

                entity.Property(e => e.Movement_2_1000)
                    .HasColumnName("movement_2_1000")
                    .HasColumnType("real");

                entity.Property(e => e.Movement_10_1000)
                    .HasColumnName("movement_10_1000")
                    .HasColumnType("real");
            });
        }
    }
}
