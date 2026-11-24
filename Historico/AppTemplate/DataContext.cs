using HistoricoApp.Servicos;
using Microsoft.EntityFrameworkCore;

namespace HistoricoApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<RegistroHistorico> RegistrosHistorico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistroHistorico>().HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
