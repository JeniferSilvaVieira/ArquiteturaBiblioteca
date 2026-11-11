using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LivrosApp
{
    // Usada apenas pelo "dotnet ef" / Add-Migration para criar o DataContext
    // sem precisar executar o Program.cs inteiro.
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlite("Data Source=Livros.db");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
