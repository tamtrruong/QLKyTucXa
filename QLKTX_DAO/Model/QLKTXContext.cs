using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLKTX_DAO.Model.Entities;

namespace QLKTX_DAO.Model;

public partial class QLKTXContext : DbContext
{
    public QLKTXContext()
    {
    }

    public QLKTXContext(DbContextOptions<QLKTXContext> options)
        : base(options)
    {
    }

    public virtual DbSet<bang_gium> bang_gia { get; set; }

    public virtual DbSet<dien_nuoc> dien_nuocs { get; set; }

    public virtual DbSet<hoa_don> hoa_dons { get; set; }

    public virtual DbSet<hop_dong> hop_dongs { get; set; }

    public virtual DbSet<phong> phongs { get; set; }

    public virtual DbSet<sinh_vien> sinh_viens { get; set; }

    public virtual DbSet<tai_khoan> tai_khoans { get; set; }

    public virtual DbSet<toa_nha> toa_nhas { get; set; }

    public virtual DbSet<vi_pham> vi_phams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=dpg-d5n566re5dus73eu5q40-a.singapore-postgres.render.com;Port=5432;Database=qlktx_xgzm;Username=qlktx_admin;Password=JFaE2NKO7LFJSw43c7tgTBbtHyYQOIRX;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<bang_gium>(entity =>
        {
            entity.HasKey(e => e.ma_bang_gia).HasName("bang_gia_pkey");

            entity.Property(e => e.dang_su_dung).HasDefaultValue(true);
            entity.Property(e => e.don_gia_dien).HasPrecision(18);
            entity.Property(e => e.don_gia_nuoc).HasPrecision(18);
            entity.Property(e => e.don_gia_phong).HasPrecision(18);
            entity.Property(e => e.loai_phong).HasMaxLength(50);
            entity.Property(e => e.ngay_ap_dung)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.phi_rac).HasPrecision(18);
        });

        modelBuilder.Entity<dien_nuoc>(entity =>
        {
            entity.HasKey(e => e.id).HasName("dien_nuoc_pkey");

            entity.ToTable("dien_nuoc");

            entity.HasIndex(e => new { e.ma_phong, e.ky_ghi_nhan }, "idx_dien_nuoc_ma_phong_ky");

            entity.HasIndex(e => new { e.ma_phong, e.ky_ghi_nhan }, "uq_dien_nuoc").IsUnique();

            entity.Property(e => e.dien_cu).HasDefaultValue(0);
            entity.Property(e => e.dien_moi).HasDefaultValue(0);
            entity.Property(e => e.ma_phong).HasMaxLength(10);
            entity.Property(e => e.ngay_chot)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.nuoc_cu).HasDefaultValue(0);
            entity.Property(e => e.nuoc_moi).HasDefaultValue(0);

            entity.HasOne(d => d.ma_phongNavigation).WithMany(p => p.dien_nuocs)
                .HasForeignKey(d => d.ma_phong)
                .HasConstraintName("dien_nuoc_ma_phong_fkey");
        });

        modelBuilder.Entity<hoa_don>(entity =>
        {
            entity.HasKey(e => e.ma_hoa_don).HasName("hoa_don_pkey");

            entity.ToTable("hoa_don");

            entity.HasIndex(e => e.ky_hoa_don, "idx_hoa_don_ky");

            entity.HasIndex(e => e.trang_thai, "idx_hoa_don_trang_thai");

            entity.HasIndex(e => new { e.ma_phong, e.ky_hoa_don }, "uq_hoa_don").IsUnique();

            entity.Property(e => e.ma_hoa_don).HasMaxLength(20);
            entity.Property(e => e.ma_phong).HasMaxLength(10);
            entity.Property(e => e.ngay_lap)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ngay_thanh_toan).HasColumnType("timestamp without time zone");
            entity.Property(e => e.tien_dien).HasPrecision(18);
            entity.Property(e => e.tien_nuoc).HasPrecision(18);
            entity.Property(e => e.tien_phat).HasPrecision(18);
            entity.Property(e => e.tien_phong).HasPrecision(18);
            entity.Property(e => e.tong_tien)
                .HasPrecision(18)
                .HasComputedColumnSql("(((tien_phong + tien_dien) + tien_nuoc) + tien_phat)", true);
            entity.Property(e => e.trang_thai).HasDefaultValue((short)0);

            entity.HasOne(d => d.ma_phongNavigation).WithMany(p => p.hoa_dons)
                .HasForeignKey(d => d.ma_phong)
                .HasConstraintName("hoa_don_ma_phong_fkey");
        });

        modelBuilder.Entity<hop_dong>(entity =>
        {
            entity.HasKey(e => e.ma_hop_dong).HasName("hop_dong_pkey");

            entity.ToTable("hop_dong");

            entity.HasIndex(e => e.ma_phong, "idx_hop_dong_ma_phong");

            entity.HasIndex(e => e.ma_sv, "idx_hop_dong_ma_sv");

            entity.HasIndex(e => new { e.ma_sv, e.ma_phong, e.ngay_bat_dau }, "uq_hop_dong").IsUnique();

            entity.Property(e => e.ma_hop_dong).HasMaxLength(20);
            entity.Property(e => e.ma_phong).HasMaxLength(10);
            entity.Property(e => e.ma_sv).HasMaxLength(20);
            entity.Property(e => e.ngay_bat_dau).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ngay_ket_thuc).HasColumnType("timestamp without time zone");
            entity.Property(e => e.tinh_trang).HasDefaultValue((short)0);

            entity.HasOne(d => d.ma_phongNavigation).WithMany(p => p.hop_dongs)
                .HasForeignKey(d => d.ma_phong)
                .HasConstraintName("hop_dong_ma_phong_fkey");

            entity.HasOne(d => d.ma_svNavigation).WithMany(p => p.hop_dongs)
                .HasForeignKey(d => d.ma_sv)
                .HasConstraintName("hop_dong_ma_sv_fkey");
        });

        modelBuilder.Entity<phong>(entity =>
        {
            entity.HasKey(e => e.ma_phong).HasName("phong_pkey");

            entity.ToTable("phong");

            entity.Property(e => e.ma_phong).HasMaxLength(10);
            entity.Property(e => e.loai_phong).HasMaxLength(50);
            entity.Property(e => e.ma_toa).HasMaxLength(10);
            entity.Property(e => e.so_nguoi_hien_tai).HasDefaultValue(0);
            entity.Property(e => e.ten_phong).HasMaxLength(50);
            entity.Property(e => e.trang_thai).HasDefaultValue((short)0);

            entity.HasOne(d => d.ma_toaNavigation).WithMany(p => p.phongs)
                .HasForeignKey(d => d.ma_toa)
                .HasConstraintName("phong_ma_toa_fkey");
        });

        modelBuilder.Entity<sinh_vien>(entity =>
        {
            entity.HasKey(e => e.ma_sv).HasName("sinh_vien_pkey");

            entity.ToTable("sinh_vien");

            entity.Property(e => e.ma_sv).HasMaxLength(20);
            entity.Property(e => e.dia_chi).HasMaxLength(100);
            entity.Property(e => e.ho_ten).HasMaxLength(100);
            entity.Property(e => e.lop).HasMaxLength(50);
            entity.Property(e => e.sdt).HasMaxLength(10);
        });

        modelBuilder.Entity<tai_khoan>(entity =>
        {
            entity.HasKey(e => e.ten_dang_nhap).HasName("tai_khoan_pkey");

            entity.ToTable("tai_khoan");

            entity.Property(e => e.ten_dang_nhap).HasMaxLength(50);
            entity.Property(e => e.ma_sv).HasMaxLength(20);
            entity.Property(e => e.mat_khau).HasMaxLength(100);

            entity.HasOne(d => d.ma_svNavigation).WithMany(p => p.tai_khoans)
                .HasForeignKey(d => d.ma_sv)
                .HasConstraintName("tai_khoan_ma_sv_fkey");
        });

        modelBuilder.Entity<toa_nha>(entity =>
        {
            entity.HasKey(e => e.ma_toa).HasName("toa_nha_pkey");

            entity.ToTable("toa_nha");

            entity.Property(e => e.ma_toa).HasMaxLength(10);
            entity.Property(e => e.ten_toa).HasMaxLength(50);
        });

        modelBuilder.Entity<vi_pham>(entity =>
        {
            entity.HasKey(e => e.ma_vi_pham).HasName("vi_pham_pkey");

            entity.ToTable("vi_pham");

            entity.Property(e => e.hinh_thuc_xu_ly).HasMaxLength(50);
            entity.Property(e => e.ma_sv).HasMaxLength(20);
            entity.Property(e => e.ngay_vi_pham)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.noi_dung).HasMaxLength(500);

            entity.HasOne(d => d.ma_svNavigation).WithMany(p => p.vi_phams)
                .HasForeignKey(d => d.ma_sv)
                .HasConstraintName("vi_pham_ma_sv_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
