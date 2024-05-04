namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietTaiKhoan")]
    public partial class ChiTietTaiKhoan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaChitietTaiKhoan { get; set; }

        public int? MaTaiKhoan { get; set; }

        public string HoTen { get; set; }

        public string DiaChi { get; set; }

        public string SoDienThoai { get; set; }

        public string AnhDaiDien { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
