using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Testovoe.Model;
using Testovoe.Services;

namespace Testovoe.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Parameters> parameters { get; set; }

        public ApplicationContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch { MessageBox.Show("Ошибка подключения к бд, перезапустите приложение и проверьте параметры подключения!"); }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionParamModel savedConSettings = FileService.GetConnectionSetting();
            optionsBuilder.UseNpgsql($"Host={savedConSettings.Host};Port={savedConSettings.Port};" +
                $"Database={savedConSettings.DataBase};Username={savedConSettings.Username};Password={savedConSettings.Password}");
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
