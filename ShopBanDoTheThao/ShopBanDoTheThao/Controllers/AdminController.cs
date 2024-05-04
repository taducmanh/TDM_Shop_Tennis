using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ShopBanDoTheThao.Models;

namespace ShopBanDoTheThao.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private static int _maTk = 0;
        public ActionResult Index(string chonngay)
        {
            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {

                int thanghientai = DateTime.Now.Month;
                int thangTruoc = DateTime.Now.Month-1;
                ShopEntities shop = new ShopEntities();
                var model = new ThongKeViewModel();
                var listXemNhieu = new List<SanPham>();
                var hoaDonChuaDuyet = new List<HoaDon>();
                var dsDonHangChuaDuyet = new List<HoaDon>();
                var hoaDonDaDuyet = new List<HoaDon>();
                var dsDonHangDaDuyet = new List<HoaDon>();
            
                var listTin = new List<TinTuc>();
                
                listXemNhieu = shop.SanPhams.OrderByDescending(s => s.LuotXem).Skip(0).Take(5).ToList();
                hoaDonChuaDuyet = shop.HoaDons.Where(h => h.TrangThai == false).ToList();
                dsDonHangChuaDuyet = (from item in hoaDonChuaDuyet
                                      let ngaytao = DateTime.Parse(item.NgayTao.ToString()).Month
                                      where ngaytao == thanghientai
                                      select item).ToList();
                hoaDonDaDuyet = shop.HoaDons.Where(h => h.TrangThai == true).ToList();
                dsDonHangDaDuyet = (from item in hoaDonDaDuyet
                                    let ngayduyet = DateTime.Parse(item.NgayDuyet.ToString()).Month
                                    where ngayduyet == thanghientai
                                    select item).ToList();
           
                listTin = shop.TinTucs.ToList();
                var dsTinTuc = (from item in listTin
                                let ngaytao = DateTime.Parse(item.NgayTao.ToString()).Month
                                where ngaytao == thanghientai
                                select item).ToList();

                var dsSanPhamTonKho = shop.SanPhams.Where(s=>s.SoLuong>0).ToList();
                decimal?tongGiaTriTonKho = dsSanPhamTonKho.Aggregate<SanPham, decimal?>(0, (current, item) => current + item.SoLuong*item.Gia);
                model.DanhSachSanPhamBanNhieu = SanPhamBanChay();
                model.TongGiaTriHoaDonDaDuyet = GiaTriHoaDon(dsDonHangDaDuyet);
                model.TongGiaTriHoaDonChuaDuyet = GiaTriHoaDon(dsDonHangChuaDuyet);
                model.DanhSachSanPhamXemNhieu = listXemNhieu;
                model.DanhSachHoaDonChuaDuyet = dsDonHangChuaDuyet;
                model.DanhSachHoaDonDaDuyet = dsDonHangDaDuyet;
                model.TongThuNhap = GiaTriHoaDon(dsDonHangDaDuyet) - GiaTriHoaDonNhap();
                model.TongNhapHang= GiaTriHoaDonNhap();
                model.DanhSachTinTucs = dsTinTuc;
                model.DanhSachSanPhamTonKho = dsSanPhamTonKho;
                model.TongGiaTriTonKho = (decimal)tongGiaTriTonKho;

                if (!String.IsNullOrEmpty(chonngay))
                {
                    string[] array = chonngay.Split('-');
                    var ngaybatdau = DateTime.Parse(array[0].ToString());
                    var ngayketthuc = DateTime.Parse(array[1].ToString());

                    listXemNhieu = shop.SanPhams.OrderByDescending(s => s.LuotXem).Skip(0).Take(5).ToList();

                    hoaDonChuaDuyet = shop.HoaDons.Where(h => h.TrangThai == false).ToList();
                    dsDonHangChuaDuyet = (from item in hoaDonChuaDuyet
                                          let ngaytao = DateTime.Parse(item.NgayTao.ToString())
                                          where ngaytao >= ngaybatdau && ngaytao <= ngayketthuc
                                          select item).ToList();

                    hoaDonDaDuyet = shop.HoaDons.Where(h => h.TrangThai == true).ToList();
                    dsDonHangDaDuyet = (from item in hoaDonDaDuyet
                                        let ngayduyet = DateTime.Parse(item.NgayDuyet.ToString())
                                        where ngayduyet >= ngaybatdau && ngayduyet <= ngayketthuc
                                        select item).ToList();

                    listTin = shop.TinTucs.ToList();
                    dsTinTuc = (from item in listTin
                                let ngaytao = DateTime.Parse(item.NgayTao.ToString())
                                where ngaytao >= ngaybatdau && ngaytao <= ngayketthuc
                                select item).ToList();
                    model.DanhSachSanPhamBanNhieu = SanPhamBanChay(ngaybatdau, ngayketthuc);
                    model.TongGiaTriHoaDonDaDuyet = GiaTriHoaDon(dsDonHangDaDuyet);
                    model.TongGiaTriHoaDonChuaDuyet = GiaTriHoaDon(dsDonHangChuaDuyet);
                    model.DanhSachSanPhamXemNhieu = listXemNhieu;
                    model.DanhSachHoaDonChuaDuyet = dsDonHangChuaDuyet;
                    model.DanhSachHoaDonDaDuyet = dsDonHangDaDuyet;
                    model.TongThuNhap = GiaTriHoaDon(dsDonHangDaDuyet) - GiaTriHoaDonNhap(ngaybatdau, ngayketthuc);
                    model.TongNhapHang = GiaTriHoaDonNhap(ngaybatdau, ngayketthuc);
                    model.DanhSachTinTucs = dsTinTuc;
                }
                return View(model);
            }


        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TaiKhoan tk)
        {

            ShopEntities shop = new ShopEntities();
            var taiKhoan = shop.TaiKhoans.SingleOrDefault(t => t.TenTaiKhoan == tk.TenTaiKhoan && t.MatKhau == tk.MatKhau);

            if (ModelState.IsValid)
            {
                if (taiKhoan != null)
                {
                    if (taiKhoan.LoaiTaiKhoan == 1)
                    {
                        _maTk = taiKhoan.MaTaiKhoan;
                        Session["MaTKAdmin"] = taiKhoan.MaTaiKhoan;
                        Session["TenTaiKhoanAdmin"] = taiKhoan.TenTaiKhoan;
                        var ct = shop.ChiTietTaiKhoans.SingleOrDefault(c => c.MaTaiKhoan == taiKhoan.MaTaiKhoan);
                        if (ct != null) Session["AnhDaiDien"] = ct.AnhDaiDien;
                        Session["HoTen"] = ct.HoTen;
                        Session["Email"] = taiKhoan.Email;

                        return Redirect("/Admin/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bạn không có quyền truy cập");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
            }

            return View(taiKhoan);
        }

        public ActionResult DangXuat()
        {
            Session["MaTKAdmin"] = null;
            Session["TenTaiKhoanAdmin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        //public ThongKeViewModel ThongKe()
        //{

        //}
        [HttpGet]
        public ActionResult ThongTinTaiKhoan()
        {


            if (Session["MaTKAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ShopEntities shop = new ShopEntities();

                var dsLoaiTaiKhoan = shop.LoaiTaiKhoans.ToList();
                List<SelectListItem> slLoaiTK = new List<SelectListItem>();


                for (int i = 0; i < dsLoaiTaiKhoan.Count; i++)
                {
                    SelectListItem sl = new SelectListItem() { Text = dsLoaiTaiKhoan[i].TenLoai, Value = dsLoaiTaiKhoan[i].MaLoai.ToString() };
                    slLoaiTK.Add(sl);
                }
                ViewBag.LTK = slLoaiTK;

                var model = new QuanLyTaiKhoan()
                {
                    TaiKhoan = shop.TaiKhoans.SingleOrDefault(t => t.MaTaiKhoan == _maTk),
                    ChiTietTaiKhoan = shop.ChiTietTaiKhoans.SingleOrDefault(c => c.MaTaiKhoan == _maTk)
                };
                return View(model);
            }
        }

        //public ActionResult ThongKe(string chonngay)
        //{
        //    var model = new ThongKeViewModel();


        //    }
        //    return 
        //}

        public List<SanPhamBanChay> SanPhamBanChay()
        {
            var shop = new ShopEntities();
            int thang = DateTime.Now.Month;

            var dshd = shop.HoaDons.Where(h => h.TrangThai == true).ToList();
            var dsDonHangDaDuyet = (from item in dshd let thangduyet = DateTime.Parse(item.NgayDuyet.ToString()).Month where thangduyet == thang select item).ToList();
            var dsChiTiet = dsDonHangDaDuyet.SelectMany(item => shop.ChiTietHoaDons.Where(c => c.MaHoaDon == item.MaHoaDon).OrderByDescending(c => c.SoLuong).Skip(0).Take(5).ToList()).ToList();

            var dsBanChay =
                dsChiTiet.GroupBy(s => s.MaSanPham)
                    .Select(
                        cl => new SanPhamBanChay { MaSanPham = cl.First().MaSanPham, SoLuong = cl.Sum(c => c.SoLuong) })
                    .ToList();
            return dsBanChay.OrderByDescending(c => c.SoLuong).ToList();
        }
        public List<SanPhamBanChay> SanPhamBanChay(DateTime ngaybatdau, DateTime ngayketthuc)
        {
            var shop = new ShopEntities();


            var dshd = shop.HoaDons.Where(h => h.TrangThai == true).ToList();
            var dsDonHangDaDuyet = (from item in dshd let ngayduyet = DateTime.Parse(item.NgayDuyet.ToString()) where ngayduyet >= ngaybatdau && ngayduyet <= ngayketthuc select item).ToList();
            var dsChiTiet = dsDonHangDaDuyet.SelectMany(item => shop.ChiTietHoaDons.Where(c => c.MaHoaDon == item.MaHoaDon).OrderByDescending(c => c.SoLuong).Skip(0).Take(5).ToList()).ToList();

            var dsBanChay =
                dsChiTiet.GroupBy(s => s.MaSanPham)
                    .Select(
                        cl => new SanPhamBanChay { MaSanPham = cl.First().MaSanPham, SoLuong = cl.Sum(c => c.SoLuong) })
                    .ToList();



            return dsBanChay.OrderByDescending(c => c.SoLuong).ToList();

        }

        public decimal GiaTriHoaDon(List<HoaDon> list)
        {
            decimal? tong = list.Aggregate<HoaDon, decimal?>(0, (current, item) => current + item.TongGia);
            return (decimal)tong;
        }
        public decimal GiaTriHoaDonNhap()
        {
            ShopEntities shop = new ShopEntities();
            var thangHienTai = DateTime.Now.Month;
            var listHd = shop.HoaDonNhaps.ToList();
            var dsHoaDon = (from item in listHd let thang = DateTime.Parse(item.NgayTao.ToString()).Month where thang == thangHienTai select item).ToList();

            var dsChiTiet = new List<ChiTietHoaDonNhap>();
            foreach (var item in dsHoaDon)
            {
                dsChiTiet.AddRange(shop.ChiTietHoaDonNhaps.Where(c => c.MaHoaDon == item.MaHoaDon));
            }
            decimal? tong = dsChiTiet.Aggregate<ChiTietHoaDonNhap, decimal?>(0, (current, item) => current + item.TongTien);
            return (decimal)tong;
        }

        public decimal GiaTriHoaDonNhap(DateTime ngaybatdau,DateTime ngayketthuc)
        {
            ShopEntities shop = new ShopEntities();
            
            var listHd = shop.HoaDonNhaps.ToList();
            var dsHoaDon = (from item in listHd let ngaynhap = DateTime.Parse(item.NgayTao.ToString()) where ngaynhap >=ngaybatdau&&ngaynhap<=ngayketthuc select item).ToList();

            var dsChiTiet = new List<ChiTietHoaDonNhap>();
            foreach (var item in dsHoaDon)
            {
                dsChiTiet.AddRange(shop.ChiTietHoaDonNhaps.Where(c => c.MaHoaDon == item.MaHoaDon));
            }
            decimal? tong = dsChiTiet.Aggregate<ChiTietHoaDonNhap, decimal?>(0, (current, item) => current + item.TongTien);
            return (decimal)tong;
        }
    }
}