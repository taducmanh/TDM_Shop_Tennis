using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLyDonHangController : Controller
    {
        // GET: AdminQuanLyDonHang
        [HttpGet]
        public ActionResult DonHangChuaDuyet(string searchString, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTao(searchString, page)
                    };

                    return View(model);
                }
                catch (Exception)
                {
                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
               
            }
          
        }
        [HttpGet]
        public ActionResult DonHangDaDuyet(string searchString, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTaoDSDonHangDaDuyet(searchString, page)
                    };

                    return View(model);
                }
                catch (Exception)
                {

                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTaoDSDonHangDaDuyet(searchString, page)
                    };

                    return View(model);
                }
               
            }
           
        }
        public IPagedList<HoaDon> KhoiTaoDSDonHangDaDuyet(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();

            var dsDonHangDaDuyet = shop.HoaDons.Where(hd => hd.TrangThai == true).OrderByDescending(s => s.MaHoaDon);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    dsDonHangDaDuyet = shop.HoaDons.Where(hd => hd.TrangThai == true&&hd.(searchString)).OrderByDescending(s => s.MaSanPham);
            //}

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsDonHangDaDuyet.ToPagedList(pageNumber, pageSize);
            return dsDonHangDaDuyet.ToPagedList(pageNumber, pageSize);
        }
        public IPagedList<HoaDon> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();

            var dsDonHangChuaDuyet = shop.HoaDons.Where(hd=>hd.TrangThai==false).OrderByDescending(s => s.MaHoaDon);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsDonHangChuaDuyet.ToPagedList(pageNumber, pageSize);
            return dsDonHangChuaDuyet.ToPagedList(pageNumber, pageSize);
        }
        public IPagedList<ChiTietHoaDon> KhoiTaoChitietHoaDon(int id, int? page)
        {  
            ShopEntities shop = new ShopEntities();
            var hoaDon = shop.HoaDons.Single(h => h.MaHoaDon == id);
            if (hoaDon.TrangThai == false)
            {
                ViewBag.Url = "/AdminQuanLyDonHang/DonHangChuaDuyet";
            }
            else
            {
                ViewBag.Url = "/AdminQuanLyDonHang/DonHangDaDuyet";
            }
            var dsChiTiet = shop.ChiTietHoaDons.Where(c => c.MaHoaDon == id).OrderByDescending(s => s.MaHoaDon); ;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsChiTiet.ToPagedList(pageNumber, pageSize);   
            return dsChiTiet.ToPagedList(pageNumber, pageSize);
        }
        public ActionResult DonHangDaDuyet()
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult XoaDonHang(int id,int loai)
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
                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTao("", 1)
                    };

                    var hoaDon = shop.HoaDons.Single(s => s.MaHoaDon == id);
                    var dsChiTiet = shop.ChiTietHoaDons.Where(c => c.MaHoaDon == id).ToList();

                    if (loai == 0)
                    {
                        foreach (var item in dsChiTiet)
                        {
                            shop.ChiTietHoaDons.Remove(item);
                        }
                        shop.HoaDons.Remove(hoaDon);
                        shop.SaveChanges();
                        return View("DonHangChuaDuyet", model);
                    }
                    else
                    {
                        foreach (var item in dsChiTiet)
                        {
                            var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == item.MaSanPham);
                            sp.SoLuong -= item.SoLuong;

                            shop.ChiTietHoaDons.Remove(item);
                            shop.SaveChanges();
                        }
                        shop.HoaDons.Remove(hoaDon);
                        shop.SaveChanges();

                        return View("DonHangDaDuyet", model);
                    }
                   
                }
                catch (Exception)
                {
                    return View("DonHangChuaDuyet");
                }
                
            }
           
        }
        public ActionResult DuyetDonHang(int id)
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
                    var model = new QuanLyDonHang()
                    {
                        DanhSachHoaDon = KhoiTao("", 1)
                    };
                    var hoaDon = shop.HoaDons.Single(s => s.MaHoaDon == id);
                    hoaDon.TrangThai = true;
                    hoaDon.NgayDuyet = DateTime.Now;
                    shop.SaveChanges();
                    var list = shop.ChiTietHoaDons.Where(c => c.MaHoaDon == id).ToList();
                    foreach (var item in list)
                    {
                        var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == item.MaSanPham);
                        sp.SoLuong += item.SoLuong;
                        shop.SaveChanges();
                    }

                   
                    return View("DonHangChuaDuyet", model);
                }
                catch (Exception)
                {
                    return View("DonHangChuaDuyet");
                }
               
            }
           
        }
        [HttpGet]
        public ActionResult ChiTietDonHang(int id, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    ViewBag.id = id;
                    ShopEntities shop = new ShopEntities();
                    var model = new QuanLyDonHang()
                    {
                        DanhSachChiTiet = KhoiTaoChitietHoaDon(id, page),
                        HoaDon = shop.HoaDons.SingleOrDefault(h => h.MaHoaDon == id)
                    };

                    return View(model);
                }
                catch (Exception)
                {
                    return View("ChiTietDonHang");

                }
                
            }
           
        }
    }
}