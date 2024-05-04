using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ShopEntities shop = new ShopEntities();
            ViewData["SanPhamMoi"] = shop.SanPhams.Where(s=>s.DacBiet==false).OrderByDescending(c => c.MaSanPham).Skip(0).Take(12).ToList();

            ViewData["SanPhamDacSac"] = shop.SanPhams.Where(c=>c.DacBiet==true).OrderByDescending(c => c.MaSanPham ).Skip(0).Take(12).ToList();
            ViewData["TinMoiNhat"] = shop.TinTucs.OrderByDescending(t => t.MaTinTuc).Skip(0).Take(3).ToList();
            ViewData["DanhSachChuyenMuc"] =
                shop.ChuyenMucs.Where(c => c.MaChuyenMucCha == null && c.DacBiet == false)
                    .OrderByDescending(c => c.MaChuyenMuc)
                    .ToList();    
            return View();
        }

     

       
        public ActionResult TimKiem(string q,int machuyenmuc)
        {
           
            ShopEntities shop = new ShopEntities();
            var dsChuyenMuc= shop.ChuyenMucs.Where(p =>p.MaChuyenMucCha == machuyenmuc).ToList();
            var list=new List<SanPham>();
            foreach (var item in dsChuyenMuc)
            {
                list.AddRange(shop.SanPhams.Where(s=>s.TenSanPham.Contains(q) && s.MaChuyenMuc==item.MaChuyenMuc).ToList());
            }
            ViewData["DanhSachSanPham"] = list;
            return View();
        }

    }
}