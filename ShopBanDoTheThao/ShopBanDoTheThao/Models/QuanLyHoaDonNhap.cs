using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ShopBanDoTheThao.Models
{
    public class QuanLyHoaDonNhap
    {
        public IPagedList<HoaDonNhap> DanhSachHoaDonNhap { get; set; }
        public IPagedList<ChiTietHoaDonNhap> DanhSachChiTietHoaDonNhap { get; set; }
        public HoaDonNhap HoaDonNhap { get; set; }
        public ChiTietHoaDonNhap ChiTietHoaDonNhap { get; set; }
        public SanPham SanPham { get; set; }
        public NhaPhanPhois NhaPhanPhois { get; set; }
    }
}