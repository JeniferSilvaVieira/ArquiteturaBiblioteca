using LeitoresApp.Servicos;
using Microsoft.EntityFrameworkCore;

namespace LeitoresApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Leitor> Leitores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leitor>().HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
