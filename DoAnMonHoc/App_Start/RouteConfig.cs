using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAnMonHoc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Điện thoại theo loại",
               url: "dien-thoai-theo-loai-{id}",
               defaults: new { controller = "Index", action = "DT_THEO_LOAI", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Phụ kiện theo loại",
               url: "phu-kien-theo-loai-{id}",
               defaults: new { controller = "Index", action = "PK_THEO_LOAI", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Điện thoại hết hàng",
               url: "dien-thoai-het-hang",
               defaults: new { controller = "Index", action = "DT_HET_HANG", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Chi tiết điện thoại không bán",
               url: "dien-thoai-het-hang-{id}",
               defaults: new { controller = "Index", action = "DETAILS1", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Chi tiết điện thoại kinh doanh",
               url: "dien-thoai-chi-tiet-{id}",
               defaults: new { controller = "Index", action = "DETAILS", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Chi tiết phụ kiện kinh doanh",
               url: "phu-kien-chi-tiet-{id}",
               defaults: new { controller = "Index", action = "DETAILS_PK", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Danh sách nhà cung cấp",
               url: "admin/danh-sach-nha-cung-cap",
               defaults: new { controller = "Admin", action = "DS_NhaCungCap", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Thêm mới nhà cung cấp",
               url: "admin/them-moi-nha-cung-cap",
               defaults: new { controller = "Admin", action = "Themmoincc", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Chi tiết nhà cung cấp",
              url: "admin/chi-tiet-nha-cung-cap-id-{id}",
              defaults: new { controller = "Admin", action = "Chitietncc", id = UrlParameter.Optional }
              );

            routes.MapRoute(
              name: "Xóa nhà cung cấp",
              url: "admin/xoa-nha-cung-cap-id-{id}",
              defaults: new { controller = "Admin", action = "Xoancc", id = UrlParameter.Optional }
              );

            routes.MapRoute(
              name: "Sửa nhà cung cấp",
              url: "admin/sua-nha-cung-cap-id-{id}",
              defaults: new { controller = "Admin", action = "Suancc", id = UrlParameter.Optional }
              );

            routes.MapRoute(
               name: "Địa chỉ cửa hàng",
               url: "dia-chi-cua-hang",
               defaults: new { controller = "Index", action = "Locate", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Đăng ký",
               url: "nguoi-dung/dang-ky",
               defaults: new { controller = "Nguoidung", action = "Dangky", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Đăng nhập",
               url: "nguoi-dung/dang-nhap",
               defaults: new { controller = "Nguoidung", action = "Dangnhap", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Giỏ hàng",
               url: "gio-hang",
               defaults: new { controller = "GioHang", action = "GioHang", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Xác nhận đơn hàng",
               url: "xac-nhan-don-hang",
               defaults: new { controller = "GioHang", action = "XacNhanDonHang", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "Đơn đặt hàng",
               url: "don-dat-hang",
               defaults: new { controller = "GioHang", action = "DatHang", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
