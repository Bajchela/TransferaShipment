using Microsoft.EntityFrameworkCore;
using Shipments.Domain.Models.Auth;
using Shipments.Domain.Models;

namespace Shipments.Infrastructure.Persistance
{
    public class ShipmentsDbContext : DbContext
    {
        public ShipmentsDbContext(DbContextOptions<ShipmentsDbContext> options) : base(options) { }
        public DbSet<Shipment> Shipments => Set<Shipment>();
        public DbSet<ShipmentDocument> ShipmentDocuments => Set<ShipmentDocument>();

        #region Authentication and Authorization
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
