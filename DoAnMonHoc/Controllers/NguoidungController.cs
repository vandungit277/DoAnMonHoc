using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnMonHoc.Controllers
{
    public class NguoidungController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenTK"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["MatKhauNhapLai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            var gioitinh = collection["GioiTinh"];

            //-------------------------------------------------------
            Session["TenTK"] = tendn;
            Session["HoTen"] = hoten;
            Session["DiaChi"] = diachi;
            Session["Email"] = email;
            Session["DienThoai"] = dienthoai;
            Session["NgaySinh"] = ngaysinh;
            //-------------------------------------------------------
            if (String.IsNullOrEmpty(hoten))
                ViewData["Loi1"] = "Vui lòng không để trống";
            else if (String.IsNullOrEmpty(tendn))
                ViewData["Loi2"] = "Vui lòng không để trống";
            else if (data.KHACHHANGs.Count(k => k.TAIKHOAN == tendn) > 0)
                ViewData["Loi2"] = "Tên tài khoản đã tồn tại";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi3"] = "Vui lòng không để trống";
            else if (matkhau.Length < 6)
                ViewData["Loi3"] = "Vui lòng nhập mật khẩu có ít nhất 6 kí tự";
            else if (String.IsNullOrEmpty(matkhaunhaplai))
                ViewData["Loi4"] = "Vui lòng không để trống";
            else if (String.Compare(matkhau, matkhaunhaplai) != 0)
                ViewData["Loi4"] = "Mật khẩu nhập lại không khớp";
            else if (String.IsNullOrEmpty(dienthoai))
                ViewData["Loi5"] = "Vui lòng không để trống";
            else if (String.IsNullOrEmpty(email))
                ViewData["Loi6"] = "Vui lòng không để trống";
            else
            {
                kh.HOTEN_KH = hoten;
                kh.TAIKHOAN = tendn;
                kh.MATKHAU = matkhau;
                kh.EMAIL = email;
                kh.DIACHI_KH = diachi;
                kh.SODT_KH = dienthoai;
                kh.NGAYSINH_KH = DateTime.Parse(ngaysinh);
                if (gioitinh == "Nam")
                    kh.GIOITINH_KH = true;
                if (gioitinh == "Nữ")
                    kh.GIOITINH_KH = false;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
 [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenTK"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TAIKHOAN == tendn && n.MATKHAU == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    Session["HotenKH"] = kh.HOTEN_KH;
                    Session["MaKH"] = kh.MA_KH;
                    return RedirectToAction("Index", "Index");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Dangxuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "Index");
        }
    }
}