using Microsoft.EntityFrameworkCore;
using Shipments.Domain.Models;

namespace Shipments.WorkerService.Persistence.Worker;

    public class WorkerDbContext : DbContext
    {
        public WorkerDbContext(DbContextOptions<WorkerDbContext> options) : base(options) { }

        public DbSet<Shipment> Shipments => Set<Shipment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkerDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }

