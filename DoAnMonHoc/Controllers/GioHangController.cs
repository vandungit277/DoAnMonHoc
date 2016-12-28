using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;

namespace WebDoAn.Controllers
{
    public class GioHangController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // Giỏ hàng điện thoại
        public List<GioHangDT> LayGioHang()
        {
            List<GioHangDT> lstGioHang = Session["GioHangDT"] as List<GioHangDT>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHangDT>();
                Session["GioHangDT"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int _MA_DT, string strURL)
        {
            List<GioHangDT> lstGioHang = LayGioHang();
            GioHangDT dt = lstGioHang.Find(n => n._MA_DT == _MA_DT);
            if (dt == null)
            {
                dt = new GioHangDT(_MA_DT);
                lstGioHang.Add(dt);
                return Redirect(strURL);
            }
            else
            {
                dt._SL_DT++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int _TongSL = 0;
            List<GioHangDT> lstGioHang = Session["GioHangDT"] as List<GioHangDT>;
            if (lstGioHang != null)
            {
                _TongSL = lstGioHang.Sum(n => n._SL_DT);
            }
            return _TongSL;
        }
        private double TongTien()
        {
            double _TongTien = 0;
            List<GioHangDT> lstGioHang = Session["GioHangDT"] as List<GioHangDT>;
            if (lstGioHang != null)
            {
                _TongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return _TongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHangDT> lstGioHang = LayGioHang();
            List<GioHangPK> lstGioHang_pk = LayGioHangPK();
            if (lstGioHang.Count + lstGioHang_pk.Count == 0)
            {
                return RedirectToAction("Index", "Index");
            }
            ViewBag.TongSL_DT = TongSoLuong();
            ViewBag.TongTien_DT = TongTien();
            ViewBag.TongSL = TongSoLuong() + TongSoLuong_PK();
            ViewBag.TongTien = TongTien() + TongTien_PK();
            return View(lstGioHang);
        }
        public ActionResult XoaGioHang_DT(int _MA_DT)
        {
            List<GioHangDT> lstGioHang = LayGioHang();
            GioHangDT dt = lstGioHang.SingleOrDefault(n => n._MA_DT == _MA_DT);
            if (dt != null)
            {
                lstGioHang.RemoveAll(n => n._MA_DT == _MA_DT);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
                return RedirectToAction("Index", "Index");
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang_DT(int _MA_DT, FormCollection f)
        {
            List<GioHangDT> lstGiohang = LayGioHang();
            GioHangDT dt = lstGiohang.SingleOrDefault(n => n._MA_DT == _MA_DT);
            if (dt != null)
            {
                dt._SL_DT = int.Parse(f["txtSL"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult ClearGioHang_DT()
        {
            List<GioHangDT> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        // Giỏ hàng phụ kiện
        public List<GioHangPK> LayGioHangPK()
        {
            List<GioHangPK> lstGioHang_pk = Session["GioHangPK"] as List<GioHangPK>;
            if (lstGioHang_pk == null)
            {
                lstGioHang_pk = new List<GioHangPK>();
                Session["GioHangPK"] = lstGioHang_pk;
            }
            return lstGioHang_pk;
        }
        public ActionResult ThemGioHang_PK(int _MA_PK, string strURL)
        {
            List<GioHangPK> lstGioHang_pk = LayGioHangPK();
            GioHangPK pk = lstGioHang_pk.Find(n => n._MA_PK == _MA_PK);
            if (pk == null)
            {
                pk = new GioHangPK(_MA_PK);
                lstGioHang_pk.Add(pk);
                return Redirect(strURL);
            }
            else
            {
                pk._SL_PK++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong_PK()
        {
            int _TongSL = 0;
            List<GioHangPK> lstGioHang_pk = Session["GioHangPK"] as List<GioHangPK>;
            if (lstGioHang_pk != null)
            {
                _TongSL = lstGioHang_pk.Sum(n => n._SL_PK);
            }
            return _TongSL;
        }
        private double TongTien_PK()
        {
            double _TongTien = 0;
            List<GioHangPK> lstGioHang_pk = Session["GioHangPK"] as List<GioHangPK>;
            if (lstGioHang_pk != null)
            {
                _TongTien = lstGioHang_pk.Sum(n => n.ThanhTien);
            }
            return _TongTien;
        }
        public ActionResult GioHang_PK()
        {
            List<GioHangDT> lstGioHang = LayGioHang();
            List<GioHangPK> lstGioHang_pk = LayGioHangPK();
            if (lstGioHang_pk.Count + lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Index");
            }
            ViewBag.TongSL_PK = TongSoLuong_PK();
            ViewBag.TongTien_PK = TongTien_PK();
            ViewBag.TongSL = TongSoLuong() + TongSoLuong_PK();
            ViewBag.TongTien = TongTien() + TongTien_PK();
            return PartialView(lstGioHang_pk);
        }
        public ActionResult XoaGioHang_PK(int _MA_PK)
        {
            List<GioHangPK> lstGioHang = LayGioHangPK();
            GioHangPK pk = lstGioHang.SingleOrDefault(n => n._MA_PK == _MA_PK);
            if (pk != null)
            {
                lstGioHang.RemoveAll(n => n._MA_PK == _MA_PK);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
                return RedirectToAction("Index", "Index");
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang_PK(int _MA_PK, FormCollection f)
        {
            List<GioHangPK> lstGiohang = LayGioHangPK();
            GioHangPK pk = lstGiohang.SingleOrDefault(n => n._MA_PK == _MA_PK);
            if (pk != null)
            {
                pk._SL_PK = int.Parse(f["txtSL"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult ClearGioHang_PK()
        {
            List<GioHangPK> lstGiohang = LayGioHangPK();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        // Hiển thị Giỏ hàng trên thanh menu
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TongSoLuong() + TongSoLuong_PK();
            ViewBag.TongTien = TongTien() + TongTien_PK();
            return PartialView();
        }
        // Xóa toàn bộ giỏ hàng
        public ActionResult ClearGioHang()
        {
            ClearGioHang_DT();
            ClearGioHang_PK();
            return RedirectToAction("Index", "Index");
        }
        // Đặt hàng khi đã chọn xong sản phẩm_điện thoại
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (Session["GioHangDT"] == null && Session["GioHangPK"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            List<GioHangDT> lstGiohang_DT = LayGioHang();
            ViewBag.TongSL = TongSoLuong() + TongSoLuong_PK();
            ViewBag.TongTien = TongTien() + TongTien_PK();
            return View(lstGiohang_DT);
        }
        // Đặt hàng khi đã chọn xong sản phẩm_phụ kiện
        public ActionResult DatHang_PK()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (Session["GioHangDT"] == null && Session["GioHangPK"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            List<GioHangPK> lstGiohang_PK = LayGioHangPK();
            return PartialView(lstGiohang_PK);
        }
        // Lưu đơn hàng vào CSDL
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Thêm thông tin vào table Hóa Đơn
            HOADON hd = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHangDT> ghDT = LayGioHang();
            List<GioHangPK> ghPK = LayGioHangPK();
            hd.MA_KH = kh.MA_KH;
            hd.NGAYLAP_HD = DateTime.Now;
            hd.NGAYGIAO_HD = DateTime.Parse(String.Format("{0:dd/MM.yyyy}", collection["NgayGiao"]));
            hd.GIAOHANG = false;
            hd.THANHTOAN = false;
            hd.TONGTIEN_HD = (decimal)(TongTien() + TongTien_PK());

            if (hd.NGAYGIAO_HD <= hd.NGAYLAP_HD)
            {
                ViewBag.ThongBao = "cần ít nhất 1 ngày để giao hàng";
                ViewBag.TongSL = TongSoLuong() + TongSoLuong_PK();
                ViewBag.TongTien = TongTien() + TongTien_PK();
                return View(ghDT);
            }

            var hoten = collection["HoTen_NguoiNhan"];
            if (String.IsNullOrEmpty(hoten))
                hoten = kh.HOTEN_KH;
            hd.TEN_NGUOINHAN = hoten;

            var diachi = collection["DiaChi_NguoiNhan"];
            if (String.IsNullOrEmpty(diachi))
                diachi = kh.DIACHI_KH;
            hd.DIACHI_NGUOINHAN = diachi;

            var sodt = collection["Sdt_NguoiNhan"];
            if (String.IsNullOrEmpty(sodt))
                sodt = kh.SODT_KH;
            hd.SODT_NGUOINHAN = sodt;

            data.HOADONs.InsertOnSubmit(hd);
            data.SubmitChanges();

            // Thêm thông tin vào table chi tiết hóa đơn điện thoại
            foreach (var item in ghDT)
            {
                CT_HOADON dt = new CT_HOADON();
                dt.MA_HD = hd.MA_HD;
                dt.MA_DT = item._MA_DT;
                dt.SOLUONGMUA = item._SL_DT;
                dt.DONGIAMUA = (decimal)item.ThanhTien;
                data.CT_HOADONs.InsertOnSubmit(dt);
            }

            // Thêm thông tin vào table chi tiết hóa đơn phụ kiện
            foreach (var item in ghPK)
            {
                CT_HOADON_PK pk = new CT_HOADON_PK();
                pk.MA_HD = hd.MA_HD;
                pk.MA_PK = item._MA_PK;
                pk.SLMUA_PK = item._SL_PK;
                pk.DGMUA_PK = (decimal)item.ThanhTien;
                data.CT_HOADON_PKs.InsertOnSubmit(pk);
            }

            data.SubmitChanges();
            Session["GioHangDT"] = null;
            Session["GioHangPK"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        // Xác nhận đơn hàng sau khi đã đặt hàng xong
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}