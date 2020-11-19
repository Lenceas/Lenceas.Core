using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Lenceas.Core.Model
{
    public class MySqlContext : DbContext
    {
        public MySqlContext()
        {
        }

        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }

        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var mysql = DifDBConnOfSecurity(@"D:\my-file\mysql_Conn.txt", @"c:\my-file\mysql_Conn.txt");
            optionsBuilder.UseMySql(mysql);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("Test");
            });
        }

        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (System.Exception) { }
            }
            return conn[^1];
        }
    }
}
