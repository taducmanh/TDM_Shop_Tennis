using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ShopBanDoTheThao.Models
{
    public class QuanLyDonHang
    {
        public IPagedList<HoaDon> DanhSachHoaDon { get; set; }
        public HoaDon HoaDon { get; set; }
        public IPagedList<ChiTietHoaDon> DanhSachChiTiet { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public ChiTietTaiKhoan ChiTietTaiKhoan { get; set; }
        public ChiTietHoaDon ChiTietHoaDon { get; set; }
    }
}