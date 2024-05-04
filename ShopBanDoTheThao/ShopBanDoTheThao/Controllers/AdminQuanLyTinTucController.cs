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
    public class AdminQuanLyTinTucController : Controller
    {
        // GET: AdminQuanLyTinTuc
        private static int maTin = 0;
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
                    QuanLyTinTuc model = new QuanLyTinTuc()
                    {
                        DanhSachTinTuc = KhoiTao(searchString, page)
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
        [ValidateInput(false)]
        public ActionResult ThemTinTuc(QuanLyTinTuc qltt, HttpPostedFileBase file, string submit, int id, string hdimg)
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
                                var path = Path.Combine(Server.MapPath("~/Images/TinTuc/"), fileName);
                                file.SaveAs(path);
                            }
                            TinTuc tt = new TinTuc();
                            tt.TieuDe = qltt.TinTuc.TieuDe;
                            tt.AnhDaiDien = "/Images/TinTuc/" + fileName;
                            tt.MoTa = qltt.TinTuc.MoTa;
                            tt.NgayTao = DateTime.Now;
                            tt.ChiTiet = qltt.TinTuc.ChiTiet;
                            tt.LuotXem = 0;
                            shop.TinTucs.Add(tt);
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
                                var path = Path.Combine(Server.MapPath("~/Images/TinTuc/"), fileName);
                                file.SaveAs(path);
                                var tt = shop.TinTucs.Single(s => s.MaTinTuc == id);
                                tt.TieuDe = qltt.TinTuc.TieuDe;
                                tt.AnhDaiDien = "/Images/TinTuc/" + fileName;
                                tt.MoTa = qltt.TinTuc.MoTa;
                                tt.ChiTiet = qltt.TinTuc.ChiTiet;
                                shop.SaveChanges();
                            }
                        }
                        else
                        {
                            var tt = shop.TinTucs.Single(s => s.MaTinTuc == id);
                            tt.TieuDe = qltt.TinTuc.TieuDe;
                            tt.AnhDaiDien = hdimg;
                            tt.MoTa = qltt.TinTuc.MoTa;
                            tt.ChiTiet = qltt.TinTuc.ChiTiet;
                            shop.SaveChanges();
                        }

                    }
                    // KhoiTao();
                    QuanLyTinTuc model = new QuanLyTinTuc()
                    {
                        DanhSachTinTuc = KhoiTao("", 1)
                    };
                    return View("Index", model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
               
            }
           
        }
       [HttpPost]
        public ActionResult XoaTin(int id)
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
                    TinTuc tin = shop.TinTucs.SingleOrDefault(t => t.MaTinTuc == id);
                    shop.TinTucs.Remove(tin);
                    shop.SaveChanges();
                    return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }
        public IPagedList<TinTuc> KhoiTao(string searchString, int? page)
        {

            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();          
            var dsTinTuc = shop.TinTucs.OrderByDescending(s => s.MaTinTuc);
            if (!String.IsNullOrEmpty(searchString))
            {
                dsTinTuc = shop.TinTucs.Where(s => s.TieuDe.Contains(searchString)).OrderByDescending(s => s.MaTinTuc);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            dsTinTuc.ToPagedList(pageNumber, pageSize);
            return dsTinTuc.ToPagedList(pageNumber, pageSize);
        }
        [HttpGet]
        public ActionResult LayTinTuc(int matin)
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
                    JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                    TinTuc tt = shop.TinTucs.Single(t => t.MaTinTuc == matin);
                    var result = JsonConvert.SerializeObject(tt, Formatting.Indented, jss);
                    return this.Json(result, JsonRequestBehavior.AllowGet); ;
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }
                
            }
           
        }
       

      
        
    }
}