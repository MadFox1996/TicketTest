using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskApi.DAL.Entities;

namespace TaskApi.DAL.EF
{
    public class TicketDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFile> TicketFiles { get; set; }

        public TicketDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Ticket>().HasData(
               new Ticket { Id = 1, CreatedDate = DateTime.Now, Name = "Ticket1", Stage = Enum.TicketStage.New },
               new Ticket { Id = 2, CreatedDate = DateTime.Now, Name = "Ticket2", Stage = Enum.TicketStage.New },
               new Ticket { Id = 3, CreatedDate = DateTime.Now, Name = "Ticket3", Stage = Enum.TicketStage.New }
           );

            base.OnModelCreating(modelBuilder);
        }
    }
}
