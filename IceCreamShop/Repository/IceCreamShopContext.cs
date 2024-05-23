using System;
using System.Collections.Generic;
using IceCreamShop.Models;
using Microsoft.EntityFrameworkCore;

namespace IceCreamShop.Repository;

public partial class IceCreamShopContext : DbContext
{
    public IceCreamShopContext()
    {
    }

    public IceCreamShopContext(DbContextOptions<IceCreamShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<IcecreamModel> IcecreamModels { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=IceCreamShop;Username=postgres;Password=sabinamini04");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("employee_id");
            entity.Property(e => e.ContactDetails)
                .HasMaxLength(100)
                .HasColumnName("contact_details");
            entity.Property(e => e.EName)
                .HasMaxLength(50)
                .HasColumnName("e_name");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.TimeEmployees)
                .HasMaxLength(100)
                .HasColumnName("time_employees");

            entity.HasOne(d => d.Store).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("employees_store_id_fkey");
        });

        modelBuilder.Entity<IcecreamModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("icecream_models_pkey");

            entity.ToTable("icecream_models");

            entity.Property(e => e.ModelId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("model_id");
            entity.Property(e => e.Barcode).HasColumnName("barcode");
            entity.Property(e => e.DateOfManufacture).HasColumnName("date_of_manufacture");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.ModelName)
                .HasMaxLength(100)
                .HasColumnName("model_name");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");

            entity.HasOne(d => d.Product).WithMany(p => p.IcecreamModels)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("icecream_models_product_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.IcecreamModels)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("icecream_models_sale_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("product_id");
            entity.Property(e => e.ProductComposition)
                .HasMaxLength(300)
                .HasColumnName("product_composition");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(300)
                .HasColumnName("product_description");
            entity.Property(e => e.ProductManufacturer)
                .HasMaxLength(100)
                .HasColumnName("product_manufacturer");
            entity.Property(e => e.ProductPrice)
                .HasPrecision(10, 2)
                .HasColumnName("product_price");
            entity.Property(e => e.ProductType)
                .HasMaxLength(100)
                .HasColumnName("product_type");
            entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("sales_pkey");

            entity.ToTable("sales");

            entity.Property(e => e.SaleId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sale_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.SaleDate).HasColumnName("sale_date");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("sales_employee_id_fkey");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("store_pkey");

            entity.ToTable("store");

            entity.Property(e => e.StoreId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("store_id");
            entity.Property(e => e.StoreAddress)
                .HasMaxLength(200)
                .HasColumnName("store_address");
            entity.Property(e => e.StorePhone)
                .HasMaxLength(100)
                .HasColumnName("store_phone");
            entity.Property(e => e.StoreTimetable)
                .HasMaxLength(100)
                .HasColumnName("store_timetable");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
