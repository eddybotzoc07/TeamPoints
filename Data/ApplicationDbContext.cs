using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamPoints.Models;

namespace TeamPoints.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Logro> Logros { get; set; } = null!;
        public DbSet<Premio> Premios { get; set; } = null!;
        public DbSet<TransaccionPuntos> Transacciones { get; set; } = null!;
    }
}
