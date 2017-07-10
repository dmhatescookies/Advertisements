using System.Data.Entity;
using AdvertisementsMVC;

namespace AdvertisementsServer
{
    public class AdvertisementsContext : DbContext
    {
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Person> Persons { get; set; }

        public AdvertisementsContext() : base("DefaultConnection")
        {   
            Database.CreateIfNotExists();
        }

    }
}
