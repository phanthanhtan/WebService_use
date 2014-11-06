using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebService_use.Models;

namespace WebService_use.Models
{
    public class ThoiKhoaBieuController : Controller
    {
        //
        // GET: /ThoiKhoaBieu/

        public ActionResult Index(String mssv)//mssv la alert
        {
            try
            {
                if (mssv != null)
                    ViewBag.Alert = mssv;
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.ThoiKhoaBieu = entity.ThoiKhoaBieu.Select(item => new Select_Index { MaSinhVien = item.MaSinhVien, TenSinhVien = item.ThongTin.TenSinhVien }).Distinct().ToList();
                int count = ((List<Select_Index>)ViewBag.ThoiKhoaBieu).Count;
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
        public ActionResult Xoa(String mssv, String mahocky)
        {
            try
            {//kiểm tra mssv
                TTDTEntities entity = new TTDTEntities();
                ViewBag.TKB = entity.ThoiKhoaBieu.Where(item => item.MaSinhVien.Equals(mssv)).ToList();
                int count = ((List<ThoiKhoaBieu>)ViewBag.TKB).Count;
                if (count != 0)//có dữ liệu
                {
                    int dem = 0;
                    foreach (ThoiKhoaBieu tkb in (List<ThoiKhoaBieu>)ViewBag.TKB)
                    {
                        if (mahocky == null)//xóa tất cả học kỳ của 1 sinh viên
                        {
                            Xoa_MaHocKy(tkb);
                            dem++;
                        }
                        else//xóa theo mã học kỳ của 1 svinh viên
                            if (tkb.MaHocKy == mahocky)
                            {
                                Xoa_MaHocKy(tkb);
                                dem++;
                            }
                    }
                    if (dem != 0)
                        return RedirectToAction("Index/XoaOk");//Xóa thành công
                    else
                        return RedirectToAction("Index/XoaNullMaHocKy");//không có mã học kỳ cần xóa

                }
                else//không có dữ liệu
                    return RedirectToAction("Index/XoaNull");//mssv nhập không có trong csdl => không xóa, /LichThi/Xoa/31104... 
            }
            catch
            {
                return RedirectToAction("Index/XoaError");//lỗi khi xóa
            }
        }
        public void Xoa_MaHocKy(ThoiKhoaBieu tkb)
        {
            TTDTEntities entity = new TTDTEntities();
            ThoiKhoaBieu _tkb = entity.ThoiKhoaBieu.Single(item => item.MaSinhVien.Equals(tkb.MaSinhVien) && item.MaHocKy.Equals(tkb.MaHocKy) && item.MaTuan.Equals(tkb.MaTuan) && item.MaMonHoc.Equals(tkb.MaMonHoc) && item.Thu.Equals(tkb.Thu) && item.TietBatDau.Equals(tkb.TietBatDau));
            entity.ThoiKhoaBieu.Remove(_tkb);
            entity.SaveChanges();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            try
            {
                System.Collections.Specialized.NameValueCollection nv = Request.Form;
                String submit = nv["submit"].ToString();
                String mssv = nv["txtMaSinhVien"].ToString();
                switch (submit)
                {
                    case "Xem":
                        return RedirectToAction("Xem/" + mssv);
                    case "Cập nhật":
                        return RedirectToAction("CapNhat/" + mssv);
                    case "Xóa":
                        return RedirectToAction("Xoa/" + mssv);
                    default:
                        return RedirectToAction("Index/SubmitError");
                }
            }
            catch
            {
                return RedirectToAction("Index/SubmitError");//lỗi
            }
        }
        public ActionResult Xem(String mssv, String mahocky, String matuan)
        {
            try
            {//load thong tin tu csdl
                String[] mssv_alert = mssv.Split('_');
                mssv = mssv_alert[0];
                if (mssv_alert.Count() > 1)
                    ViewBag.Alert = mssv_alert[1];//CapNhatOk
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                int count;
                if (mahocky == null)//tất cả học kỳ
                {
                    ViewBag.ThoiKhoaBieu = entity.ThoiKhoaBieu.Where(item => item.MaSinhVien.Equals(mssv)).Select(item => new ThoiKhoaBieu_Xem { MaSinhVien = item.MaSinhVien, MaHocKy = item.MaHocKy, TenHocKy = item.HocKy.TenHocKy }).Distinct().OrderByDescending(item => item.MaHocKy).ToList();
                    count = ((List<ThoiKhoaBieu_Xem>)ViewBag.ThoiKhoaBieu).Count;
                    ViewBag.Check = 1;//theo mã sv
                }
                else//mahocky
                {
                    ViewBag.ThoiKhoaBieu = entity.ThoiKhoaBieu.Where(item => item.MaSinhVien.Equals(mssv) && item.MaHocKy.Equals(mahocky)).Select(item => new ThoiKhoaBieu_Xem_MaHocKy { MaSinhVien = item.MaSinhVien, MaHocKy = item.MaHocKy, TenHocKy = item.HocKy.TenHocKy, MaTuan = item.MaTuan, TenTuan = item.Tuan.TenTuan }).Distinct().OrderBy(item => item.MaTuan).ToList();
                    count = ((List<ThoiKhoaBieu_Xem_MaHocKy>)ViewBag.ThoiKhoaBieu).Count;
                    String _maTuan;
                    if (matuan == null)
                        //lấy tuần đầu tiên của combobox để show
                        _maTuan = ((List<ThoiKhoaBieu_Xem_MaHocKy>)ViewBag.ThoiKhoaBieu)[0].MaTuan;
                    else
                        _maTuan = matuan;
                    ViewBag.MaTuan = _maTuan;
                    ViewBag.ThoiKhoaBieu_Tuan = entity.ThoiKhoaBieu.Where(item => item.MaSinhVien.Equals(mssv) && item.MaHocKy.Equals(mahocky) && item.MaTuan.Equals(_maTuan)).OrderBy(item => new { item.Thu, item.TietBatDau }).ToList();
                    ViewBag.Check = 2;//chi tiết theo mã học kỳ
                }
                if (count != 0)
                {
                    ViewBag.ThongTin = new TTDTEntities().ThongTin.Where(item => item.MaSinhVien.Equals(mssv)).FirstOrDefault();
                    return View();//có dữ liệu
                }
                else
                    return RedirectToAction("Index/XemNull");//không có dữ liệu
            }
            catch
            {
                return RedirectToAction("Index/XemError");//lỗi => load khong dc
            }
        }
        public ActionResult CapNhat(String mssv, String mahocky, String matuan) //mahocky = null
        {
            try
            {
                thongtindaotao.Service ttdt = new thongtindaotao.Service();
                ttdt.Timeout = 3600000;
                String str = ttdt.XemTKB(mssv).ToString();
                //Lop, TenMonHoc, MaMonHoc, Thu, SoTinChi, PhongHoc, TietBatDau, SoTiet, GiangVien, NgayBatDau, NgayKetThuc
                //String str = "{\"taikhoan\":{\"masinhvien\":\"3110410126\",\"tensinhvien\":\"Phan Thanh T\\u00e2n\"},\"hocky\":[{\"mahocky\":\"20141\",\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 - N\\u0103m h\\u1ecdc 2014-2015\",\"tuan\":[{\"matuan\":\"Tu\\u1ea7n 07 [T\\u1eeb 20/10/2014 -- \\u0110\\u1ebfn 26/10/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 01 [T\\u1eeb 08/09/2014 -- \\u0110\\u1ebfn 14/09/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 02 [T\\u1eeb 15/09/2014 -- \\u0110\\u1ebfn 21/09/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 03 [T\\u1eeb 22/09/2014 -- \\u0110\\u1ebfn 28/09/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 04 [T\\u1eeb 29/09/2014 -- \\u0110\\u1ebfn 05/10/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 05 [T\\u1eeb 06/10/2014 -- \\u0110\\u1ebfn 12/10/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 06 [T\\u1eeb 13/10/2014 -- \\u0110\\u1ebfn 19/10/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 08 [T\\u1eeb 27/10/2014 -- \\u0110\\u1ebfn 02/11/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 09 [T\\u1eeb 03/11/2014 -- \\u0110\\u1ebfn 09/11/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 10 [T\\u1eeb 10/11/2014 -- \\u0110\\u1ebfn 16/11/2014]\",\"monhoc\":[[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"7\",\"3\",\"C.A502\",\"3\",\"3\",\"Hu\\u1ef3nh Th\\u1eafng \\u0110\\u01b0\\u1ee3c\",\"13/09/2014\",\"16/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"841072\",\"5\",\"3\",\"C.A110\",\"4\",\"2\",\"\\u0110\\u1ed7 Ng\\u1ecdc Nh\\u01b0 Loan\",\"25/09/2014\",\"14/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"6\",\"3\",\"C.A106\",\"6\",\"2\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"26/09/2014\",\"15/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"841071\",\"4\",\"3\",\"C.A501\",\"8\",\"3\",\"Ph\\u00f9ng Th\\u00e1i Thi\\u00ean Trang\",\"10/09/2014\",\"13/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 11 [T\\u1eeb 17/11/2014 -- \\u0110\\u1ebfn 23/11/2014]\",\"monhoc\":[[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 12 [T\\u1eeb 24/11/2014 -- \\u0110\\u1ebfn 30/11/2014]\",\"monhoc\":[[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"4\",\"4\",\"C.A501\",\"6\",\"2\",\"L\\u00ea Ng\\u1ecdc Anh\",\"10/09/2014\",\"27/11/2014\",\"\",\"420\",\"\"],[\"DCT1101\",\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"841073\",\"3\",\"4\",\"C.HTC\",\"8\",\"3\",\"L\\u00ea Ng\\u1ecdc Anh\",\"09/09/2014\",\"26/11/2014\",\"\",\"420\",\"\"]]},{\"matuan\":\"Tu\\u1ea7n 13 [T\\u1eeb 01/12/2014 -- \\u0110\\u1ebfn 07/12/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 14 [T\\u1eeb 08/12/2014 -- \\u0110\\u1ebfn 14/12/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 15 [T\\u1eeb 15/12/2014 -- \\u0110\\u1ebfn 21/12/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 16 [T\\u1eeb 22/12/2014 -- \\u0110\\u1ebfn 28/12/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 17 [T\\u1eeb 29/12/2014 -- \\u0110\\u1ebfn 04/01/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 18 [T\\u1eeb 05/01/2015 -- \\u0110\\u1ebfn 11/01/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 19 [T\\u1eeb 12/01/2015 -- \\u0110\\u1ebfn 18/01/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 20 [T\\u1eeb 19/01/2015 -- \\u0110\\u1ebfn 25/01/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 21 [T\\u1eeb 26/01/2015 -- \\u0110\\u1ebfn 01/02/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 22 [T\\u1eeb 02/02/2015 -- \\u0110\\u1ebfn 08/02/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 23 [T\\u1eeb 09/02/2015 -- \\u0110\\u1ebfn 15/02/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 24 [T\\u1eeb 16/02/2015 -- \\u0110\\u1ebfn 22/02/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 25 [T\\u1eeb 23/02/2015 -- \\u0110\\u1ebfn 01/03/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 26 [T\\u1eeb 02/03/2015 -- \\u0110\\u1ebfn 08/03/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 27 [T\\u1eeb 09/03/2015 -- \\u0110\\u1ebfn 15/03/2015]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 28 [T\\u1eeb 16/03/2015 -- \\u0110\\u1ebfn 22/03/2015]\",\"monhoc\":null}]},{\"mahocky\":\"20133\",\"tenhocky\":\"H\\u1ecdc k\\u1ef3 3 - N\\u0103m h\\u1ecdc 2013-2014\",\"tuan\":[{\"matuan\":\"Tu\\u1ea7n 47 [T\\u1eeb 14/07/2014 -- \\u0110\\u1ebfn 20/07/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 48 [T\\u1eeb 21/07/2014 -- \\u0110\\u1ebfn 27/07/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 49 [T\\u1eeb 28/07/2014 -- \\u0110\\u1ebfn 03/08/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 50 [T\\u1eeb 04/08/2014 -- \\u0110\\u1ebfn 10/08/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 51 [T\\u1eeb 11/08/2014 -- \\u0110\\u1ebfn 17/08/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 52 [T\\u1eeb 18/08/2014 -- \\u0110\\u1ebfn 24/08/2014]\",\"monhoc\":null},{\"matuan\":\"Tu\\u1ea7n 53 [T\\u1eeb 25/08/2014 -- \\u0110\\u1ebfn 31/08/2014]\",\"monhoc\":null}]}]}<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE--> <center><a href=\"http://somee.com\">Web hosting by Somee.com</a></center> </textarea></xml></script></noframes></noscript></object></layer></style></title></applet> <script language=\"JavaScript\" src=\"http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js\"></script> <!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->";
                //kiểm tra có mã sv vừa nhập không
                //không có => Không tìm thấy mã vừa nhập
                //có trả về chuỗi json = > {...}
                if (str[0].ToString() == "K") //kiểm tra ký tự đầu
                {
                    return RedirectToAction("Index/CapNhatNull");//không có mã sv vừa nhập => không cập nhật
                }
                else
                {//có json
                    //cắt chuỗi => bỏ quảng cáo của host
                    int length = str.Length;
                    int end = length;
                    for (int i = length - 1; i >= 0; i--)
                    {
                        if (str[i].ToString() == "}")
                        {//0-7 => 8 ký tự, 0-end => end+1 ký tự
                            end = i + 1; //cắt từ 0, số ký tự cần lấy
                            break;
                        }
                    }
                    String jsonString = str.Substring(0, end);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    dynamic data = js.Deserialize<dynamic>(jsonString);
                    String maSinhVien = data["taikhoan"]["masinhvien"];
                    String tenSinhVien = data["taikhoan"]["tensinhvien"];
                    try//thêm vào ThongTin => không đc => đã có
                    {
                        if (maSinhVien != "" && maSinhVien != null)
                        {
                            ThongTin tt = new ThongTin();
                            tt.MaSinhVien = maSinhVien;
                            tt.TenSinhVien = tenSinhVien;
                            TTDTEntities entity = new TTDTEntities();
                            entity.ThongTin.Add(tt);
                            entity.SaveChanges();
                        }
                    }
                    catch { }//có phần cập nhật ThongTin rồi => không cập nhật tại đây
                    int hocky = ((ICollection)data["hocky"]).Count;
                    int dem = 0;//kiểm tra xem có mã học kỳ cần cập nhật không
                    for (int i = 0; i < hocky; i++)
                    {
                        String maHocKy = data["hocky"][i]["mahocky"];//nếu muốn cập nhật theo mahocky => if tại đây
                        dynamic dataHocKy = data["hocky"][i];
                        if (mahocky != null)
                        {
                            if (maHocKy == mahocky)
                            {
                                Luu_MaHocky(maSinhVien, maHocKy, dataHocKy);
                                dem++;
                            }
                        }
                        else
                        {
                            Luu_MaHocky(maSinhVien, maHocKy, dataHocKy);
                            dem++;
                        }
                    }
                    if (dem != 0)
                    {
                        return RedirectToAction("Xem/" + mssv + "_CapNhatOk");
                        //ViewBag.Ok = "capnhat2";//cập nhập (thêm) thành công
                        //ViewBag.Mssv = mssv;
                    }
                    else
                        return RedirectToAction("Index/CapNhatNullMaHocKy");//không có mã học kỳ cần cập nhật
                }
            }
            catch
            {
                return RedirectToAction("Index/CapNhatError");//lỗi khi cập nhật
            }
        }
        public void Luu_MaHocky(String maSinhVien, String maHocKy, dynamic dataHocKy)
        {
            String tenHocKy = dataHocKy["tenhocky"];
            int tuan = ((ICollection)dataHocKy["tuan"]).Count;//tuần không null => khỏi kiểm tra
            for (int j = 0; j < tuan; j++)
            {
                ThoiKhoaBieu TKB = new ThoiKhoaBieu();
                TKB.MaSinhVien = maSinhVien;
                TKB.MaHocKy = maHocKy;
                HocKy hk = new HocKy();
                hk.MaHocKy = maHocKy;
                hk.TenHocKy = tenHocKy;
                try//lưu HocKy => k đc => đã có
                {
                    TTDTEntities entity = new TTDTEntities();
                    entity.HocKy.Add(hk);
                    entity.SaveChanges();
                }
                catch//update
                {
                    //TTDTEntities entity = new TTDTEntities();
                    //entity.HocKy.Attach(hk);
                    //entity.Entry(hk).State = EntityState.Modified;
                    //entity.SaveChanges();
                }
                dynamic dataTuan = dataHocKy["tuan"][j];
                String _tenTuan = dataTuan["matuan"]; //Tuần 01 [Từ dd/mm/yyyy -- đến dd/mm/yyyy]
                String _maTuan = maHocKy + _tenTuan.Substring(5, 2);//2010 1 01
                Tuan _tuan = new Tuan();
                _tuan.MaTuan = _maTuan;
                _tuan.TenTuan = _tenTuan;
                try//lưu Tuan => k đc => đã có
                {
                    TTDTEntities entity = new TTDTEntities();
                    entity.Tuan.Add(_tuan);
                    entity.SaveChanges();
                }
                catch//update
                {
                    //TTDTEntities entity = new TTDTEntities();
                    //entity.Tuan.Attach(_tuan);
                    //entity.Entry(_tuan).State = EntityState.Modified;
                    //entity.SaveChanges();
                }
                TKB.MaTuan = _maTuan;//nếu chọn lưu theo số tuần mặc định 5 10 15 ... => kiểm tra matuan != null mới được lưu
                if (dataTuan["monhoc"] != null)//có môn học trong tuần
                {
                    int monhoc = ((ICollection)dataTuan["monhoc"]).Count;
                    for (int k = 0; k < monhoc; k++)
                    {
                        dynamic dataMonHoc = dataTuan["monhoc"][k];
                        String maMonHoc = dataMonHoc[2];
                        TKB.MaMonHoc = maMonHoc;
                        String tenMonHoc = dataMonHoc[1];
                        MonHoc mh = new MonHoc();
                        mh.MaMonHoc = maMonHoc;
                        mh.TenMonHoc = tenMonHoc;
                        try//lưu MonHoc => k đc => đã có
                        {
                            TTDTEntities entity = new TTDTEntities();
                            entity.MonHoc.Add(mh);
                            entity.SaveChanges();
                        }
                        catch//update
                        {
                            //TTDTEntities entity = new TTDTEntities();
                            //entity.MonHoc.Attach(mh);
                            //entity.Entry(mh).State = EntityState.Modified;
                            //entity.SaveChanges();
                        }
                        TKB.PhongHoc = dataMonHoc[5];
                        TKB.Thu = dataMonHoc[3];
                        TKB.TietBatDau = int.Parse(dataMonHoc[6]);
                        TKB.SoTiet = int.Parse(dataMonHoc[7]);
                        TKB.GiangVien = dataMonHoc[8];
                        TKB.Lop = dataMonHoc[0];
                        LuuThoiKhoaBieu(TKB);//lưu LuuThoiKhoaBieu
                    }
                }
                else//không có môn học nào trong tuần
                {//nếu không muốn lưu các tuần TKB null => else không làm gì hết
                    TKB.MaMonHoc = "0";
                    TKB.Thu = "0";
                    TKB.TietBatDau = 0;
                    LuuThoiKhoaBieu(TKB);//lưu LuuThoiKhoaBieu
                }
            }
        }
        public void LuuThoiKhoaBieu(ThoiKhoaBieu tkb)
        {
            TTDTEntities entity = new TTDTEntities();
            try //không kiểm tra CSDL => chưa có TKB => try => insert
            {
                entity.ThoiKhoaBieu.Add(tkb);
                entity.SaveChanges();
            }
            catch//có TKB rồi => catch => update
            {
                entity.ThoiKhoaBieu.Attach(tkb);
                entity.Entry(tkb).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
    }
}
