namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietSanPham")]
    public partial class ChiTietSanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaChiTietSanPham { get; set; }

        public int? MaSanPham { get; set; }

        public int? MaNhaSanXuat { get; set; }

        public string MoTa { get; set; }

        public string ChiTiet { get; set; }

        public virtual HangSanXuat HangSanXuat { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
