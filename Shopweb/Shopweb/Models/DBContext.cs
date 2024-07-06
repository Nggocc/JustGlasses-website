using Shopweb.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Shopweb.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Cart_Details> Cart_Details { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order_Details> Order_Details { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.loginName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.Cart_Details)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.email)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Shipments)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.WishLists)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Details)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.SKU)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.description)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.image)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.gallery)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Cart_Details)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Details)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.WishLists)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Shipment)
                .WillCascadeOnDelete(false);
        }
    }
}
