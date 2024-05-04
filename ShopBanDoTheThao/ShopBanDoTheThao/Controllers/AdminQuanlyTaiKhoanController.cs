using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanlyTaiKhoanController : Controller
    {
        // GET: AdminQuanlyTaiKhoan
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
                    var model = new QuanLyTaiKhoan()
                    {
                        DanhSachTaiKhoan = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
               
            }
           
        }

        public IPagedList<TaiKhoan> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();
            var dsLoaiTaiKhoan = shop.LoaiTaiKhoans.ToList();
            List<SelectListItem> slLoaiTK = new List<SelectListItem>();

          
            for (int i = 0; i < dsLoaiTaiKhoan.Count; i++)
            {
                SelectListItem sl = new SelectListItem() { Text = dsLoaiTaiKhoan[i].TenLoai, Value = dsLoaiTaiKhoan[i].MaLoai.ToString() };
                slLoaiTK.Add(sl);
            }
            ViewBag.LTK = slLoaiTK;

            var dsTaiKhoan = shop.TaiKhoans.OrderByDescending(s => s.MaTaiKhoan);
            if (!String.IsNullOrEmpty(searchString))
            {
                dsTaiKhoan = shop.TaiKhoans.Where(s => s.TenTaiKhoan.Contains(searchString)).OrderByDescending(s => s.MaTaiKhoan);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsTaiKhoan.ToPagedList(pageNumber, pageSize);
          
            return dsTaiKhoan.ToPagedList(pageNumber, pageSize);
        }
      [HttpGet]
        public ActionResult LayChiTietTaiKhoan(int matk)
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
                    //JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                    ChiTietTaiKhoan ct = shop.ChiTietTaiKhoans.Single(t => t.MaTaiKhoan == matk);
                    //var result = JsonConvert.SerializeObject(ct, Formatting.Indented, jss);
                    return this.Json(new { hoten = ct.HoTen, diachi = ct.DiaChi, sdt = ct.SoDienThoai, anh = ct.AnhDaiDien }, JsonRequestBehavior.AllowGet); ;
                }
                catch (Exception)
                {
                    return RedirectToAction("Index"); 
                }
               
            }
          
        }
        public ActionResult ThemTaiKhoan(QuanLyTaiKhoan qltk, string submit,int id, HttpPostedFileBase file, string img)
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
                        shop.TaiKhoans.Add(qltk.TaiKhoan);
                        shop.SaveChanges();

                        var tkMoi = (from s in shop.TaiKhoans orderby s.MaTaiKhoan descending select s).FirstOrDefault();
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/Avatars/"), fileName);
                                file.SaveAs(path);
                            }
                            ChiTietTaiKhoan ct = new ChiTietTaiKhoan();
                            ct.MaTaiKhoan = tkMoi.MaTaiKhoan;
                            ct.HoTen = qltk.ChiTietTaiKhoan.HoTen;
                            ct.DiaChi = qltk.ChiTietTaiKhoan.DiaChi;
                            ct.SoDienThoai = qltk.ChiTietTaiKhoan.SoDienThoai;
                            ct.AnhDaiDien = "/Images/Avatars/" + fileName;
                            shop.ChiTietTaiKhoans.Add(ct);
                            shop.SaveChanges();
                        }
                        else
                        {
                            ChiTietTaiKhoan ct = new ChiTietTaiKhoan();
                            ct.MaTaiKhoan = tkMoi.MaTaiKhoan;
                            ct.HoTen = qltk.ChiTietTaiKhoan.HoTen;
                            ct.DiaChi = qltk.ChiTietTaiKhoan.DiaChi;
                            ct.SoDienThoai = qltk.ChiTietTaiKhoan.SoDienThoai;
                            ct.AnhDaiDien = "";
                            shop.ChiTietTaiKhoans.Add(ct);
                            shop.SaveChanges();
                        }

                    }
                    if (submit == "Sửa")
                    {

                        TaiKhoan tk = shop.TaiKhoans.SingleOrDefault(t => t.MaTaiKhoan == id);
                        tk.LoaiTaiKhoan = qltk.TaiKhoan.LoaiTaiKhoan;
                        tk.TenTaiKhoan = qltk.TaiKhoan.TenTaiKhoan;
                        tk.MatKhau = qltk.TaiKhoan.MatKhau;
                        tk.Email = qltk.TaiKhoan.Email;

                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/Avatars/"), fileName);
                                file.SaveAs(path);
                                ChiTietTaiKhoan ct = shop.ChiTietTaiKhoans.SingleOrDefault(c => c.MaTaiKhoan == id);
                                ct.HoTen = qltk.ChiTietTaiKhoan.HoTen;
                                ct.DiaChi = qltk.ChiTietTaiKhoan.DiaChi;
                                ct.AnhDaiDien = "/Images/Avatars/" + fileName;
                                ct.SoDienThoai = qltk.ChiTietTaiKhoan.SoDienThoai;
                                shop.SaveChanges();
                            }


                        }
                        else
                        {
                            ChiTietTaiKhoan ct = shop.ChiTietTaiKhoans.SingleOrDefault(c => c.MaTaiKhoan == id);
                            ct.HoTen = qltk.ChiTietTaiKhoan.HoTen;
                            ct.DiaChi = qltk.ChiTietTaiKhoan.DiaChi;
                            ct.AnhDaiDien = img;
                            ct.SoDienThoai = qltk.ChiTietTaiKhoan.SoDienThoai;
                            shop.SaveChanges();
                        }

                    }
                    var model = new QuanLyTaiKhoan()
                    {
                        DanhSachTaiKhoan = KhoiTao("", 1)
                    };

                    return RedirectToAction("Index", model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
                
            }
           
        }
        [HttpPost]
        public ActionResult XoaTaiKhoan(int id)
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
                    ChiTietTaiKhoan ct = shop.ChiTietTaiKhoans.SingleOrDefault(c => c.MaTaiKhoan == id);
                    shop.ChiTietTaiKhoans.Remove(ct);
                    shop.SaveChanges();
                    TaiKhoan tk = shop.TaiKhoans.SingleOrDefault(t => t.MaTaiKhoan == id);
                    shop.TaiKhoans.Remove(tk);
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