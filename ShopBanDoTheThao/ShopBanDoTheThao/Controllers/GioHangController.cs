using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
     
        public ActionResult XoaSanPham(int id)
        {
            ShoppingCart objCart = (ShoppingCart)Session["Cart"];
            if (objCart != null)
            {
                objCart.RemoveFromCart(id);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }
        // thêm vào giỏ hàng 1 sản phẩm có id = id của sản phẩm
        [HttpPost]
        public ActionResult ThemVaoGioHang(int id,int soLuong)
        {
          
                ShopEntities db = new ShopEntities();
                var p = db.SanPhams.SingleOrDefault(s => s.MaSanPham.Equals(id));
                decimal?gia = 0;
                gia = p.GiaGiam > 0 ? p.GiaGiam : p.Gia;

                if (p != null)
                {
                    ShoppingCart objCart = (ShoppingCart)Session["Cart"];
                    if (objCart == null)
                    {
                        objCart = new ShoppingCart();
                    }
                    ShoppingCart.ShoppingCartItem item = new ShoppingCart.ShoppingCartItem()
                    {
                        Image = p.AnhDaiDien,
                        ProductName = p.TenSanPham,
                        ProductID = p.MaSanPham,
                      
                        Price = gia.ToString(),
                        Quanlity = soLuong,
                        Total = Convert.ToDouble(gia.ToString().Trim().Replace(",", string.Empty).Replace(".", string.Empty)) * soLuong
                    };
                    objCart.AddToCart(item);
                    Session["Cart"] = objCart;
               
            }
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };         
            var result = JsonConvert.SerializeObject("Thêm thành công", Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet); ;
          
        }
        // cập nhật giỏ hàng theo loại sản phẩm và số lượng
      
        public ActionResult UpdateQuantity(string proID, int quantity)
        {
            int id = Convert.ToInt32(proID.Substring(7, proID.Length - 7));
            ShoppingCart objCart = (ShoppingCart)Session["Cart"];
            if (objCart != null)
            {
                objCart.UpdateQuantity(id, quantity);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }
        // giỏ hàng mặc định, nếu session = null thì hiện không có hàng trong giỏ, nếu có thì trả lại list các sản phẩm
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];
            return View(model);
        }
        [HttpPost]
        public ActionResult ThanhToan(HoaDon hoadon)
        {
            ShopEntities shop=new ShopEntities();
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart) Session["Cart"];
            decimal tong = model.Cart.ListItem.Sum(item => decimal.Parse(item.Price)*item.Quanlity);
            HoaDon hd=new HoaDon();
            hd.TenKH = hoadon.TenKH;
            hd.GioiTinh = hoadon.GioiTinh;
            hd.DiaChi = hoadon.DiaChi;
            hd.Email = hoadon.Email;
            hd.SDT = hoadon.SDT;
            hd.DiaChiGiaoHang = hoadon.DiaChiGiaoHang;
            hd.ThoiGianGiaoHang = hoadon.ThoiGianGiaoHang;
            hd.NgayTao = DateTime.Now;
            hd.TongGia = tong;
            hd.TrangThai = false;
            shop.HoaDons.Add(hd);
            shop.SaveChanges();

            KhachHang kh=new KhachHang();
            kh.TenKH = hoadon.TenKH;
            kh.GioiTinh = hoadon.GioiTinh;
            kh.DiaChi = hoadon.DiaChi;
            kh.Email = hoadon.Email;
            kh.SDT = hoadon.SDT;
            shop.KhachHangs.Add(kh);
            shop.SaveChanges();
            var hoaDon = (from h in shop.HoaDons orderby h.MaHoaDon descending select h).FirstOrDefault();
            foreach (var item in model.Cart.ListItem)
            {
                ChiTietHoaDon ct=new ChiTietHoaDon();
                ct.MaHoaDon = hoaDon.MaHoaDon;
                ct.MaSanPham = item.ProductID;
                ct.SoLuong = item.Quanlity;
                ct.TongGia = decimal.Parse(item.Total.ToString(CultureInfo.InvariantCulture)) ;
                shop.ChiTietHoaDons.Add(ct);
                shop.SaveChanges();
            }
            model.Cart.ListItem.Clear();
            return View();
        }

        public ActionResult DienThongTin()
        {
            return View();
        }
    }
}