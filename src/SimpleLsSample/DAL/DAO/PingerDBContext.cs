using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace SimpleLsSample.DAL.DAO
{
    public class PingerDBContext : DbContext
    {
        public DbSet<Ping> Pings { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<ErrorMessages> FriendlyMessages { get; set; }

        public string DbPath { get; }

        public PingerDBContext()
        {
            DbPath = "pingig.db";

        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableDetailedErrors(true);
            options.UseSqlite($"Data Source={DbPath}");
        }

    }
    public class Ping 
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public DateTime DateTime { get; set; } 
    }
    public class Error
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class ErrorMessages
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}
