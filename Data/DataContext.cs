using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dot_battle.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasOne<User>(c => c.User)
                .WithMany(c => c.Characters)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharacterWeapon>()
                .HasOne<Character>(w => w.Character)
                .WithOne(c => c.CharacterWeapon)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<CharacterWeapon> Weapons => Set<CharacterWeapon>();
        public DbSet<Character> Characters => Set<Character>();
    }
}