using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLyHangController : Controller
    {
        // GET: AdminQuanLyHang
        [HttpGet]
        public ActionResult Index(string searchString, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    var model = new QuanLyHangSX()
                    {
                        DanhSachHangSanXuat = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception)
                {

                    return View();
                }
               
            }
            
        }

        public IPagedList<HangSanXuat> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();

            var dsHang = shop.HangSanXuats.OrderByDescending(s => s.MaHang);
            if (!String.IsNullOrEmpty(searchString))
            {
                dsHang = shop.HangSanXuats.Where(s => s.TenHang.Contains(searchString)).OrderByDescending(s => s.MaHang);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsHang.ToPagedList(pageNumber, pageSize);
            return dsHang.ToPagedList(pageNumber, pageSize);
        }

        public ActionResult ThemHangSX(QuanLyHangSX qlhang, HttpPostedFileBase file, string submit, int id, string img)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    var model = new QuanLyHangSX()
                    {
                        DanhSachHangSanXuat = KhoiTao("", 1)
                    };
                    ShopEntities shop = new ShopEntities();
                    if (submit == "Thêm")
                    {
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/HangSXs/"), fileName);
                                file.SaveAs(path);
                            }
                            HangSanXuat hang = new HangSanXuat();

                            hang.TenHang = qlhang.HangSanXuat.TenHang;
                            hang.LinkWeb = qlhang.HangSanXuat.LinkWeb;
                            hang.AnhDaiDien = "/Images/HangSXs/" + fileName;
                            shop.HangSanXuats.Add(hang);
                            shop.SaveChanges();
                        }
                    }
                    if (submit == "Sửa")
                    {
                        var hang = shop.HangSanXuats.SingleOrDefault(h => h.MaHang == id);
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/HangSXs/"), fileName);
                                file.SaveAs(path);

                                hang.TenHang = qlhang.HangSanXuat.TenHang;
                                hang.LinkWeb = qlhang.HangSanXuat.LinkWeb;
                                hang.AnhDaiDien = "/Images/HangSXs/" + fileName;
                                shop.SaveChanges();
                            }
                        }
                        else
                        {
                            hang.TenHang = qlhang.HangSanXuat.TenHang;
                            hang.LinkWeb = qlhang.HangSanXuat.LinkWeb;
                            hang.AnhDaiDien = img;
                            shop.SaveChanges();
                        }
                    }


                    return RedirectToAction("Index", model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
           

        }
        [HttpPost]
        public ActionResult XoaHang(int id)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                   
                    ShopEntities shop = new ShopEntities();
                    var hang = shop.HangSanXuats.Single(s => s.MaHang == id);
                    var dsChiTietSP = shop.ChiTietSanPhams.Where(c => c.MaNhaSanXuat == id).ToList();
                    foreach (var item in dsChiTietSP)
                    {

                        var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == item.MaSanPham);
                        shop.ChiTietSanPhams.Remove(item);
                        shop.SanPhams.Remove(sp);
                        shop.SaveChanges();
                    }
                    shop.HangSanXuats.Remove(hang);
                    shop.SaveChanges();
                    return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
            
        }
    }
}