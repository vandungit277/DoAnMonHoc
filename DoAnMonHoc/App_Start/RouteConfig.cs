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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
