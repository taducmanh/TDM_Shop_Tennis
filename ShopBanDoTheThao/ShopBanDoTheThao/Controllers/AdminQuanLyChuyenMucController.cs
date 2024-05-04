using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminQuanLyChuyenMucController : Controller
    {
        // GET: AdminQuanLyChuyenMuc
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
                    var model = new QuanLyChuyenMuc()
                    {
                        DanhSachChuyenMuc = KhoiTao(searchString, page)
                    };
                    return View(model);
                }
                catch (Exception ex)
                {
                    var model = new QuanLyChuyenMuc()
                    {
                        DanhSachChuyenMuc = KhoiTao("", 1)
                    };
                    return View(model);

                }
               
            }
           

           
        }

        public IPagedList<ChuyenMuc> KhoiTao(string searchString, int? page)
        {
           
            ViewBag.CurrentFilter = searchString;
            ShopEntities shop = new ShopEntities();

            var dsChuyenMucCha = shop.ChuyenMucs.Where(c => c.MaChuyenMucCha == null ).ToList();
            List<SelectListItem> cmCha = new List<SelectListItem>();

            cmCha.Add(new SelectListItem() { Text = "KHÔNG CÓ CHUYÊN MỤC CHA", Value = null });
          
            for (int i = 0; i < dsChuyenMucCha.Count; i++)
            {
                if (dsChuyenMucCha[i].DacBiet == true)
                {
                    SelectListItem sl = new SelectListItem() { Text = dsChuyenMucCha[i].TenChuyenMuc+"(Đặc biệt)", Value = dsChuyenMucCha[i].MaChuyenMuc.ToString() };
                    cmCha.Add(sl);
                }
                else
                {
                    SelectListItem sl = new SelectListItem() { Text = dsChuyenMucCha[i].TenChuyenMuc, Value = dsChuyenMucCha[i].MaChuyenMuc.ToString() };
                    cmCha.Add(sl);
                }
               
                
            }
            ViewBag.CM = cmCha;
            var dsChuyenMuc = shop.ChuyenMucs.Where(c=>c.MaChuyenMucCha==null).OrderByDescending(s => s.MaChuyenMuc);
            if (!String.IsNullOrEmpty(searchString))
            {
                dsChuyenMuc = shop.ChuyenMucs.Where(s => s.TenChuyenMuc.Contains(searchString)).OrderByDescending(s => s.MaChuyenMuc);
            }
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            dsChuyenMuc.ToPagedList(pageNumber, pageSize);

            return dsChuyenMuc.ToPagedList(pageNumber, pageSize);
        }
        [ValidateInput(false)]
        public ActionResult ThemChuyenMucChinh(QuanLyChuyenMuc qlcm,string submit,int id)
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
                    var model = new QuanLyChuyenMuc()
                    {
                        DanhSachChuyenMuc = KhoiTao("", 1)
                    };
                    if (submit == "Thêm")
                    {
                        ChuyenMuc cm = new ChuyenMuc();
                        cm.TenChuyenMuc = qlcm.ChuyenMuc.TenChuyenMuc;
                        cm.MaChuyenMucCha = qlcm.ChuyenMuc.MaChuyenMucCha;

                        cm.DacBiet = qlcm.ChuyenMuc.DacBiet;
                        if (qlcm.ChuyenMuc.DacBiet == true)
                        {
                            cm.NoiDung = qlcm.ChuyenMuc.NoiDung;
                        }
                        shop.ChuyenMucs.Add(cm);
                        shop.SaveChanges();
                    }
                    if (submit == "Sửa")
                    {
                        ChuyenMuc cm = shop.ChuyenMucs.SingleOrDefault(c => c.MaChuyenMuc == id);

                        cm.TenChuyenMuc = qlcm.ChuyenMuc.TenChuyenMuc;
                        cm.MaChuyenMucCha = qlcm.ChuyenMuc.MaChuyenMucCha;
                        cm.DacBiet = qlcm.ChuyenMuc.DacBiet;
                        if (qlcm.ChuyenMuc.DacBiet == true)
                        {
                            cm.NoiDung = qlcm.ChuyenMuc.NoiDung;
                        }
                        shop.SaveChanges();


                    }

                    return RedirectToAction("Index", model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
                
            }
            
        }
        [HttpGet]
        public ActionResult LayChuyenMuc(int id)
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

                    ChuyenMuc cm = shop.ChuyenMucs.SingleOrDefault(c => c.MaChuyenMuc == id);
                    var result = JsonConvert.SerializeObject(cm, Formatting.Indented, jss);
                    return this.Json(result, JsonRequestBehavior.AllowGet); ;
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
          
        }
        public ActionResult XoaChuyenMuc(int id)
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
                    var model = new QuanLyChuyenMuc()
                    {
                        DanhSachChuyenMuc = KhoiTao("", 1)
                    };
                    ChuyenMuc cm = shop.ChuyenMucs.SingleOrDefault(c => c.MaChuyenMuc == id);
                    var dsSanPham = shop.SanPhams.Where(s => s.MaChuyenMuc == id);
                    foreach (var item in dsSanPham)
                    {
                        shop.SanPhams.Remove(item);
                        shop.SaveChanges();
                    }
                    shop.ChuyenMucs.Remove(cm);
                    shop.SaveChanges();
                    return RedirectToAction("Index", model);
                }
                catch (Exception)
                {

                    return RedirectToAction("Index");
                }
               
            }
           
        }
    }
}