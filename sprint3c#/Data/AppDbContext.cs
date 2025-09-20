// Arquivo: Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // A linha abaixo configura o SQLite para criar e usar um arquivo 'banco_apostas.db'
        optionsBuilder.UseSqlite("Data Source=banco_apostas.db");
    }
}