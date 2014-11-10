using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService_use.Models;

namespace WebService_use.Models
{
    public class SXTKB_MonHocController : Controller
    {
        //
        // GET: /MonHoc/

        public ActionResult Index(String mamonhoc)//mamonhoc la alert
        {
            try
            {
                if (mamonhoc != null)
                    ViewBag.Alert = mamonhoc;
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_MonHoc = entity.SXTKB_MonHoc.Distinct().ToList();
                int count = ((List<SXTKB_MonHoc>)ViewBag.SXTKB_MonHoc).Count;
                if (count != 0)
                    ViewBag.Ok = "Index1";//có dữ liệu
                else
                    ViewBag.Ok = "Index2";//không có dữ liệu
            }
            catch
            {
                ViewBag.Ok = "";//lỗi
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult Submit(FormCollection fc)
        //{
        //    try
        //    {
        //        String submit = fc["submit"].ToString();
        //        String mamonhoc = fc["txtMaMonHoc"].ToString();
        //        switch (submit)
        //        {
        //            case "Xem":
        //                return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc);
        //            case "Sửa":
        //                return RedirectToAction("../SXTKB/SXTKB_MonHoc/SuaMonHoc/" + mamonhoc);
        //            case "Xóa":
        //                return RedirectToAction("../SXTKB/SXTKB_MonHoc/XoaMonHoc/" + mamonhoc);
        //            default:
        //                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/SubmitError");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/SubmitError");//lỗi
        //    }
        //}
        public ActionResult XemMonHoc(String mamonhoc)
        {
            try
            {
                String[] mamonhoc_alert = mamonhoc.Split('_');
                mamonhoc = mamonhoc_alert[0];
                if (mamonhoc_alert.Count() > 1)
                    ViewBag.Alert = mamonhoc_alert[1];//CapNhatOk
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_MonHoc = entity.SXTKB_MonHoc.Where(item => item.MaMonHoc.Equals(mamonhoc)).FirstOrDefault();
                if (ViewBag.SXTKB_MonHoc != null)
                {
                    TTDTEntities entity1 = new TTDTEntities();
                    ViewBag.SXTKB_NhomMonHoc = entity1.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc == mamonhoc).ToList();
                    return View();//có dữ liệu
                }
                else
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/XemNull");//không có dữ liệu
            }
            catch
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/XemError");//lỗi
            } 
        }
        public ActionResult XoaNhomMonHoc(String mamonhoc, String nhom, String thu, String tietbatdau)
        {
            try
            {
                int _Thu = int.Parse(thu);
                int _TietBatDau = int.Parse(tietbatdau);
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_NhomMonHoc = entity.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc == mamonhoc && item.Nhom == nhom && item.Thu == _Thu && item.TietBatDau == _TietBatDau).FirstOrDefault();
                if (ViewBag.SXTKB_NhomMonHoc != null)
                {
                    SXTKB_NhomMonHoc Nmh = (SXTKB_NhomMonHoc)ViewBag.SXTKB_NhomMonHoc;
                    TTDTEntities entity1 = new TTDTEntities();
                    SXTKB_NhomMonHoc nmh = entity1.SXTKB_NhomMonHoc.Single(item => item.MaMonHoc.Equals(Nmh.MaMonHoc) && item.Nhom.Equals(Nmh.Nhom) &&item.Thu.Equals(Nmh.Thu) &&item.TietBatDau.Equals(Nmh.TietBatDau));
                    entity1.SXTKB_NhomMonHoc.Remove(nmh);
                    entity1.SaveChanges();
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_XoaNhomMonHocOk");//xóa thành công
                }
                else//không có trong CSDL => không xóa
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_XoaNhomMonHocNull");
            }
            catch
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_XoaNhomMonHocError");//lỗi
            }
        }
        public ActionResult XoaMonHoc(String mamonhoc)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_MonHoc = entity.SXTKB_MonHoc.Where(item => item.MaMonHoc == mamonhoc).FirstOrDefault();
                if (ViewBag.SXTKB_MonHoc != null)//kiểm tra có môn học không
                {
                    //xóa nhóm môn học
                    //xóa môn học
                    TTDTEntities entity1 = new TTDTEntities();
                    List<SXTKB_NhomMonHoc> ListNhomMonHoc = entity1.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc == mamonhoc).ToList();
                    foreach (SXTKB_NhomMonHoc Nmh in ListNhomMonHoc)
                    {
                        TTDTEntities entity2 = new TTDTEntities();
                        SXTKB_NhomMonHoc nmh = entity2.SXTKB_NhomMonHoc.Single(item => item.MaMonHoc.Equals(Nmh.MaMonHoc) && item.Nhom.Equals(Nmh.Nhom) && item.Thu.Equals(Nmh.Thu) && item.TietBatDau.Equals(Nmh.TietBatDau));
                        entity2.SXTKB_NhomMonHoc.Remove(nmh);
                        entity2.SaveChanges();
                    }
                    TTDTEntities entity3 = new TTDTEntities();
                    SXTKB_MonHoc mh = entity3.SXTKB_MonHoc.Single(item => item.MaMonHoc == mamonhoc);
                    entity3.SXTKB_MonHoc.Remove(mh);
                    entity3.SaveChanges();
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/XoaOk");//xóa thành công
                }
                else//không có trong CSDL => không xóa
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/XoaNull"); 
            }
            catch
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/XoaError");//lỗi
            }
        }
        public ActionResult ThemMonHoc()
        {
            ViewBag.Action = "Them";
            return View("MonHoc");
        }
        [HttpPost]
        public ActionResult ThemMonHocForm(FormCollection fc)//Thêm môn => kiểm tra đã có môn đó trong CSDL chưa
        {
            try
            {
                SXTKB_MonHoc mh = new SXTKB_MonHoc();
                String mamonhoc = fc["txtMaMonHoc"].ToString();
                mh.MaMonHoc = mamonhoc;
                mh.TenMonHoc = fc["txtTenMonHoc"].ToString();
                try//thêm môn học
                {
                    TTDTEntities entity = new TTDTEntities();
                    entity.SXTKB_MonHoc.Add(mh);
                    entity.SaveChanges();
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_ThemMonHocOk");
                }
                catch//mã môn học đã có => chuyển đến trang xem của mã môn học đó
                {
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_MaMonHocExist");
                }
            }
            catch//lỗi
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/ThemMonHocError");
            }
        }
        public bool Check(int _Thu, int _TietBatDau, int _SoTiet)
        {
            if (_Thu >= 2 && _Thu <= 8 && _TietBatDau > 0 && _TietBatDau <= 13 && _SoTiet <= (13 - _TietBatDau + 1))
                return true;
            else
                return false;
        }
        [HttpPost]
        public ActionResult ThemNhomMonHoc(FormCollection fc)
        {
            try
            {
                String mamonhoc = fc["txtMaMonHoc"].ToString();
                int thu = int.Parse(fc["txtThu"].ToString());
                int tietbatdau = int.Parse(fc["txtTietBatDau"].ToString());
                int sotiet = int.Parse(fc["txtSoTiet"].ToString());
                if (Check(thu, tietbatdau, sotiet))
                {
                    SXTKB_NhomMonHoc nmh = new SXTKB_NhomMonHoc();
                    nmh.MaMonHoc = mamonhoc;
                    nmh.Nhom = fc["txtNhom"].ToString();
                    nmh.Thu = thu;
                    nmh.TietBatDau = tietbatdau;
                    nmh.SoTiet = sotiet;
                    TTDTEntities entity = new TTDTEntities();
                    entity.SXTKB_NhomMonHoc.Add(nmh);
                    entity.SaveChanges();
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_ThemNhomMonHocOk");
                }
                else
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_ThemNhomMonHocErrorCheck");
            }
            catch//lỗi
            {
                String mamonhoc = fc["txtMaMonHoc"].ToString();
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_ThemNhomMonHocError");
            }
        }
        public ActionResult SuaMonHoc(String mamonhoc)
        {
            try
            {
                ViewBag.Action = "Sua";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_MonHoc = entity.SXTKB_MonHoc.Where(item => item.MaMonHoc.Equals(mamonhoc)).FirstOrDefault();
                if(ViewBag.SXTKB_MonHoc != null)
                    return View("MonHoc");
                else
                    return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/SuaError");
            }
            catch
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/SuaError");
            }
        }
        [HttpPost]
        public ActionResult SuaMonHocForm(FormCollection fc)
        {
            try
            {
                SXTKB_MonHoc mh = new SXTKB_MonHoc();
                String mamonhoc = fc["txtMaMonHoc"].ToString();
                mh.MaMonHoc = mamonhoc;
                mh.TenMonHoc = fc["txtTenMonHoc"].ToString();
                TTDTEntities entity = new TTDTEntities();
                entity.SXTKB_MonHoc.Attach(mh);
                entity.Entry(mh).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/XemMonHoc/" + mamonhoc + "_SuaMonHocOk");
            }
            catch//lỗi
            {
                return RedirectToAction("../SXTKB/SXTKB_MonHoc/Index/SuaMonHocError");
            }
        }
    }
}
