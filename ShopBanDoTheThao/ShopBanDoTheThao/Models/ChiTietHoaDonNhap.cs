namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietHoaDonNhap")]
    public partial class ChiTietHoaDonNhap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? MaHoaDon { get; set; }

        public int? MaSanPham { get; set; }

        public int? SoLuong { get; set; }

        public string DonViTinh { get; set; }

        public decimal? GiaNhap { get; set; }

        public decimal? TongTien { get; set; }

        public virtual HoaDonNhap HoaDonNhap { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
