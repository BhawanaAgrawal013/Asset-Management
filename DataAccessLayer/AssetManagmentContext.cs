using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DataAccessLayer
{
    public partial class AssetManagmentContext : DbContext
    {
        public AssetManagmentContext(DbContextOptions<AssetManagmentContext> options) : base(options) 
        {
            //Database.SetConnectionString("Data Source=.;Initial Catalog=AssetManagement;Trusted_Connection=True;TrustServerCertificate=True");
        }

        public virtual DbSet<AssetDetail> AssetDetails { get; set; }
        public virtual DbSet<AssetCategory> AssetCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

