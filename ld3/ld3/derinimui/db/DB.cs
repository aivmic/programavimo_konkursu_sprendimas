namespace Org.Ktu.T120B197.DBs;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.Ktu.T120B197.DBs.Entities;


public partial class DB : DbContext
{
    /// <summary>
    /// Logger to use. Can be null.
    /// </summary>
    public static Action<string> Logger { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public DB() : base()
    {
    }
    public DbSet<Author> Author { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Example> Example { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Comment> Comment { get; set; }

    /// <summary>
    /// Is invoked to configure the context.
    /// </summary>
    /// <param name="optionsBuilder">Options builder to use.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //set database connection string
            var dbConnStr = "Server=localhost;User=root;Password=root;Database=t120b197";
            optionsBuilder.UseMySql(dbConnStr, ServerVersion.AutoDetect(dbConnStr));

            //enable SQL logging for development
            if (Logger != null)
                optionsBuilder.LogTo(Logger, LogLevel.Information);
        }
    }
}
