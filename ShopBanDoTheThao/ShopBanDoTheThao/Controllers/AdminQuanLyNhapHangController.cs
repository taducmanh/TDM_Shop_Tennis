using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLyNhapHangController : Controller
    {
        // GET: AdminQuanLyNhapHang
        public static int maNPP,maHoaDon;

       public static int id_rows=0;
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
                    //ViewBag.i = id_rows;
                    var model = new QuanLyHoaDonNhap()
                    {
                        DanhSachHoaDonNhap = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }

        public IPagedList<HoaDonNhap> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();
            var dsNhaPP = shop.NhaPhanPhois.ToList();
            List<SelectListItem> slNhaPP = new List<SelectListItem>();

            for (int i = 0; i < dsNhaPP.Count; i++)
            {
                SelectListItem sl = new SelectListItem() { Text = dsNhaPP[i].TenNhaPhanPhoi, Value = dsNhaPP[i].MaNhaPhanPhoi.ToString() };
                slNhaPP.Add(sl);
            }
            ViewBag.NPP = slNhaPP;
            var dsHoaDonNhap = shop.HoaDonNhaps.OrderByDescending(s => s.MaHoaDon);

            if (!String.IsNullOrEmpty(searchString))
            {
                dsHoaDonNhap = shop.HoaDonNhaps.Where(s => s.NhaPhanPhois.TenNhaPhanPhoi.Contains(searchString)).OrderByDescending(s => s.MaHoaDon);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsHoaDonNhap.ToPagedList(pageNumber, pageSize);
            return dsHoaDonNhap.ToPagedList(pageNumber, pageSize);
        }

        public ActionResult ThemHoaDonNhap(QuanLyHoaDonNhap qlhd)
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
                    HoaDonNhap hoaDon = new HoaDonNhap();
                    hoaDon.MaNhaPhanPhoi = qlhd.HoaDonNhap.MaNhaPhanPhoi;
                    hoaDon.NgayTao = DateTime.Now;
                    hoaDon.KieuThanhToan = qlhd.HoaDonNhap.KieuThanhToan;
                    hoaDon.MaTaiKhoan = int.Parse(Session["MaTKAdmin"].ToString());
                    shop.HoaDonNhaps.Add(hoaDon);
                    shop.SaveChanges();
                    var hoaDonMoi = (from h in shop.HoaDonNhaps orderby h.MaHoaDon descending select h).FirstOrDefault();
                    return RedirectToAction("ChiTietHoaDonNhap", new { id = hoaDonMoi.MaHoaDon });
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
              
            }
           
        }
        [HttpPost]
        public ActionResult XoaHoaDonNhap(int id)
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
                    var hoaDon = shop.HoaDonNhaps.Single(s => s.MaHoaDon == id);
                    var dsChiTiet = shop.ChiTietHoaDonNhaps.Where(c => c.MaHoaDon == id).ToList();
                    foreach (var item in dsChiTiet)
                    {
                        var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == item.MaSanPham);

                        if (sp != null) sp.SoLuong = sp.SoLuong - item.SoLuong;
                        shop.ChiTietHoaDonNhaps.Remove(item);
                        shop.SaveChanges();
                    }
                    shop.HoaDonNhaps.Remove(hoaDon);
                    shop.SaveChanges();
                    return Json(new { dt = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
              
            }
            
        }
        public IPagedList<ChiTietHoaDonNhap> KhoiTaoChitietHoaDonNhap(int id, int? page)
        {
            ShopEntities shop = new ShopEntities();
         
            var dsChiTiet = shop.ChiTietHoaDonNhaps.Where(c => c.MaHoaDon == id).OrderByDescending(s => s.Id); ;
            var hoaDonNhap = shop.HoaDonNhaps.Single(h => h.MaHoaDon == id);
            var npp = shop.NhaPhanPhois.Single(n => n.MaNhaPhanPhoi == hoaDonNhap.MaNhaPhanPhoi);
            maNPP = npp.MaNhaPhanPhoi;
            var dsSanPham = npp.SanPhams.ToList();
            List<SelectListItem> slSanPham= dsSanPham.Select(t => new SelectListItem() {Text = t.TenSanPham, Value = t.MaSanPham.ToString()}).ToList();

            ViewBag.SP = slSanPham;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsChiTiet.ToPagedList(pageNumber, pageSize);
            return dsChiTiet.ToPagedList(pageNumber, pageSize);
        }
        [HttpGet]
        public ActionResult ChiTietHoaDonNhap(int id, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    ViewBag.i = id_rows;
                    maHoaDon = id;
                    var model = new QuanLyHoaDonNhap()
                    {
                        DanhSachChiTietHoaDonNhap = KhoiTaoChitietHoaDonNhap(id, page)
                    };

                    return View(model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
          
        }
        [HttpPost]
        public ActionResult ThemSanPham(QuanLyHoaDonNhap qlhd)
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
                    var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == qlhd.ChiTietHoaDonNhap.MaSanPham);

                    var chiTietHoaDonNhap = new ChiTietHoaDonNhap();
                    chiTietHoaDonNhap.MaSanPham = qlhd.ChiTietHoaDonNhap.MaSanPham;
                    chiTietHoaDonNhap.MaHoaDon = maHoaDon;
                    chiTietHoaDonNhap.SoLuong = qlhd.ChiTietHoaDonNhap.SoLuong;
                    chiTietHoaDonNhap.DonViTinh = qlhd.ChiTietHoaDonNhap.DonViTinh;
                    chiTietHoaDonNhap.GiaNhap = qlhd.ChiTietHoaDonNhap.GiaNhap;
                    chiTietHoaDonNhap.TongTien = (qlhd.ChiTietHoaDonNhap.GiaNhap * qlhd.ChiTietHoaDonNhap.SoLuong);

                    shop.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                    sp.SoLuong += qlhd.ChiTietHoaDonNhap.SoLuong;
                    shop.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }
        [HttpPost]
        public ActionResult SuaChiTietHoaDon(QuanLyHoaDonNhap qlhd)
        {
            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    ShopEntities shop = new ShopEntities();
                    var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == qlhd.ChiTietHoaDonNhap.MaSanPham);

                    var chiTietHoaDonNhap = shop.ChiTietHoaDonNhaps.SingleOrDefault(c => c.Id == qlhd.ChiTietHoaDonNhap.Id);
                    sp.SoLuong -= chiTietHoaDonNhap.SoLuong;
                    chiTietHoaDonNhap.MaSanPham = qlhd.ChiTietHoaDonNhap.MaSanPham;
                    chiTietHoaDonNhap.MaHoaDon = maHoaDon;
                    chiTietHoaDonNhap.SoLuong = qlhd.ChiTietHoaDonNhap.SoLuong;
                    chiTietHoaDonNhap.DonViTinh = qlhd.ChiTietHoaDonNhap.DonViTinh;
                    chiTietHoaDonNhap.GiaNhap = qlhd.ChiTietHoaDonNhap.GiaNhap;
                    chiTietHoaDonNhap.TongTien = (qlhd.ChiTietHoaDonNhap.GiaNhap * qlhd.ChiTietHoaDonNhap.SoLuong);

                    sp.SoLuong += qlhd.ChiTietHoaDonNhap.SoLuong;
                    shop.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
               
            }

        }
        [HttpPost]
        public ActionResult SuaSanPham(QuanLyHoaDonNhap qlhd,int id)
        {
            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    ShopEntities shop = new ShopEntities();
                    var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == qlhd.ChiTietHoaDonNhap.MaSanPham);

                    var chiTietHoaDonNhap = new ChiTietHoaDonNhap();
                    chiTietHoaDonNhap.MaSanPham = qlhd.ChiTietHoaDonNhap.MaSanPham;
                    chiTietHoaDonNhap.MaHoaDon = maHoaDon;
                    chiTietHoaDonNhap.SoLuong = qlhd.ChiTietHoaDonNhap.SoLuong;
                    chiTietHoaDonNhap.DonViTinh = qlhd.ChiTietHoaDonNhap.DonViTinh;
                    chiTietHoaDonNhap.GiaNhap = qlhd.ChiTietHoaDonNhap.GiaNhap;
                    chiTietHoaDonNhap.TongTien = (qlhd.ChiTietHoaDonNhap.GiaNhap * qlhd.ChiTietHoaDonNhap.SoLuong);

                    shop.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                    sp.SoLuong += qlhd.ChiTietHoaDonNhap.SoLuong;
                    shop.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");
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

                    var ct = shop.ChiTietHoaDonNhaps.SingleOrDefault(c => c.Id == id);
                    var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == ct.MaSanPham);

                    sp.SoLuong = sp.SoLuong - ct.SoLuong;
                    shop.ChiTietHoaDonNhaps.Remove(ct);
                    shop.SaveChanges();
                    return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
           
        }
        [HttpGet]
        public ActionResult ThemSp(int id)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                id_rows = id;
                ViewBag.i = id;

                return this.Json(ViewBag.i, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}