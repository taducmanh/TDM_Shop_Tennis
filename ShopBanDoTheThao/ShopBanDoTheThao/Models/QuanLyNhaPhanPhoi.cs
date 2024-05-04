using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ShopBanDoTheThao.Models
{
    public class QuanLyNhaPhanPhoi
    {
        public IPagedList<NhaPhanPhois> DanhSachNhaPhanPhoi { get; set; }
        public NhaPhanPhois NhaPhanPhois { get; set; }
        public SanPham SanPham { get; set; }
        public IPagedList<SanPham> DanhSachSanPhamPhanPhoi { get; set; } 
    }
}