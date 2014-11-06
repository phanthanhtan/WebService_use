using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebService_use.Models;

namespace WebService_use.Models
{
    public class ThongTinController : Controller
    {
        //
        // GET: /ThongTin/

        public ActionResult Index(String mssv)//mssv la alert
        {
            try
            {
                if (mssv != null)
                    ViewBag.Alert = mssv;
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.dsTK = entity.ThongTin.Select(item => new Select_Index { MaSinhVien = item.MaSinhVien, TenSinhVien = item.TenSinhVien }).ToList();
                int count = ((List<Select_Index>)ViewBag.dsTK).Count;
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
        [HttpPost]
        public ActionResult Submit(FormCollection fc)
        {
            try
            {
                String submit = fc["submit"].ToString();
                String mssv = fc["txtMaSinhVien"].ToString();
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
                return RedirectToAction("Index/SubmitError");
            }
        }
        public ActionResult Xem(String mssv)
        {
            try
            {//load thong tin tu csdl
                String[] mssv_alert = mssv.Split('_');
                mssv = mssv_alert[0];
                TTDTEntities entity = new TTDTEntities();
                ViewBag.strThongTin = entity.ThongTin.Where(item => item.MaSinhVien == mssv).FirstOrDefault();
                if (ViewBag.strThongTin != null)//co du lieu
                {
                    ViewBag.OK = 1;//xem
                    if (mssv_alert.Count() > 1 && mssv_alert[1] == "CapNhatOk")
                        ViewBag.OK = 5;//mới cập nhật
                    return View();
                }
                else
                    return RedirectToAction("Index/XemNull");
            }
            catch
            {
                return RedirectToAction("Index/XemError");
            }
        }
        public ActionResult Xoa(String mssv)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                ViewBag.strThongTin = entity.ThongTin.Where(item => item.MaSinhVien == mssv).FirstOrDefault();
                if (ViewBag.strThongTin != null)//kiểm tra có thông tin cá nhân theo mssv không
                {
                    try
                    {
                        TTDTEntities entity1 = new TTDTEntities(); //do sử dụng entity trên để kiểm tra => tạo entity1 để xóa
                        ThongTin tt = entity1.ThongTin.Single(item => item.MaSinhVien.Equals(mssv));
                        entity1.ThongTin.Remove(tt);
                        //entity1.ThongTin.Attach(tk);
                        //entity1.Entry(tk).State = EntityState.Deleted;
                        entity1.SaveChanges();
                        return RedirectToAction("Index/XoaOk");
                    }
                    catch
                    {
                        return RedirectToAction("Index/XoaLink");
                    }
                }
                else
                    return RedirectToAction("Index/XoaNull");
            }
            catch
            {
                return RedirectToAction("Index/XoaEror");
            }
        }
        public ActionResult CapNhat(String mssv)
        {
            try
            {
                thongtindaotao.Service ttdt = new thongtindaotao.Service();
                String str = ttdt.XemThongTin(mssv).ToString();
                //kiểm tra có mã sv vừa nhập không
                //không có => Không tìm thấy mã vừa nhập
                //có trả về chuỗi json = > {...}
                if (str[0].ToString() == "K") //kiểm tra ký tự đầu
                {
                    return RedirectToAction("Index/CapNhatNull");
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
                    ThongTin tk = new ThongTin();
                    tk.MaSinhVien = data["taikhoan"]["masinhvien"];
                    tk.TenSinhVien = data["taikhoan"]["tensinhvien"];
                    String lop = data["taikhoan"]["lop"];//DCT1105();
                    tk.Lop = lop;
                    String MaNganh = lop.Substring(1, 2);//CT
                    String MaKhoa = mssv.Substring(4, 2);//3110410126 => 41
                    String MaHeDaoTao = lop.Substring(0, 1) + lop.Substring(3, 1);//D1
                    String MaKhoaHoc = lop.Substring(0, 6);//DCT110
                    String TenNganh = data["taikhoan"]["nganh"];
                    String TenKhoa = data["taikhoan"]["khoa"];
                    String TenHeDaoTao = data["taikhoan"]["hedaotao"];
                    String TenKhoaHoc = data["taikhoan"]["khoahoc"];
                    Nganh nganh = new Nganh();
                    nganh.MaNganh = MaNganh;
                    nganh.TenNganh = TenNganh;
                    LuuNganh(nganh);
                    Khoa khoa = new Khoa();
                    khoa.MaKhoa = MaKhoa;
                    khoa.TenKhoa = TenKhoa;
                    LuuKhoa(khoa);
                    HeDaoTao hdt = new HeDaoTao();
                    hdt.MaHeDaoTao = MaHeDaoTao;
                    hdt.TenHeDaoTao = TenHeDaoTao;
                    LuuHeDaoTao(hdt);
                    KhoaHoc kh = new KhoaHoc();
                    kh.MaKhoaHoc = MaKhoaHoc;
                    kh.TenKhoaHoc = TenKhoaHoc;
                    LuuKhoaHoc(kh);

                    tk.MaNganh = MaNganh;
                    tk.MaKhoa = MaKhoa;
                    tk.MaHeDaoTao = MaHeDaoTao;
                    tk.MaKhoaHoc = MaKhoaHoc;
                    try //không kiểm tra CSDL => chưa có mã sv => try => insert
                    {
                        TTDTEntities entity = new TTDTEntities();
                        entity.ThongTin.Add(tk);
                        entity.SaveChanges();//cập nhập (thêm) thành công
                        return RedirectToAction("Xem/" + mssv + "_CapNhatOk");
                    }
                    catch//có mã sv rồi => catch => update
                    {
                        try
                        {
                            TTDTEntities entity = new TTDTEntities();
                            entity.ThongTin.Attach(tk);
                            entity.Entry(tk).State = EntityState.Modified;
                            entity.SaveChanges();//cập nhập thành công
                            return RedirectToAction("Xem/" + mssv + "_CapNhatOk");
                        }
                        catch
                        {
                            return RedirectToAction("Index/CapNhatError");//lỗi
                        }
                    }
                }
            }
            catch
            {
                return RedirectToAction("Index/CapNhatError");
                //ViewBag.Ok = 6;//lỗi khi cập nhật
            }
        }
        public void LuuNganh(Nganh nganh)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                entity.Nganh.Add(nganh);
                entity.SaveChanges();
            }
            catch
            {
                TTDTEntities entity = new TTDTEntities();
                entity.Nganh.Attach(nganh);
                entity.Entry(nganh).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
        public void LuuKhoa(Khoa khoa)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                entity.Khoa.Add(khoa);
                entity.SaveChanges();
            }
            catch
            {
                TTDTEntities entity = new TTDTEntities();
                entity.Khoa.Attach(khoa);
                entity.Entry(khoa).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
        public void LuuHeDaoTao(HeDaoTao hdt)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                entity.HeDaoTao.Add(hdt);
                entity.SaveChanges();
            }
            catch
            {
                TTDTEntities entity = new TTDTEntities();
                entity.HeDaoTao.Attach(hdt);
                entity.Entry(hdt).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
        public void LuuKhoaHoc(KhoaHoc kh)
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                entity.KhoaHoc.Add(kh);
                entity.SaveChanges();
            }
            catch
            {
                TTDTEntities entity = new TTDTEntities();
                entity.KhoaHoc.Attach(kh);
                entity.Entry(kh).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
    }
}
