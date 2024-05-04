namespace ShopBanDoTheThao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaiDat")]
    public partial class CaiDat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Logo { get; set; }

        public string GioLamViec { get; set; }

        public string GiaoHang { get; set; }

        public string HoanTien { get; set; }

        public string SDTLienHe { get; set; }

        public string EmailLienHe { get; set; }

        public string FaceBook { get; set; }

        public string GooglePlus { get; set; }

        public string Twiter { get; set; }

        public string YouTube { get; set; }

        public string Instargram { get; set; }

        public string GoogleMap { get; set; }

        public string MatKhauMail { get; set; }
    }
}
