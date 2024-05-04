namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string TenKH { get; set; }

        public bool? GioiTinh { get; set; }

        public string DiaChi { get; set; }

        public string SDT { get; set; }

        public string Email { get; set; }
    }
}
