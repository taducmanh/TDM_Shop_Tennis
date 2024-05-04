using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        public TaiKhoanController()
        {
            
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(TaiKhoan tk)
        {
            string email = tk.Email;
            ShopEntities shop = new ShopEntities();
            var taiKhoan = shop.TaiKhoans.SingleOrDefault(t => t.Email.Trim().ToLower() == email.Trim().ToLower());
     
            if (ModelState.IsValid)
            {
                if (taiKhoan != null)
                {
                    Session["MaTK"] = taiKhoan.MaTaiKhoan;
                    Session["TenTaiKhoan"] = taiKhoan.TenTaiKhoan;
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                }
            }
           
            return View(taiKhoan);
        }
        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKi(TaiKhoan tk)
        {
            ShopEntities shop = new ShopEntities();

            if (ModelState.IsValid)
            {
                var tkk = shop.TaiKhoans.SingleOrDefault(t => t.Email.Trim().ToLower() == tk.Email.Trim().ToLower());

            if (tkk != null)
                {  ModelState.AddModelError("", "Email đã tồn tại");}
            else
            {
                    if (KiemTraEmail(tk.Email) == false)
                    {   ModelState.AddModelError("", "Email không đúng định dạng");}
                    else
                    {
                        TaiKhoan taiKhoan = new TaiKhoan();
                        taiKhoan.LoaiTaiKhoan = 2;
                        taiKhoan.TenTaiKhoan = tk.TenTaiKhoan;
                        taiKhoan.Email = tk.Email;
                        taiKhoan.MatKhau = tk.MatKhau;
                        shop.TaiKhoans.Add(taiKhoan);
                        shop.SaveChanges();
                    }
                   

                }
            }
            return View();
        }
        private static bool KiemTraEmail(string emailAddress)
        {
            return new System.ComponentModel.DataAnnotations
                                .EmailAddressAttribute()
                                .IsValid(emailAddress);
        }
        public ActionResult QuenMatKhau(TaiKhoan tk)
        {
            return View();
        }
       
        public ActionResult DangXuat()
        {
            Session["MaTK"] = null;
            Session["TenTaiKhoan"] =null;
            return Redirect("/");
        }

    }
}