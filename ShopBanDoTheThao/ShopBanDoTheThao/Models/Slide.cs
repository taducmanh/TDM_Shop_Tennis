namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaAnh { get; set; }

        public string TieuDe { get; set; }

        public string TieuDe1 { get; set; }

        public string TieuDe2 { get; set; }

        public string MoTa1 { get; set; }

        public string MoTa2 { get; set; }

        public string MoTa3 { get; set; }

        public string MoTa4 { get; set; }

        public string LinkAnh { get; set; }
    }
}
