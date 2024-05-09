using Microsoft.EntityFrameworkCore;
using OnePieceMinimal.Api.Models;

namespace OnePieceMinimal.Api.Context
{
    public class CharacterContext : DbContext
    {
        public CharacterContext(DbContextOptions<CharacterContext> options) 
            : base(options) { }

        public DbSet<Character> Characters { get; set; }
    }
}