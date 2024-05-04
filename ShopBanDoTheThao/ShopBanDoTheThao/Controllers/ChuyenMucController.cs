using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class ChuyenMucController : Controller
    {
       
      
        public ActionResult Index(int id)
        {
           
            ShopEntities shop = new ShopEntities();
           var dsChuyenMuc = shop.ChuyenMucs.Where(c => c.MaChuyenMucCha == id).ToList();
            ViewData["DanhSachChuyenMuc"] = dsChuyenMuc;
            var dsSanPham=new List<SanPham>();
            if (dsChuyenMuc.Count != 0)
            {
                foreach (var item in dsChuyenMuc)
                {
                    var ds = shop.SanPhams.Where(s => s.MaChuyenMuc == item.MaChuyenMuc).ToList();
                    dsSanPham.AddRange(ds);
                }
            }
            else
            {
                var ds = shop.SanPhams.Where(s => s.MaChuyenMuc == id).ToList();
                dsSanPham.AddRange(ds);
            }
            ViewData["DanhSachSanPham"] = dsSanPham;
           
            return View();
        }

        public ActionResult LaySanPham(int id)
        {
            ShopEntities shop=new ShopEntities();
            ChuyenMuc cm = shop.ChuyenMucs.SingleOrDefault(s => s.MaChuyenMuc == id);
          

            ViewBag.TenChuyenMuc = cm.TenChuyenMuc;
           
            ViewData["DanhSachChuyenMuc"] = shop.ChuyenMucs.Where(c => c.MaChuyenMucCha == cm.MaChuyenMucCha).ToList();
            ViewData["DanhSachSanPham"] = shop.SanPhams.Where(s => s.MaChuyenMuc == id).ToList();
            return View("Index");
        }

        public ActionResult ChuyenMucDacBiet(int id)
        {
            ShopEntities shop=new ShopEntities();
            var model = shop.ChuyenMucs.SingleOrDefault(c => c.MaChuyenMuc == id);
            
            return View(model);
        }
    }
}