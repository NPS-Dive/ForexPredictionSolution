using ForexPrediction.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

public class ForexDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<HistoricalData> HistoricalData { get; set; }
    public DbSet<Prediction> Predictions { get; set; }

    public ForexDbContext () : base("DefaultConnection") { }

    protected override void OnModelCreating ( DbModelBuilder modelBuilder )
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<HistoricalData>().HasIndex(h => new { h.Date, h.Pair });
    }
}