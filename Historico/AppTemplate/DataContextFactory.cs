using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HistoricoApp
{
    // Usada apenas pelo "dotnet ef" / Add-Migration para criar o DataContext
    // sem precisar executar o Program.cs inteiro.
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlite("Data Source=Historico.db");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
