using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: ManageProducts
        //[HttpGet]
        //public ActionResult ChiTietSanPham()
        //{

        //    return View();
        //}
        //[HttpPost]
        public ActionResult ChiTietSanPham(int id)
        {
            ShopEntities shop=new ShopEntities();
            SanPham product= shop.SanPhams.SingleOrDefault(p => p.MaSanPham == id);

            //TangLuotXem(id);
            //ViewData["SanPham"] = ;
            ViewData["LienQuan"] = shop.SanPhams.Where(p => p.MaChuyenMuc == product.MaChuyenMuc).OrderByDescending(s => s.MaSanPham).Skip(0).Take(6).ToList();
            
            ViewData["ChiTiet"] = shop.ChiTietSanPhams.SingleOrDefault(d => d.MaSanPham == id);
            ViewData["DanhSachChuyenMuc"] =
               shop.ChuyenMucs.Where(c => c.MaChuyenMucCha == null && c.DacBiet == false)
                   .OrderByDescending(c => c.MaChuyenMuc)
                   .ToList();
            var cm = shop.ChuyenMucs.SingleOrDefault(c => c.MaChuyenMuc == product.MaChuyenMuc);
            if(cm != null && cm.MaChuyenMucCha==null)
            {
                ViewBag.Id = cm.MaChuyenMucCha;
            }
            else
            {
                if (cm != null) ViewBag.Id = cm.MaChuyenMuc;
            }
           
            ViewData["sp"] = product;
            return View();
        }
        [HttpPost]
        public void TangLuotXem(int id)
        {
            //if (Session["ma_san_pham"] == null)
            //{
                ShopEntities shop = new ShopEntities();
                SanPham product = shop.SanPhams.SingleOrDefault(p => p.MaSanPham == id);
                product.LuotXem += 1;
           
                 shop.SaveChanges();
                Session["ma_san_pham"] = id;
            //}

            //return View("ChiTietSanPham",id);
        }
    }
}