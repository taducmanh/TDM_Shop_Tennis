namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NhaPhanPhois
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaPhanPhois()
        {
            HoaDonNhaps = new HashSet<HoaDonNhap>();
            SanPhams = new HashSet<SanPham>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaNhaPhanPhoi { get; set; }

        public string TenNhaPhanPhoi { get; set; }

        public string DiaChi { get; set; }

        public string SoDienThoai { get; set; }

        public string Fax { get; set; }

        public string MoTa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
