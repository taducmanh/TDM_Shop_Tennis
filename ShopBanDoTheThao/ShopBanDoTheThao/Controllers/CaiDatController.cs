using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class CaiDatController : Controller
    {
        // GET: CaiDat
        public ActionResult Slide()
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
                    ViewData["Slide1"] = shop.Slides.Single(s => s.MaAnh == 1);
                    ViewData["Slide2"] = shop.Slides.Single(s => s.MaAnh == 2);
                    var model = new SlideViewModel()
                    {
                        Slides1 = shop.Slides.ToList()[0],
                        Slides2 = shop.Slides.ToList()[1]
                    };
                    return View(model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Slide");
                }
               

            }
           
        }

        public ActionResult SuaSlide(int id,SlideViewModel model,HttpPostedFileBase file,string hdimg)
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
                    ViewData["Slide1"] = shop.Slides.Single(s => s.MaAnh == 1);
                    ViewData["Slide2"] = shop.Slides.Single(s => s.MaAnh == 2);
                    var mode2l = new SlideViewModel()
                    {
                        Slides1 = shop.Slides.ToList()[0],
                        Slides2 = shop.Slides.ToList()[1]
                    };
                    if (id == 1)
                    {
                        var slide = shop.Slides.Single(s => s.MaAnh == 1);
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/SlideShows/"), fileName);
                                file.SaveAs(path);
                            }

                            slide.TieuDe = model.Slides1.TieuDe;
                            slide.TieuDe1 = model.Slides1.TieuDe1;
                            slide.TieuDe2 = model.Slides1.TieuDe2;
                            slide.MoTa1 = model.Slides1.MoTa1;
                            slide.MoTa2 = model.Slides1.MoTa2;
                            slide.MoTa3 = model.Slides1.MoTa3;
                            slide.MoTa4 = model.Slides1.MoTa4;
                            slide.LinkAnh = "/Images/SlideShows/" + fileName;
                            shop.SaveChanges();
                        }
                        else
                        {
                            slide.TieuDe = model.Slides1.TieuDe;
                            slide.TieuDe1 = model.Slides1.TieuDe1;
                            slide.TieuDe2 = model.Slides1.TieuDe2;
                            slide.MoTa1 = model.Slides1.MoTa1;
                            slide.MoTa2 = model.Slides1.MoTa2;
                            slide.MoTa3 = model.Slides1.MoTa3;
                            slide.MoTa4 = model.Slides1.MoTa4;
                            slide.LinkAnh = hdimg;
                            shop.SaveChanges();
                        }
                    }

                    if (id == 2)
                    {

                        var slide = shop.Slides.Single(s => s.MaAnh == 2);
                        file = file ?? Request.Files["file"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/Images/SlideShows/"), fileName);
                                file.SaveAs(path);
                            }

                            slide.TieuDe = model.Slides2.TieuDe;
                            slide.TieuDe1 = model.Slides2.TieuDe1;
                            slide.TieuDe2 = model.Slides2.TieuDe2;
                            slide.MoTa1 = model.Slides2.MoTa1;
                            slide.MoTa2 = model.Slides2.MoTa2;
                            slide.MoTa3 = model.Slides2.MoTa3;
                            slide.MoTa4 = model.Slides2.MoTa4;
                            slide.LinkAnh = "/Images/SlideShows/" + fileName;
                            shop.SaveChanges();
                        }
                        else
                        {

                            slide.TieuDe = model.Slides2.TieuDe;
                            slide.TieuDe1 = model.Slides2.TieuDe1;
                            slide.TieuDe2 = model.Slides2.TieuDe2;
                            slide.MoTa1 = model.Slides2.MoTa1;
                            slide.MoTa2 = model.Slides2.MoTa2;
                            slide.MoTa3 = model.Slides2.MoTa3;
                            slide.MoTa4 = model.Slides2.MoTa4;
                            slide.LinkAnh = hdimg;
                            shop.SaveChanges();
                        }
                    }
                    
                    return View("Slide", mode2l);
                }
                catch (Exception)
                {

                    return RedirectToAction("Slide");
                }
               
            }
           
        }

        public ActionResult QuangCao()
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
                    var model = new QuangCaoViewModel()
                    {
                        DanhSachQuangCaos = shop.QuangCaos.ToList()
                    };
                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Slide");

                }
              
            }
           
        }

        public ActionResult SuaQuangCao(QuangCaoViewModel model, int id, HttpPostedFileBase file, string img)
        {
            try
            {
                ShopEntities shop = new ShopEntities();
                var qc = shop.QuangCaos.SingleOrDefault(c => c.Id == id);
                file = file ?? Request.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Images/QuangCao/"), fileName);
                        file.SaveAs(path);
                        qc.LinkQuangCao = model.QuangCao.LinkQuangCao;
                        qc.AnhDaiDien = "/Images/QuangCao/" + fileName;
                        qc.MoTa = model.QuangCao.MoTa;
                        shop.SaveChanges();
                    }

                }
                else
                {
                    qc.LinkQuangCao = model.QuangCao.LinkQuangCao;
                    qc.AnhDaiDien = img;
                    qc.MoTa = model.QuangCao.MoTa;
                    shop.SaveChanges();

                }
                var data = new QuangCaoViewModel()
                {
                    DanhSachQuangCaos = shop.QuangCaos.ToList()
                };
                return View("QuangCao", data);
            }
            catch (Exception)
            {

                return RedirectToAction("Slide");
            }
            
        }

        public ActionResult HeThong()
        {
            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ShopEntities shop=new ShopEntities();
                var model = shop.CaiDats.ToList().Single();
                return View(model);
            }

        }

        public ActionResult SuaCaiDat(CaiDat cd,HttpPostedFileBase file,string img)
        {
            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ShopEntities shop = new ShopEntities();
                var caiDat = shop.CaiDats.SingleOrDefault(c => c.Id == 1);
                file = file ?? Request.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(path);

                        caiDat.Logo = "/Images/" + fileName;
                        caiDat.GioLamViec = cd.GioLamViec;
                        caiDat.GiaoHang = cd.GiaoHang;
                        caiDat.HoanTien = cd.HoanTien;
                        caiDat.SDTLienHe = cd.SDTLienHe;
                        caiDat.EmailLienHe = cd.EmailLienHe;
                        caiDat.FaceBook = cd.FaceBook;
                        caiDat.GooglePlus = cd.GooglePlus;
                        caiDat.Twiter = cd.Twiter;
                        caiDat.YouTube = cd.YouTube;
                        caiDat.Instargram = cd.Instargram;
                        caiDat.GoogleMap = cd.GoogleMap;
                        caiDat.MatKhauMail = cd.MatKhauMail;
                        shop.SaveChanges();
                    }
                }
                else
                {
                    caiDat.Logo = img;
                    caiDat.GioLamViec = cd.GioLamViec;
                    caiDat.GiaoHang = cd.GiaoHang;
                    caiDat.HoanTien = cd.HoanTien;
                    caiDat.SDTLienHe = cd.SDTLienHe;
                    caiDat.EmailLienHe = cd.EmailLienHe;
                    caiDat.FaceBook = cd.FaceBook;
                    caiDat.GooglePlus = cd.GooglePlus;
                    caiDat.Twiter = cd.Twiter;
                    caiDat.YouTube = cd.YouTube;
                    caiDat.Instargram = cd.Instargram;
                    caiDat.GoogleMap = cd.GoogleMap;
                    caiDat.MatKhauMail = cd.MatKhauMail;
                    shop.SaveChanges();
                }

                var model = shop.CaiDats.ToList().Single();
                return View("HeThong", model);
            }
        }
    }
}