using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DoAnMonHoc.Models;

namespace WebDoAn.Controllers
{
    public class IndexController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();


        // Lấy điện thoại mới
        private List<DIENTHOAI> LAY_DT_MOI(int count)
        {
            return data.DIENTHOAIs.OrderByDescending(d => d.NGAYNHAP_DT).Where(d => d.NGAYNHAP_DT <= DateTime.Now && d.SOLUONGTON != 0).Take(count).ToList();
        }

        // GET: Index
        public ActionResult Index(int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pagesize = 9;
            //Tạo biến số trang
            int pagenum = (page ?? 1);
            //Lấy top 9 điện thoại mới nhất
            var DT_MOI = LAY_DT_MOI(27);
            return View(DT_MOI.ToPagedList(pagenum, pagesize));
        }

        // Trang địa điểm cửa hàng
        public ActionResult Locate()
        {
            return View(data.CUAHANGs.Where(n => n.MA_CH == 1).SingleOrDefault());
        }

        // Lấy loại điện thoại
        public ActionResult LAY_LOAI_DT()
        {
            var ldt = data.LAY_DS_LOAI_DT();
            return PartialView(ldt);
        }

        // Lấy loại phụ kiện
        public ActionResult LAY_LOAI_PK()
        {
            var lpk = data.LAY_DS_LOAI_PK();
            return PartialView(lpk);
        }

        // Sản phẩm sắp hết hàng
        public ActionResult DT_SL_MIN()
        {
            var kq = (from d in data.DIENTHOAIs
                      where d.SOLUONGTON != 0 && d.NGAYNHAP_DT <= DateTime.Now
                      orderby d.SOLUONGTON ascending
                      select d).Skip(0).Take(1);
            return PartialView(kq.Single());
        }

        // Sản phẩm còn nhiều
        public ActionResult DT_SL_MAX()
        {
            var kq = (from d in data.DIENTHOAIs
                      where d.SOLUONGTON != 0 && d.NGAYNHAP_DT <= DateTime.Now
                      orderby d.SOLUONGTON descending
                      select d).Skip(0).Take(1);
            return PartialView(kq.Single());
        }

        // Danh sách điện thoại theo loại
        public ActionResult DT_THEO_LOAI(int id, int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pagesize = 9;
            //Tạo biến số trang
            int pagenum = (page ?? 1);
            var kq = from d in data.DIENTHOAIs
                     where d.MA_LDT == id && d.SOLUONGTON != 0 && d.NGAYNHAP_DT <= DateTime.Now
                     select d;
            return View(kq.ToPagedList(pagenum, pagesize));
        }

        // Danh sách phụ kiện theo loại
        public ActionResult PK_THEO_LOAI(int id, int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pagesize = 9;
            //Tạo biến số trang
            int pagenum = (page ?? 1);
            var kq = from p in data.PHUKIENs
                     where p.MA_LPK == id
                     select p;
            return View(kq.ToPagedList(pagenum, pagesize));
        }

        // Chi tiết điện thoại bán
        public ActionResult DETAILS(int id)
        {
            var kq = from dt in data.DIENTHOAIs
                     where dt.MA_DT == id
                     select dt;
            if (kq.Single().SOLUONGTON == 0 || kq.Single().NGAYNHAP_DT > DateTime.Now)
                return RedirectToAction("DETAILS1", new { id = id });
            return View(kq.Single());
        }

        // Chi tiết điện thoại không bán
        public ActionResult DETAILS1(int id)
        {
            var kq = from dt in data.DIENTHOAIs
                     where dt.MA_DT == id
                     select dt;
            return View(kq.Single());
        }

        // Điện thoại cùng loại
        public ActionResult DT_CUNG_LOAI(int MA_LDT, int MA_DT, int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pagesize = 3;
            //Tạo biến số trang
            int pagenum = (page ?? 1);
            var kq = from dt in data.DIENTHOAIs
                     where dt.MA_LDT == MA_LDT && dt.MA_DT != MA_DT && dt.SOLUONGTON != 0 && dt.NGAYNHAP_DT <= DateTime.Now
                     select dt;
            return PartialView(kq.ToPagedList(pagenum, pagesize));
        }

        // Chi tiết phụ kiện
        public ActionResult DETAILS_PK(int id)
        {
            var kq = from pk in data.PHUKIENs
                     where pk.MA_PK == id
                     select pk;
            return View(kq.Single());
        }

        // Điện thoại sắp ra mắt
        public ActionResult DT_SAP_RA_MAT()
        {
            return View(data.DIENTHOAIs.Where(d => d.NGAYNHAP_DT > DateTime.Now).ToList());
        }

        // Điện thoại đã hết hàng
        public ActionResult DT_HET_HANG()
        {
            return View(data.DIENTHOAIs.Where(d => d.NGAYNHAP_DT <= DateTime.Now && d.SOLUONGTON == 0).ToList());
        }

        // Load ảnh event
        public ActionResult Load_Event()
        {
            return PartialView(data.EVENTs.ToList());
        }

        // Load thông tin cửa hàng lên footer
        public ActionResult Info_Footer()
        {
            return PartialView(data.CUAHANGs.Where(n => n.MA_CH == 1).SingleOrDefault());
        }

        // Load thông tin cửa hàng lên menu
        public ActionResult Info_Dropdown()
        {
            return PartialView(data.CUAHANGs.Where(n => n.MA_CH == 1).SingleOrDefault());
        }


    }
}