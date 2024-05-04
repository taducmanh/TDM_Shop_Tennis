namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHoaDon { get; set; }

        public bool? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayDuyet { get; set; }

        public decimal? TongGia { get; set; }

        public string TenKH { get; set; }

        public bool? GioiTinh { get; set; }

        public string DiaChi { get; set; }

        public string Email { get; set; }

        public string SDT { get; set; }

        public string DiaChiGiaoHang { get; set; }

        public DateTime? ThoiGianGiaoHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
