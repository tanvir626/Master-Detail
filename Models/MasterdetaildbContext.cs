using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Master_Detail.Models;

public partial class MasterdetaildbContext : DbContext
{
    public MasterdetaildbContext()
    {
    }

    public MasterdetaildbContext(DbContextOptions<MasterdetaildbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Ordertable> Ordertables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=masterdetaildb;Integrated Security=True;Encrypt=False;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF557FAB32");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.orderdetailid).HasName("PK__orderdet__7F6004ADF90A6930");

            entity.ToTable("orderdetail");

            entity.Property(e => e.orderdetailid).HasColumnName("orderdetailid");
            entity.Property(e => e.Amount)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnName("amount");
            entity.Property(e => e.orderid).HasColumnName("orderid");
            entity.Property(e => e.ProductName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__orderdeta__order__4BAC3F29");
        });

        modelBuilder.Entity<Ordertable>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("PK__ordertab__080E377545643B18");

            entity.ToTable("ordertable");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
