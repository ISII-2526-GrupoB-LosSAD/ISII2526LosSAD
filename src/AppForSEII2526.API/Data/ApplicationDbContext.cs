using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Device> Devices { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Receiptitem> Receiptitems { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Purchase> Purchase { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    public DbSet<Scale> Scales { get; set; }
    public DbSet<ReviewItem> ReviewItems { get; set; }  
}
