using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ShopBanDoTheThao.Models
{
    public class QuanLyTaiKhoan
    {
        public IPagedList<TaiKhoan> DanhSachTaiKhoan { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public ChiTietTaiKhoan ChiTietTaiKhoan { get; set; }
    }
}