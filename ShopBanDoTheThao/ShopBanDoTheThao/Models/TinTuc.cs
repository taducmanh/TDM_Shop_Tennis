namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TinTuc")]
    public partial class TinTuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaTinTuc { get; set; }

        public string TieuDe { get; set; }

        public string AnhDaiDien { get; set; }

        public string MoTa { get; set; }

        public DateTime? NgayTao { get; set; }

        public string ChiTiet { get; set; }

        public int? LuotXem { get; set; }
    }
}
