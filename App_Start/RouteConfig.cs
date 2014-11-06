using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebService_use
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SXTKB",
                url: "SXTKB/{controller}/{action}/{mamonhoc}/{nhom}/{thu}/{tietbatdau}",
                defaults: new { controller = "MonHoc", action = "Index", mamonhoc = UrlParameter.Optional, nhom = UrlParameter.Optional, thu = UrlParameter.Optional, tietbatdau = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{mssv}/{mahocky}/{matuan}",
                defaults: new { controller = "Default", action = "Index", mssv = UrlParameter.Optional, mahocky = UrlParameter.Optional, matuan = UrlParameter.Optional }
            );
        }
    }
}