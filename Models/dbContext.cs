
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace PC02.Models
{
    public class dbContext : IdentityDbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Menu> Menus { get; set; }

        public dbContext(DbContextOptions<dbContext> o) : base(o) { }
    }
}