using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ShopBanDoTheThao.Models
{
    public partial class ShopEntities : DbContext
    {
        public ShopEntities()
            : base("name=ShopEntities")
        {
        }

        public virtual DbSet<CaiDat> CaiDats { get; set; }
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }
        public virtual DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public virtual DbSet<ChiTietTaiKhoan> ChiTietTaiKhoans { get; set; }
        public virtual DbSet<ChuyenMuc> ChuyenMucs { get; set; }
        public virtual DbSet<DKBanTin> DKBanTins { get; set; }
        public virtual DbSet<HangSanXuat> HangSanXuats { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<LoaiTaiKhoan> LoaiTaiKhoans { get; set; }
        public virtual DbSet<NhaPhanPhois> NhaPhanPhois { get; set; }
        public virtual DbSet<QuangCao> QuangCaos { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<TinTuc> TinTucs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(e => e.TongGia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .Property(e => e.GiaNhap)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .Property(e => e.TongTien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ChuyenMuc>()
                .HasMany(e => e.ChuyenMuc1)
                .WithOptional(e => e.ChuyenMuc2)
                .HasForeignKey(e => e.MaChuyenMucCha);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.TongGia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LoaiTaiKhoan>()
                .HasMany(e => e.TaiKhoans)
                .WithOptional(e => e.LoaiTaiKhoan1)
                .HasForeignKey(e => e.LoaiTaiKhoan);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.Gia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.GiaGiam)
                .HasPrecision(18, 0);
        }
    }
}
