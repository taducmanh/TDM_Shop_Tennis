namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
            ChiTietHoaDonNhaps = new HashSet<ChiTietHoaDonNhap>();
            ChiTietSanPhams = new HashSet<ChiTietSanPham>();
            NhaPhanPhois = new HashSet<NhaPhanPhois>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaSanPham { get; set; }

        public int? MaChuyenMuc { get; set; }

        public string TenSanPham { get; set; }

        public string AnhDaiDien { get; set; }

        public decimal? Gia { get; set; }

        public decimal? GiaGiam { get; set; }

        public int? SoLuong { get; set; }

        public bool? TrangThai { get; set; }

        public int? LuotXem { get; set; }

        public bool? DacBiet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhaPhanPhois> NhaPhanPhois { get; set; }
        public virtual ChuyenMuc ChuyenMuc { get; set; }
    }
}
