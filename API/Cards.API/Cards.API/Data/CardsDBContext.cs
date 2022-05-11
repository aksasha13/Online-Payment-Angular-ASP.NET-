using Cards.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Data
{
    public class CardsDBContext : DbContext
    {
        public CardsDBContext(DbContextOptions options) : base(options)
        {
            
        }

        //Dbset
        public DbSet<Card> Cards { get; set; }

    }
}
