using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnMonHoc.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace DoAnMonHoc.Controllers
{
    public class AdminController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: Admin
        public ActionResult Index(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 3;
            var kq = from d in data.HOADONs
                     where d.NGAYGIAO_HD >= DateTime.Now && d.GIAOHANG == false
                     orderby d.NGAYGIAO_HD ascending
                     select d;
            return View(kq.ToPagedList(pagenum, pagesize));
        }

        #region xử lý thông tin cửa hàng

        [HttpGet]
        // Sửa thông tin cửa hàng
        public ActionResult SuaThongTinCH()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            CUAHANG ch = data.CUAHANGs.SingleOrDefault(n => n.MA_CH == 1);
            ViewBag.MaCH = ch.MA_CH;
            if (ch == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ch);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaThongTinCH(CUAHANG _ch)
        {
            CUAHANG ch = data.CUAHANGs.Where(n => n.MA_CH == 1).FirstOrDefault();
            UpdateModel(ch);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region xử lý tài khoản cá nhân

        // đăng nhập cho admin
        [HttpGet]
        public ActionResult Login_Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login_Admin(FormCollection col)
        {
            var tentk = col["username"];
            var matkhau = col["password"];
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk && n.MatKhau_Admin == matkhau);
            if (ad != null)
            {
                Session["TaiKhoan_Admin"] = ad;
                Session["TenTK_Admin"] = ad.Ten_TK_Admin;
                Session["HoTen_Admin"] = ad.Hoten_Admin;
                Session["Ma_PQ"] = ad.MA_PQ;
                return RedirectToAction("Index", "Admin");
            }
            else
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }
        // đăng xuất cho admin
        public ActionResult Logout_Admin()
        {
            Session["TaiKhoan_Admin"] = null;
            Session["TenTK_Admin"] = null;
            Session["HoTen_Admin"] = null;
            Session["Ma_PQ"] = null;
            return RedirectToAction("Login_Admin", "Admin");
        }
        // thông tin admin
        public ActionResult Info_Admin()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View(data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == Session["TenTK_Admin"].ToString()));
        }
        // sửa thông tin cá nhân
        [HttpGet]
        public ActionResult Edit_Admin(string tentk)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk);
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit_Admin(Admin _ad)
        {
            Admin ad = data.Admins.Where(n => n.Ten_TK_Admin == _ad.Ten_TK_Admin).FirstOrDefault();

            UpdateModel(ad);
            data.SubmitChanges();
            return RedirectToAction("Info_Admin");
        }

        #endregion

        #region xử lý nhân viên

        // Hiển thị danh sách nhân viên
        public ActionResult DS_NhanVien()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            return View(data.Admins.ToList().OrderBy(n => n.MA_PQ));
        }

        // thêm mới nhân viên
        [HttpGet]
        public ActionResult Themmoinv()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            ViewBag.Ma_PQ = new SelectList(data.PhanQuyens.ToList().OrderBy(n => n.MA_PQ), "Ma_PQ", "Ten_PQ");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoinv(Admin ad)
        {
            ViewBag.Ma_PQ = new SelectList(data.PhanQuyens.ToList().OrderBy(n => n.MA_PQ), "Ma_PQ", "Ten_PQ");
            if (ModelState.IsValid)
            {
                data.Admins.InsertOnSubmit(ad);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_NhanVien");
        }
        // xem thông tin chi tiết nhân viên
        public ActionResult Chitietnv(string tentk)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk);
            ViewBag.TenTK = ad.Ten_TK_Admin;
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }
        // xóa nhân viên
        [HttpGet]
        public ActionResult Xoanv(string tentk)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk);
            ViewBag.TenTK = ad.Ten_TK_Admin;
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }
        // xác nhận xóa nhân viên
        [HttpPost, ActionName("Xoanv")]
        public ActionResult XacNhanXoanv(string tentk)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
                if (Session["TenTK_Admin"].ToString() == tentk)
                    return RedirectToAction("DS_NhanVien");
            }
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk);
            ViewBag.TenTK = ad.Ten_TK_Admin;
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Admins.DeleteOnSubmit(ad);
            data.SubmitChanges();
            return RedirectToAction("DS_NhanVien");
        }
        // sửa nhân viên
        [HttpGet]
        public ActionResult Suanv(string tentk)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            Admin ad = data.Admins.SingleOrDefault(n => n.Ten_TK_Admin == tentk);
            ViewBag.TenTK = ad.Ten_TK_Admin;
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var list = data.PhanQuyens.ToList().Where(n => n.MA_PQ == ad.MA_PQ).Union(data.PhanQuyens.ToList().Where(n => n.MA_PQ != ad.MA_PQ));
            ViewBag.Ma_PQ = new SelectList(list, "Ma_PQ", "Ten_PQ");
            return View(ad);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suanv(Admin _ad)
        {
            ViewBag.Ma_PQ = new SelectList(data.PhanQuyens.ToList().OrderBy(n => n.MA_PQ), "Ma_PQ", "Ten_PQ");
            Admin ad = data.Admins.Where(n => n.Ten_TK_Admin == _ad.Ten_TK_Admin).FirstOrDefault();
            if (Session["TenTK_Admin"].ToString() == ad.Ten_TK_Admin)
            {
                if (_ad.MA_PQ != ad.MA_PQ)
                    return RedirectToAction("DS_NhanVien");
            }
            UpdateModel(ad);
            data.SubmitChanges();
            return RedirectToAction("DS_NhanVien");
        }

        #endregion

        #region xử lý khách hàng

        // Hiển thị danh sách nhân viên
        public ActionResult DS_KhachHang(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 6;
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.MA_KH).ToPagedList(pagenum, pagesize));
        }
        // xem thông tin chi tiết nhân viên
        public ActionResult Chitietkh(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MA_KH == id);
            ViewBag.Makh = kh.MA_KH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        #endregion

        #region xử lý điện thoại

        // hiển thị danh sách điện thoại
        public ActionResult DS_DienThoai(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 6;
            return View(data.DIENTHOAIs.ToList().OrderBy(n => n.MA_LDT).ToPagedList(pagenum, pagesize));
        }
        // thêm mới điện thoại
        [HttpGet]
        public ActionResult Themmoidt()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            ViewBag.Ma_LDT = new SelectList(data.LOAIDIENTHOAIs.ToList().OrderBy(n => n.MA_LDT), "Ma_LDT", "Ten_LDT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoidt(DIENTHOAI dt)
        {
            ViewBag.Ma_LDT = new SelectList(data.LOAIDIENTHOAIs.ToList().OrderBy(n => n.MA_LDT), "Ma_LDT", "Ten_LDT");

            var fileName1 = Path.GetFileName(Request.Files["HinhAnh1"].FileName);
            var fileName2 = Path.GetFileName(Request.Files["HinhAnh2"].FileName);
            //if (HinhAnh1 == null)
            //{
            //    ViewBag.HinhAnh1 = "Vui lòng chọn ảnh";
            //    return View();
            //}
            if (ModelState.IsValid)
            {
                //var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                //var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                //if (System.IO.File.Exists(path1))
                //{
                //    ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                //    return View();
                //}
                //else
                //    HinhAnh1.SaveAs(path1);
                if (fileName1 != "")
                {
                    var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                    if (System.IO.File.Exists(path1))
                    {
                        if (System.IO.File.Exists(path1))
                            ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View();
                    }
                    else
                    {
                        Request.Files["HinhAnh1"].SaveAs(path1);
                    }
                    //==================================================
                    dt.HINHANH_DT = fileName1;
                }
                //==================================================
                if (fileName2 != "")
                {
                    var path2 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName2);
                    if (System.IO.File.Exists(path2))
                    {
                        if (System.IO.File.Exists(path2))
                            ViewBag.HinhAnh2 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View();
                    }
                    else
                    {
                        Request.Files["HinhAnh2"].SaveAs(path2);
                    }
                    //==================================================
                    dt.HINHANH_DT2 = fileName2;
                }
                //==================================================
                data.DIENTHOAIs.InsertOnSubmit(dt);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_DienThoai");
        }
        // xem thông tin chi tiết điện thoại
        public ActionResult Chitietdt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            DIENTHOAI dt = data.DIENTHOAIs.SingleOrDefault(n => n.MA_DT == id);
            ViewBag.MaDT = dt.MA_DT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }
        // xóa điện thoại
        [HttpGet]
        public ActionResult Xoadt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            DIENTHOAI dt = data.DIENTHOAIs.SingleOrDefault(n => n.MA_DT == id);
            ViewBag.MaDT = dt.MA_DT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }
        // xác nhận xóa điện thoại
        [HttpPost, ActionName("Xoadt")]
        public ActionResult XacNhanXoadt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            DIENTHOAI dt = data.DIENTHOAIs.SingleOrDefault(n => n.MA_DT == id);
            ViewBag.MaDT = dt.MA_DT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DIENTHOAIs.DeleteOnSubmit(dt);
            data.SubmitChanges();
            return RedirectToAction("DS_DienThoai");
        }
        // sửa điện thoại
        [HttpGet]
        public ActionResult Suadt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            DIENTHOAI dt = data.DIENTHOAIs.SingleOrDefault(n => n.MA_DT == id);
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var list = data.LOAIDIENTHOAIs.ToList().Where(n => n.MA_LDT == dt.MA_LDT).Union(data.LOAIDIENTHOAIs.ToList().Where(n => n.MA_LDT != dt.MA_LDT));
            ViewBag.Ma_LDT = new SelectList(list, "Ma_LDT", "Ten_LDT");
            return View(dt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadt(DIENTHOAI _dt)
        {
            ViewBag.Ma_LDT = new SelectList(data.LOAIDIENTHOAIs.ToList().OrderBy(n => n.MA_LDT), "Ma_LDT", "Ten_LDT");
            DIENTHOAI dt = data.DIENTHOAIs.Where(n => n.MA_DT == _dt.MA_DT).FirstOrDefault();
            var fileName1 = Path.GetFileName(Request.Files["HinhAnh1"].FileName);
            var fileName2 = Path.GetFileName(Request.Files["HinhAnh2"].FileName);
            //if (HinhAnh1 == null)
            //{
            //    ViewBag.HinhAnh1 = "Vui lòng chọn ảnh";
            //    return View();
            //}
            if (ModelState.IsValid)
            {
                //var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                //var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                //if (System.IO.File.Exists(path1))
                //{
                //    ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                //    return View();
                //}
                //else
                //    HinhAnh1.SaveAs(path1);
                if (fileName1 != "")
                {
                    var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                    if (System.IO.File.Exists(path1))
                    {
                        ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View(dt);
                    }
                    else
                    {
                        Request.Files["HinhAnh1"].SaveAs(path1);
                    }
                    //==================================================
                    dt.HINHANH_DT = fileName1;
                }
                //==================================================
                if (fileName2 != "")
                {
                    var path2 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName2);
                    if (System.IO.File.Exists(path2))
                    {
                        ViewBag.HinhAnh2 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View(dt);
                    }
                    else
                    {
                        Request.Files["HinhAnh2"].SaveAs(path2);
                    }
                    //==================================================
                    dt.HINHANH_DT2 = fileName2;
                }
                //==================================================
                UpdateModel(dt);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_DienThoai");
        }

        #endregion

        #region xử lý loại điện thoại

        // Hiển thị danh sách loại điện thoại
        public ActionResult DS_LoaiDienThoai(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 8;
            return View(data.LOAIDIENTHOAIs.ToList().OrderBy(n => n.MA_LDT).ToPagedList(pagenum, pagesize));
        }

        // thêm mới loại điện thoại
        [HttpGet]
        public ActionResult Themmoildt()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            ViewBag.Ma_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.MA_NCC), "Ma_NCC", "Ten_NCC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoildt(LOAIDIENTHOAI ldt)
        {
            ViewBag.Ma_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.MA_NCC), "Ma_NCC", "Ten_NCC");
            if (ModelState.IsValid)
            {
                data.LOAIDIENTHOAIs.InsertOnSubmit(ldt);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_LoaiDienThoai");
        }
        // xóa loại điện thoại
        [HttpGet]
        public ActionResult Xoaldt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIDIENTHOAI ldt = data.LOAIDIENTHOAIs.SingleOrDefault(n => n.MA_LDT == id);
            ViewBag.MaLDT = ldt.MA_LDT;
            if (ldt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ldt);
        }
        // xác nhận xóa loại điện thoại
        [HttpPost, ActionName("Xoaldt")]
        public ActionResult XacNhanXoaldt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIDIENTHOAI ldt = data.LOAIDIENTHOAIs.SingleOrDefault(n => n.MA_LDT == id);
            ViewBag.MaDT = ldt.MA_LDT;
            if (ldt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LOAIDIENTHOAIs.DeleteOnSubmit(ldt);
            data.SubmitChanges();
            return RedirectToAction("DS_LoaiDienThoai");
        }
        // sửa loại điện thoại
        [HttpGet]
        public ActionResult Sualdt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIDIENTHOAI ldt = data.LOAIDIENTHOAIs.SingleOrDefault(n => n.MA_LDT == id);
            if (ldt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Ma_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.MA_NCC), "Ma_NCC", "Ten_NCC");
            return View(ldt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sualdt(LOAIDIENTHOAI _ldt)
        {
            ViewBag.Ma_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.MA_NCC), "Ma_NCC", "Ten_NCC");
            LOAIDIENTHOAI ldt = data.LOAIDIENTHOAIs.Where(n => n.MA_LDT == _ldt.MA_LDT).FirstOrDefault();

            UpdateModel(ldt);
            data.SubmitChanges();
            return RedirectToAction("DS_LoaiDienThoai");
        }

        #endregion

        #region xử lý nhà cung cấp

        // Hiển thị danh sách nhà cung cấp
        public ActionResult DS_NhaCungCap(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 8;
            return View(data.NHACUNGCAPs.ToList().OrderBy(n => n.MA_NCC).ToPagedList(pagenum, pagesize));
        }

        // thêm mới nhà cung cấp
        [HttpGet]
        public ActionResult Themmoincc()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoincc(NHACUNGCAP ncc)
        {
            if (ModelState.IsValid)
            {
                data.NHACUNGCAPs.InsertOnSubmit(ncc);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_NhaCungCap");
        }
        // xem thông tin chi tiết nhà cung cấp
        public ActionResult Chitietncc(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MA_NCC == id);
            ViewBag.MaNCC = ncc.MA_NCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        // xóa nhà cung cấp
        [HttpGet]
        public ActionResult Xoancc(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MA_NCC == id);
            ViewBag.MaNCC = ncc.MA_NCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        // xác nhận xóa nhà cung cấp
        [HttpPost, ActionName("Xoancc")]
        public ActionResult XacNhanXoancc(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MA_NCC == id);
            ViewBag.MaNCC = ncc.MA_NCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.NHACUNGCAPs.DeleteOnSubmit(ncc);
            data.SubmitChanges();
            return RedirectToAction("DS_NhaCungCap");
        }
        // sửa nhà cung cấp
        [HttpGet]
        public ActionResult Suancc(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MA_NCC == id);
            ViewBag.MaNCC = ncc.MA_NCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suancc(NHACUNGCAP _ncc)
        {
            NHACUNGCAP ncc = data.NHACUNGCAPs.Where(n => n.MA_NCC == _ncc.MA_NCC).FirstOrDefault();

            UpdateModel(ncc);
            data.SubmitChanges();
            return RedirectToAction("DS_NhaCungCap");
        }

        #endregion

        #region xử lý phụ kiện

        // hiển thị danh sách phụ kiện
        public ActionResult DS_PhuKien(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 6;
            return View(data.PHUKIENs.ToList().OrderBy(n => n.MA_PK).ToPagedList(pagenum, pagesize));
        }
        // thêm mới phụ kiện
        [HttpGet]
        public ActionResult Themmoipk()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            ViewBag.Ma_LPK = new SelectList(data.LOAIPHUKIENs.ToList().OrderBy(n => n.MA_LPK), "Ma_LPK", "Ten_LPK");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoipk(PHUKIEN pk)
        {
            ViewBag.Ma_LPK = new SelectList(data.LOAIPHUKIENs.ToList().OrderBy(n => n.MA_LPK), "Ma_LPK", "Ten_LPK");

            var fileName = Path.GetFileName(Request.Files["HinhAnh"].FileName);
            //if (HinhAnh1 == null)
            //{
            //    ViewBag.HinhAnh1 = "Vui lòng chọn ảnh";
            //    return View();
            //}
            if (ModelState.IsValid)
            {
                //var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                //var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                //if (System.IO.File.Exists(path1))
                //{
                //    ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                //    return View();
                //}
                //else
                //    HinhAnh1.SaveAs(path1);
                if (fileName != "")
                {
                    var path = Path.Combine(Server.MapPath("~/Hinhsp"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        if (System.IO.File.Exists(path))
                            ViewBag.HinhAnh = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View();
                    }
                    else
                    {
                        Request.Files["HinhAnh"].SaveAs(path);
                    }
                    //==================================================
                    pk.HINHANH_PK = fileName;
                }
                //==================================================
                data.PHUKIENs.InsertOnSubmit(pk);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_PhuKien");
        }
        // xem thông tin chi tiết phụ kiện
        public ActionResult Chitietpk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            PHUKIEN pk = data.PHUKIENs.SingleOrDefault(n => n.MA_PK == id);
            ViewBag.MaPK = pk.MA_PK;
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(pk);
        }
        // xóa phụ kiện
        [HttpGet]
        public ActionResult Xoapk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            PHUKIEN pk = data.PHUKIENs.SingleOrDefault(n => n.MA_PK == id);
            ViewBag.MaPK = pk.MA_PK;
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(pk);
        }
        // xác nhận xóa phụ kiện
        [HttpPost, ActionName("Xoapk")]
        public ActionResult XacNhanXoapk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            PHUKIEN pk = data.PHUKIENs.SingleOrDefault(n => n.MA_PK == id);
            ViewBag.MaPK = pk.MA_PK;
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.PHUKIENs.DeleteOnSubmit(pk);
            data.SubmitChanges();
            return RedirectToAction("DS_PhuKien");
        }
        // sửa phụ kiện
        [HttpGet]
        public ActionResult Suapk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            PHUKIEN pk = data.PHUKIENs.SingleOrDefault(n => n.MA_PK == id);
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var list = data.LOAIPHUKIENs.ToList().Where(n => n.MA_LPK == pk.MA_LPK).Union(data.LOAIPHUKIENs.ToList().Where(n => n.MA_LPK != pk.MA_LPK));
            ViewBag.Ma_LPK = new SelectList(list, "Ma_LPK", "Ten_LPK");
            return View(pk);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suapk(PHUKIEN _pk)
        {
            ViewBag.Ma_LPK = new SelectList(data.LOAIPHUKIENs.ToList().OrderBy(n => n.MA_LPK), "Ma_LPK", "Ten_LPK");
            PHUKIEN pk = data.PHUKIENs.Where(n => n.MA_PK == _pk.MA_PK).FirstOrDefault();
            var fileName = Path.GetFileName(Request.Files["HinhAnh"].FileName);
            //if (HinhAnh1 == null)
            //{
            //    ViewBag.HinhAnh1 = "Vui lòng chọn ảnh";
            //    return View();
            //}
            if (ModelState.IsValid)
            {
                //var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                //var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                //if (System.IO.File.Exists(path1))
                //{
                //    ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                //    return View();
                //}
                //else
                //    HinhAnh1.SaveAs(path1);
                if (fileName != "")
                {
                    var path = Path.Combine(Server.MapPath("~/Hinhsp"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.HinhAnh = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View(pk);
                    }
                    else
                    {
                        Request.Files["HinhAnh"].SaveAs(path);
                    }
                    //==================================================
                    pk.HINHANH_PK = fileName;
                }
                //==================================================
                UpdateModel(pk);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_PhuKien");
        }

        #endregion

        #region xử lý loại phụ kiện

        // Hiển thị danh sách loại phụ kiện
        public ActionResult DS_LoaiPhuKien()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View(data.LOAIPHUKIENs.ToList().OrderBy(n => n.MA_LPK));
        }

        // thêm mới loại phụ kiện
        [HttpGet]
        public ActionResult Themmoilpk()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoilpk(LOAIPHUKIEN lpk)
        {
            if (ModelState.IsValid)
            {
                data.LOAIPHUKIENs.InsertOnSubmit(lpk);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_LoaiPhuKien");
        }
        // xóa loại phụ kiện
        [HttpGet]
        public ActionResult Xoalpk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIPHUKIEN lpk = data.LOAIPHUKIENs.SingleOrDefault(n => n.MA_LPK == id);
            ViewBag.MaLPK = lpk.MA_LPK;
            if (lpk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lpk);
        }
        // xác nhận xóa loại phụ kiện
        [HttpPost, ActionName("Xoalpk")]
        public ActionResult XacNhanXoalpk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIPHUKIEN lpk = data.LOAIPHUKIENs.SingleOrDefault(n => n.MA_LPK == id);
            ViewBag.MaLPK = lpk.MA_LPK;
            if (lpk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LOAIPHUKIENs.DeleteOnSubmit(lpk);
            data.SubmitChanges();
            return RedirectToAction("DS_LoaiPhuKien");
        }
        // sửa loại phụ kiện
        [HttpGet]
        public ActionResult Sualpk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            LOAIPHUKIEN lpk = data.LOAIPHUKIENs.SingleOrDefault(n => n.MA_LPK == id);
            if (lpk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lpk);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sualpk(LOAIPHUKIEN _lpk)
        {
            LOAIPHUKIEN lpk = data.LOAIPHUKIENs.Where(n => n.MA_LPK == _lpk.MA_LPK).FirstOrDefault();

            UpdateModel(lpk);
            data.SubmitChanges();
            return RedirectToAction("DS_LoaiPhuKien");
        }

        #endregion

        #region xử lý hóa đơn

        // Hiển thị danh sách hóa đơn
        public ActionResult DS_HoaDon(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 9;
            return View(data.HOADONs.ToList().OrderBy(n => n.GIAOHANG).ToPagedList(pagenum, pagesize));
        }
        // xem thông tin chi tiết hóa đơn
        public ActionResult Chitiethd(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            HOADON hd = data.HOADONs.SingleOrDefault(n => n.MA_HD == id);
            ViewBag.Mahd = hd.MA_HD;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        // chi tiết hóa đơn điện thoại
        public ActionResult Chitiethd_dt(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return PartialView(data.CT_HOADONs.ToList().Where(n => n.MA_HD == id));
        }
        // chi tiết hóa đơn phụ kiện
        public ActionResult Chitiet_pk(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return PartialView(data.CT_HOADON_PKs.ToList().Where(n => n.MA_HD == id));
        }
        // Cập nhật hóa đơn
        [HttpGet]
        public ActionResult Capnhathd(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            HOADON hd = data.HOADONs.SingleOrDefault(n => n.MA_HD == id);
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        [HttpPost]
        public ActionResult Capnhathd(FormCollection col, HOADON _hd)
        {
            HOADON hd = data.HOADONs.Where(n => n.MA_HD == _hd.MA_HD).SingleOrDefault();
            var thanhtoan = col["ThanhToan"];
            var giaohang = col["GiaoHang"];
            if (thanhtoan == "true")
                hd.THANHTOAN = true;
            else
                hd.THANHTOAN = false;
            if (giaohang == "true")
                hd.GIAOHANG = true;
            else
                hd.GIAOHANG = false;
            UpdateModel(hd);
            data.SubmitChanges();
            return RedirectToAction("DS_HoaDon");
        }
        // Xóa hóa đơn
        [HttpGet]
        public ActionResult Xoahd(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            HOADON hd = data.HOADONs.SingleOrDefault(n => n.MA_HD == id);
            ViewBag.MaHD = hd.MA_HD;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        [HttpPost, ActionName("Xoahd")]
        public ActionResult XacNhanXoahd(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            else
            {
                if (int.Parse(Session["MA_PQ"].ToString()) != 1)
                    return RedirectToAction("Logout_Admin");
            }
            HOADON hd = data.HOADONs.SingleOrDefault(n => n.MA_HD == id);
            List<CT_HOADON> hddt = data.CT_HOADONs.Where(n => n.MA_HD == id).ToList();
            List<CT_HOADON_PK> hdpk = data.CT_HOADON_PKs.Where(n => n.MA_HD == id).ToList();
            ViewBag.MaHD = hd.MA_HD;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            foreach (var item in hddt)
                data.CT_HOADONs.DeleteOnSubmit(item);
            foreach (var item in hdpk)
                data.CT_HOADON_PKs.DeleteOnSubmit(item);
            data.HOADONs.DeleteOnSubmit(hd);
            data.SubmitChanges();
            return RedirectToAction("DS_HoaDon");
        }

        #endregion

        #region xử lý ảnh event

        // hiển thị danh sách sự kiện
        public ActionResult DS_Event(int? page)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            int pagenum = (page ?? 1);
            int pagesize = 6;
            return View(data.EVENTs.ToList().OrderBy(n => n.MA_EVENT).ToPagedList(pagenum, pagesize));
        }
        // thêm mới sự kiện
        [HttpGet]
        public ActionResult ThemmoiEvent()
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiEvent(EVENT ev)
        {
            var fileName = Path.GetFileName(Request.Files["HinhAnh"].FileName);
            //if (HinhAnh1 == null)
            //{
            //    ViewBag.HinhAnh1 = "Vui lòng chọn ảnh";
            //    return View();
            //}
            if (ModelState.IsValid)
            {
                //var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                //var path1 = Path.Combine(Server.MapPath("~/Hinhsp"), fileName1);
                //if (System.IO.File.Exists(path1))
                //{
                //    ViewBag.HinhAnh1 = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                //    return View();
                //}
                //else
                //    HinhAnh1.SaveAs(path1);
                if (fileName != "")
                {
                    var path = Path.Combine(Server.MapPath("~/HinhEvent"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        if (System.IO.File.Exists(path))
                            ViewBag.HinhAnh = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View();
                    }
                    else
                    {
                        Request.Files["HinhAnh"].SaveAs(path);
                    }
                    //==================================================
                    ev.HINH_EVENT = fileName;
                }
                //==================================================
                data.EVENTs.InsertOnSubmit(ev);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_Event");
        }
        // xem thông tin chi tiết sự kiện
        public ActionResult ChitietEvent(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            EVENT ev = data.EVENTs.SingleOrDefault(n => n.MA_EVENT == id);
            ViewBag.MaEV = ev.MA_EVENT;
            if (ev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ev);
        }
        // xóa sự kiện
        [HttpGet]
        public ActionResult XoaEvent(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            EVENT ev = data.EVENTs.SingleOrDefault(n => n.MA_EVENT == id);
            ViewBag.MaEV = ev.MA_EVENT;
            if (ev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ev);
        }
        // xác nhận xóa sự kiện
        [HttpPost, ActionName("XoaEvent")]
        public ActionResult XacNhanXoaEvent(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            EVENT ev = data.EVENTs.SingleOrDefault(n => n.MA_EVENT == id);
            ViewBag.MaEV = ev.MA_EVENT;
            if (ev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.EVENTs.DeleteOnSubmit(ev);
            data.SubmitChanges();
            return RedirectToAction("DS_Event");
        }
        // sửa sự kiện
        [HttpGet]
        public ActionResult SuaEvent(int id)
        {
            if (Session["TaiKhoan_Admin"] == null || Session["TaiKhoan_Admin"].ToString() == "")
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            EVENT ev = data.EVENTs.SingleOrDefault(n => n.MA_EVENT == id);
            if (ev == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ev);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaEvent(EVENT _ev)
        {
            EVENT ev = data.EVENTs.Where(n => n.MA_EVENT == _ev.MA_EVENT).FirstOrDefault();
            var fileName = Path.GetFileName(Request.Files["HinhAnh"].FileName);
            if (ModelState.IsValid)
            {
                if (fileName != "")
                {
                    var path = Path.Combine(Server.MapPath("~/HinhEvent"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.HinhAnh = "Tên ảnh đã tồn tại! Vui lòng đổi tên hình ảnh";
                        return View(ev);
                    }
                    else
                    {
                        Request.Files["HinhAnh"].SaveAs(path);
                    }
                    //==================================================
                    ev.HINH_EVENT = fileName;
                }
                //==================================================
                UpdateModel(ev);
                data.SubmitChanges();
            }
            return RedirectToAction("DS_Event");
        }

        #endregion
    }
}