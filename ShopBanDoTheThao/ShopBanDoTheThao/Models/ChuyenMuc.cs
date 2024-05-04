namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChuyenMuc")]
    public partial class ChuyenMuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChuyenMuc()
        {
            SanPhams = new HashSet<SanPham>();
            ChuyenMuc1 = new HashSet<ChuyenMuc>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaChuyenMuc { get; set; }

        public int? MaChuyenMucCha { get; set; }

        public string TenChuyenMuc { get; set; }

        public bool? DacBiet { get; set; }

        public string NoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuyenMuc> ChuyenMuc1 { get; set; }

        public virtual ChuyenMuc ChuyenMuc2 { get; set; }
    }
}
