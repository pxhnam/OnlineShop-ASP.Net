using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OnlineShop.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<InforCustomer> InforCustomers { get; set; }
        public virtual DbSet<InforProduct> InforProducts { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InforCustomer>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.InforCustomer)
                .HasForeignKey(e => e.IDCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InforProduct>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.InforProduct)
                .HasForeignKey(e => e.IDProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.IDBill)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .HasForeignKey(e => e.IDPayment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Cost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Favorites)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.IDProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.InforProducts)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.IDProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.InforProducts)
                .WithRequired(e => e.Size)
                .HasForeignKey(e => e.IDSize)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Favorites)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.InforCustomers)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IDUser);
        }
    }
}
