using Microsoft.EntityFrameworkCore;
using Server.Api.Models.Database;

namespace Server.Api.Infrastructure
{
    /// <summary>
    /// Dependencies and dataseeding for dummy
    /// </summary>
    public class OnModelCreatingDummy
    {
        public OnModelCreatingDummy(ModelBuilder modelBuilder)
        {
            Creating(modelBuilder);
            Seeding(modelBuilder);
        }

        /// <summary>
        /// Dependencies
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void Creating(ModelBuilder modelBuilder)
        {
            const string _prefix = "pre_";

            modelBuilder.Entity<Db_Dummy>()
                .ToTable(_prefix + "Dummy")
                .HasKey(x => x.Id);
            modelBuilder.Entity<Db_Dummy>()
                .Property(q => q.Name).IsRequired();

        }


        /// <summary>
        /// Dataseeding
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void Seeding(ModelBuilder modelBuilder)
        {
            // Question
            modelBuilder.Entity<Db_Dummy>()
                .HasData(
                new Db_Dummy { Id = 1, Name = "Name 1" },
                new Db_Dummy { Id = 2, Name = "Name 2" },
                new Db_Dummy { Id = 3, Name = "Name 3" }
                );
        }   
        
    }
}
