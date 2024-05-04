using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLyNhaPhanPhoiController : Controller
    {
        // GET: AdminQuanLyNhaPhanPhoi
        private static int maNPP=0;
      
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
                    var model = new QuanLyNhaPhanPhoi()
                    {
                        DanhSachNhaPhanPhoi = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               

            }
          
        }
      
        public IPagedList<NhaPhanPhois> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();
            var dsNhaPP = shop.NhaPhanPhois.ToList();
          
            var dsSanPham = shop.SanPhams.ToList();
            List<SelectListItem> slSP = new List<SelectListItem>();

            for (int i = 0; i < dsSanPham.Count; i++)
            {
                SelectListItem sl = new SelectListItem() { Text = dsSanPham[i].TenSanPham, Value = dsSanPham[i].MaSanPham.ToString() };
                slSP.Add(sl);
            }
            ViewBag.SP = slSP;
          
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsNhaPP.ToPagedList(pageNumber, pageSize);
            return dsNhaPP.ToPagedList(pageNumber, pageSize);
        }

        public ActionResult ThemNhaPhanPhoi(QuanLyNhaPhanPhoi qlnpp, string submit, int id)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    var model = new QuanLyNhaPhanPhoi()
                    {
                        DanhSachNhaPhanPhoi = KhoiTao("", 1)
                    };
                    ShopEntities shop = new ShopEntities();

                    if (submit == "Thêm")
                    {
                        NhaPhanPhois npp = new NhaPhanPhois();
                        npp.TenNhaPhanPhoi = qlnpp.NhaPhanPhois.TenNhaPhanPhoi;
                        npp.DiaChi = qlnpp.NhaPhanPhois.DiaChi;
                        npp.SoDienThoai = qlnpp.NhaPhanPhois.SoDienThoai;
                        npp.Fax = qlnpp.NhaPhanPhois.Fax;
                        npp.MoTa = qlnpp.NhaPhanPhois.MoTa;


                        shop.NhaPhanPhois.Add(npp);
                        shop.SaveChanges();
                    }
                    if (submit == "Sửa")
                    {
                        var npp = shop.NhaPhanPhois.SingleOrDefault(n => n.MaNhaPhanPhoi == id);
                        npp.TenNhaPhanPhoi = qlnpp.NhaPhanPhois.TenNhaPhanPhoi;
                        npp.DiaChi = qlnpp.NhaPhanPhois.DiaChi;
                        npp.SoDienThoai = qlnpp.NhaPhanPhois.SoDienThoai;
                        npp.Fax = qlnpp.NhaPhanPhois.Fax;
                        npp.MoTa = qlnpp.NhaPhanPhois.MoTa;
                        shop.SaveChanges();
                    }
                    
                    return View("Index", model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
           
        }
        [HttpPost]
        public ActionResult XoaNhaPhanPhoi(int id)
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
                    var npp = shop.NhaPhanPhois.SingleOrDefault(n => n.MaNhaPhanPhoi == id);

                    var ds = shop.SanPhams.Where(s => s.NhaPhanPhois.Any(e => e.MaNhaPhanPhoi == id)).ToList();
                    foreach (var item in ds)
                    {
                        npp.SanPhams.Remove(item);
                        shop.SaveChanges();
                    }
                    shop.NhaPhanPhois.Remove(npp);
                    shop.SaveChanges();
                    return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }
        [HttpPost]
        public ActionResult XoaSanPhamPhanPhoi(int id)
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
                    var npp = shop.NhaPhanPhois.SingleOrDefault(n => n.MaNhaPhanPhoi == maNPP);

                    var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == id);
                    npp.SanPhams.Remove(sp);
                    shop.SaveChanges();
                    return Json(new { dt = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
               
            }
          
        }
        public ActionResult SanPhamPhanPhoi(int id, string searchString, int? page)
        {
            if (Session["MaTKAdmin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    maNPP = id;
                    var model = new QuanLyNhaPhanPhoi()
                    {
                        DanhSachSanPhamPhanPhoi = KhoiTaoDanhSachSanPhamPhanPhoi(id, searchString, page)
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
        public ActionResult ThemSanPhamPhanPhoi(string sppp)
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
                    string[] dsSanPham = sppp.Split(',');

                    var npp = shop.NhaPhanPhois.SingleOrDefault(n => n.MaNhaPhanPhoi == maNPP);
                    if (dsSanPham.Length > 0)
                    {
                        foreach (string t in dsSanPham)
                        {
                            var id = int.Parse(t);
                            var sp = shop.SanPhams.SingleOrDefault(s => s.MaSanPham == id);
                            npp.SanPhams.Add(sp);
                            shop.SaveChanges();
                        }

                    }
                    //Response.Redirect("SanPhamPhanPhoi/?id=" + maNPP + "");
                    //return View("SanPhamPhanPhoi");
                    return Json(new { dt = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
          
        }

        public IPagedList<SanPham> KhoiTaoDanhSachSanPhamPhanPhoi(int id,string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();
            var npp = shop.NhaPhanPhois.Single(n=>n.MaNhaPhanPhoi==id);
           
            var dsSanPhamDangPhanPhoi = npp.SanPhams.Where(s => s.NhaPhanPhois.Any(e => e.MaNhaPhanPhoi == id)).ToList();
            var dsSanPhamChuaPhanPhoi = shop.SanPhams.ToList();
            foreach (var item in dsSanPhamDangPhanPhoi)
            {
                dsSanPhamChuaPhanPhoi.Remove(item);
            }
            List<SelectListItem> slSP = new List<SelectListItem>();

            for (int i = 0; i < dsSanPhamChuaPhanPhoi.Count; i++)
            {
                SelectListItem sl = new SelectListItem() { Text = dsSanPhamChuaPhanPhoi[i].TenSanPham, Value = dsSanPhamChuaPhanPhoi[i].MaSanPham.ToString() };
                slSP.Add(sl);
            }
            ViewBag.SP = slSP;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsSanPhamDangPhanPhoi.ToPagedList(pageNumber, pageSize);
            return dsSanPhamDangPhanPhoi.ToPagedList(pageNumber, pageSize);
        }
    }
}