using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanDoTheThao.Models
{
    public class ThongKeViewModel
    {
        public List<HoaDon> DanhSachHoaDonBan { get; set; }
        public List<TaiKhoan> TaiKhoanDangKi { get; set; }
        public List<HoaDonNhap> DanhSachHoaDonNhaps { get; set; } 
        public List<SanPham> DanhSachSanPhamXemNhieu { get; set; }
        public List<SanPhamBanChay> DanhSachSanPhamBanNhieu { get; set; }
       public List<HoaDon> DanhSachHoaDonChuaDuyet { get; set; }
        public List<HoaDon> DanhSachHoaDonDaDuyet { get; set; }
        public decimal TongGiaTriHoaDonDaDuyet { get; set; }
        public decimal TongGiaTriHoaDonChuaDuyet { get; set; }
        public decimal TongThuNhap { get; set; }
        public decimal TongNhapHang { get; set; }
        public List<TinTuc> DanhSachTinTucs { get; set; }
        
        public List<SanPham> DanhSachSanPhamTonKho { get; set; } 
        public decimal TongGiaTriTonKho { get; set; }
    }
}