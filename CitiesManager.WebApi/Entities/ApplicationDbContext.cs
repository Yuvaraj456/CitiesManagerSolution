using CitiesManager.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebApi.Entities
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions dbContext) : base(dbContext)
        {
            
        }

        public ApplicationDbContext()
        {
            
        }

        public virtual DbSet<Cities> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cities>().HasData(new Cities()
            {
                CityId = Guid.Parse("F2BF0F42-53D3-4D82-BE17-D4AFFC947CD8"),
                CityName = "London"

            }) ;
            modelBuilder.Entity<Cities>().HasData(new Cities()
            {
                CityId = Guid.Parse("7E7ADF8C-373C-4B09-9BFA-702A2F7B5364"),
                CityName = "New York"

            });
            


        }
    }
}
