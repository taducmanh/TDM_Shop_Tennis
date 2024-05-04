using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopBanDoTheThao.Models;
using PagedList;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLySanPhamController : Controller
    {
        // GET: AdminQuanLySanPham
       [HttpGet]
        public ActionResult Index(string searchString, int? page)
        {
            if (Session["MaTKAdmin"] == null )
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    KhoiTao(searchString, page);
                    QuanLySanPham model = new QuanLySanPham()
                    {

                        ListSanPham = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception)
                {
                   
                    throw new Exception("Đã xảy ra lỗi , xin vui lòng thử lại");
                }
               
               
            }
           
        }

       [HttpPost]
        public ActionResult XoaSanPham(int id)
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
                    var sanPham = shop.SanPhams.Single(s => s.MaSanPham == id);
                    var ds = shop.NhaPhanPhois.Where(s => s.SanPhams.Any(e => e.MaSanPham == id)).ToList();
                    foreach (var item in ds)
                    {
                        sanPham.NhaPhanPhois.Remove(item);
                        shop.SaveChanges();
                    }
                    var chiTiet = shop.ChiTietSanPhams.SingleOrDefault(c => c.MaSanPham == id);
                    shop.ChiTietSanPhams.Remove(chiTiet);
                    shop.SanPhams.Remove(sanPham);
                    shop.SaveChanges();

                    return Json(new { ct = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception eex)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }
      
        public IPagedList<SanPham> KhoiTao(string searchString, int? page)
        {

            try
            {
                ViewBag.CurrentFilter = searchString;
                ShopEntities shop = new ShopEntities();
                var dsChuyenMuc = shop.ChuyenMucs.Where(c => c.MaChuyenMucCha != null && c.DacBiet == false).ToList();
                var dsHangSX = shop.HangSanXuats.ToList();

                List<SelectListItem> chuyenMuc = new List<SelectListItem>();
                List<SelectListItem> HangSX = new List<SelectListItem>();

                chuyenMuc.Add(new SelectListItem() { Text = "XE ĐẠP TẬP THỂ DỤC", Value = "34" });
                chuyenMuc.Add(new SelectListItem() { Text = "DỤNG CỤ VÕ THUẬT", Value = "36" });
                for (int i = 0; i < dsChuyenMuc.Count; i++)
                {
                    SelectListItem sl = new SelectListItem() { Text = dsChuyenMuc[i].TenChuyenMuc, Value = dsChuyenMuc[i].MaChuyenMuc.ToString() };
                    chuyenMuc.Add(sl);
                }
                for (int i = 0; i < dsHangSX.Count; i++)
                {
                    SelectListItem sl = new SelectListItem() { Text = dsHangSX[i].TenHang, Value = dsHangSX[i].MaHang.ToString() };
                    HangSX.Add(sl);
                }

                ViewBag.CM = chuyenMuc;
                ViewBag.HangSX = HangSX;
                var dsSanpham = shop.SanPhams.OrderByDescending(s => s.MaSanPham);
                if (!String.IsNullOrEmpty(searchString))
                {
                    dsSanpham = shop.SanPhams.Where(s => s.TenSanPham.Contains(searchString)).OrderByDescending(s => s.MaSanPham);
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                dsSanpham.ToPagedList(pageNumber, pageSize);
                ViewData["dsSanPham"] = dsSanpham;
                return dsSanpham.ToPagedList(pageNumber, pageSize);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(QuanLySanPham sp, HttpPostedFileBase file,string submit,int id,string img)
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
                    if (submit == "Thêm")
                    {

                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/Products/"), fileName);
                                file.SaveAs(path);
                            }
                            SanPham sanPham = new SanPham();
                            sanPham.MaChuyenMuc = sp.SanPham.MaChuyenMuc;
                            sanPham.TenSanPham = sp.SanPham.TenSanPham;
                            sanPham.SoLuong = 0;
                            sanPham.DacBiet = sp.SanPham.DacBiet;
                            sanPham.TrangThai = true;
                            sanPham.AnhDaiDien = "/Images/Products/" + fileName;
                            sanPham.Gia = sp.SanPham.Gia;
                            sanPham.GiaGiam = sp.SanPham.GiaGiam ?? 0;
                            sanPham.LuotXem = 0;

                            shop.SanPhams.Add(sanPham);
                            shop.SaveChanges();


                            var spMoi = (from s in shop.SanPhams orderby s.MaSanPham descending select s).FirstOrDefault();

                            ChiTietSanPham chiTiet = new ChiTietSanPham();
                            chiTiet.MaSanPham = spMoi.MaSanPham;
                            chiTiet.MaNhaSanXuat = sp.ChiTietSanPham.MaNhaSanXuat;
                            chiTiet.MoTa = sp.ChiTietSanPham.MoTa;
                            chiTiet.ChiTiet = sp.ChiTietSanPham.ChiTiet;
                            shop.ChiTietSanPhams.Add(chiTiet);
                            shop.SaveChanges();

                        }
                    }
                    if (submit == "Sửa")
                    {
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/Products/"), fileName);
                                file.SaveAs(path);
                                var sanPham = shop.SanPhams.Single(s => s.MaSanPham == id);
                                sanPham.MaChuyenMuc = sp.SanPham.MaChuyenMuc;
                                sanPham.TenSanPham = sp.SanPham.TenSanPham;
                                sanPham.GiaGiam = sp.SanPham.GiaGiam ?? 0;
                                sanPham.AnhDaiDien = "/Images/Products/" + fileName;
                                sanPham.Gia = sp.SanPham.Gia;
                                sanPham.DacBiet = sp.SanPham.DacBiet;
                                sanPham.TrangThai = true;
                                sanPham.LuotXem = sp.SanPham.LuotXem;
                                shop.SaveChanges();
                                var chiTiet = shop.ChiTietSanPhams.SingleOrDefault(c => c.MaSanPham == id);
                                if (chiTiet != null)
                                {
                                    chiTiet.MaNhaSanXuat = sp.ChiTietSanPham.MaNhaSanXuat;
                                    chiTiet.MoTa = sp.ChiTietSanPham.MoTa;
                                    chiTiet.ChiTiet = sp.ChiTietSanPham.ChiTiet;
                                    shop.SaveChanges();
                                }
                                else
                                {
                                    var ct = new ChiTietSanPham();
                                    ct.MaSanPham = id;
                                    ct.MaNhaSanXuat = sp.ChiTietSanPham.MaNhaSanXuat;
                                    ct.MoTa = sp.ChiTietSanPham.MoTa;
                                    ct.ChiTiet = sp.ChiTietSanPham.ChiTiet;
                                    shop.ChiTietSanPhams.Add(ct);
                                    shop.SaveChanges();

                                }
                            }
                        }
                        else
                        {
                            var sanPham = shop.SanPhams.Single(s => s.MaSanPham == id);
                            sanPham.MaChuyenMuc = sp.SanPham.MaChuyenMuc;
                            sanPham.TenSanPham = sp.SanPham.TenSanPham;
                            sanPham.AnhDaiDien = img;
                            sanPham.DacBiet = sp.SanPham.DacBiet;
                            sanPham.TrangThai = true;
                            sanPham.GiaGiam = sp.SanPham.GiaGiam ?? 0;
                            sanPham.GiaGiam = sp.SanPham.GiaGiam;
                            sanPham.LuotXem = sp.SanPham.LuotXem;
                            shop.SaveChanges();

                            var chiTiet = shop.ChiTietSanPhams.SingleOrDefault(c => c.MaSanPham == id);
                            if (chiTiet != null)
                            {
                                chiTiet.MaNhaSanXuat = sp.ChiTietSanPham.MaNhaSanXuat;
                                chiTiet.MoTa = sp.ChiTietSanPham.MoTa;
                                chiTiet.ChiTiet = sp.ChiTietSanPham.ChiTiet;
                                shop.SaveChanges();
                            }
                            else
                            {
                                ChiTietSanPham ct = new ChiTietSanPham();
                                ct.MaSanPham = id;
                                ct.MaNhaSanXuat = sp.ChiTietSanPham.MaNhaSanXuat;
                                ct.MoTa = sp.ChiTietSanPham.MoTa;
                                ct.ChiTiet = sp.ChiTietSanPham.ChiTiet;
                                shop.ChiTietSanPhams.Add(ct);
                                shop.SaveChanges();

                            }
                        }

                    }
                    QuanLySanPham model = new QuanLySanPham()
                    {

                        ListSanPham = KhoiTao("", 1)
                    };
                    return View("Index", model);
                }
                catch (Exception ex)
                {

                    return RedirectToAction("Index");
                }
                
            }
           
        }
        [HttpGet]
       
        public ActionResult LayChiTietSanPham(int masp)
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

                    ChiTietSanPham ct = shop.ChiTietSanPhams.SingleOrDefault(t => t.MaSanPham == masp);

                    return this.Json(new { mansx = ct.MaNhaSanXuat, mota = ct.MoTa, chitiet = ct.ChiTiet }, JsonRequestBehavior.AllowGet); ;
                }
                catch (Exception ex)
                {

                    return RedirectToAction("Index");
                }
               

            }
           
        }
    }
   
  
}