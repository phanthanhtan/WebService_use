using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebService_use.Models;

namespace WebService_use.Controllers
{
    public class DiemController : Controller
    {
        //
        // GET: /Diem/

        public ActionResult Index(String mssv)//mssv la alert
        {
            try
            {
                if (mssv != null)
                    ViewBag.Alert = mssv;
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.Diem_TongQuat = entity.Diem_TongQuat.Select(item => new Select_Index { MaSinhVien = item.MaSinhVien, TenSinhVien = item.ThongTin.TenSinhVien }).Distinct().ToList();
                int count = ((List<Select_Index>)ViewBag.Diem_TongQuat).Count;
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
        public ActionResult Xoa(String mssv, String mahocky)
        {
            try
            {//kiểm tra mssv
                TTDTEntities entity = new TTDTEntities();
                ViewBag.Diem_ChiTiet = entity.Diem_ChiTiet.Where(item => item.MaSinhVien.Equals(mssv)).ToList();
                int count = ((List<Diem_ChiTiet>)ViewBag.Diem_ChiTiet).Count;
                if (count != 0)//có dữ liệu
                {
                    int dem = 0;
                    foreach (Diem_ChiTiet diem_ct in (List<Diem_ChiTiet>)ViewBag.Diem_ChiTiet)
                    {
                        if (mahocky == null)//xóa tất cả học kỳ của 1 sinh viên
                        {
                            Xoa_MaHocKy(diem_ct);
                            dem++;
                        }
                        else//xóa theo mã học kỳ của 1 svinh viên
                            if (diem_ct.MaHocKy == mahocky)
                            {
                                Xoa_MaHocKy(diem_ct);
                                dem++;
                            }
                    }
                    if (dem != 0)
                        return RedirectToAction("Index/XoaOk");
                    else
                        return RedirectToAction("Index/XoaNullMaHocKy");//mã học kỳ cần xóa không có
                }
                else//không có dữ liệu
                    return RedirectToAction("Index/XoaNull");//mssv nhập không có trong csdl => không xóa, /LichThi/Xoa/31104... 
            }
            catch
            {
                return RedirectToAction("Index/XoaError");//lỗi khi xóa
            }
        }
        public void Xoa_MaHocKy(Diem_ChiTiet diem_ct)
        {
            //Xóa điểm chi tiết
            TTDTEntities entity = new TTDTEntities();
            Diem_ChiTiet _diem_ct = entity.Diem_ChiTiet.Single(item => item.MaSinhVien.Equals(diem_ct.MaSinhVien) && item.MaHocKy.Equals(diem_ct.MaHocKy) && item.SoThuTu.Equals(diem_ct.SoThuTu));
            entity.Diem_ChiTiet.Remove(_diem_ct);
            entity.SaveChanges();
            //xóa điểm tổng quát
            try//nếu là học kỳ 3 => không có Diem_TongQuat
            {
                TTDTEntities entity1 = new TTDTEntities();
                Diem_TongQuat _diem_tq = entity1.Diem_TongQuat.Single(item => item.MaSinhVien.Equals(diem_ct.MaSinhVien) && item.MaHocKy.Equals(diem_ct.MaHocKy));
                entity1.Diem_TongQuat.Remove(_diem_tq);
                entity1.SaveChanges();
            }
            catch { }
        }
        public ActionResult Xem(String mssv, String mahocky)
        {
            try
            {//load thong tin tu csdl
                String[] mssv_alert = mssv.Split('_');
                mssv = mssv_alert[0];
                if (mssv_alert.Count() > 1)
                    ViewBag.Alert = mssv_alert[1];//CapNhatOk
                else
                    ViewBag.Alert = "";
                int count;
                String _mahocky;
                TTDTEntities entity = new TTDTEntities();
                ViewBag.Diem_ChiTiet_Xem = entity.Diem_ChiTiet.Where(item => item.MaSinhVien.Equals(mssv)).Select(item => new Diem_ChiTiet_Xem { MaSinhVien = item.MaSinhVien, MaHocKy = item.MaHocKy, TenHocKy = item.HocKy.TenHocKy }).Distinct().OrderByDescending(item => item.MaHocKy).ToList();
                count = ((List<Diem_ChiTiet_Xem>)ViewBag.Diem_ChiTiet_Xem).Count;
                if (count != 0)
                {
                    ViewBag.ThongTin = new TTDTEntities().ThongTin.Where(item => item.MaSinhVien.Equals(mssv)).FirstOrDefault();
                    if (mahocky == null)
                        _mahocky = ((List<Diem_ChiTiet_Xem>)ViewBag.Diem_ChiTiet_Xem)[0].MaHocKy;
                    else
                        _mahocky = mahocky;
                    ViewBag.MaHocKy = _mahocky;
                    ViewBag.mssv = mssv;
                    ViewBag.Diem_ChiTiet = entity.Diem_ChiTiet.Where(item => item.MaSinhVien.Equals(mssv) && item.MaHocKy.Equals(_mahocky)).ToList();
                    ViewBag.Diem_TongQuat = entity.Diem_TongQuat.Where(item => item.MaSinhVien.Equals(mssv) && item.MaHocKy.Equals(_mahocky)).FirstOrDefault();
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
        public ActionResult CapNhatForm(String mssv, FormCollection fc)
        {
            String mahocky = fc["txtMaHocKy"].ToString();
            return RedirectToAction("CapNhat/" + mssv + "/" + mahocky);
        }
        public ActionResult CapNhat(String mssv, String mahocky) //mahocky = null
        {
            try
            {
                thongtindaotao.Service ttdt = new thongtindaotao.Service();
                String str = ttdt.XemDiem(mssv).ToString();
                //String str = "{\"taikhoan\":{\"masinhvien\":\"3110410126\",\"tensinhvien\":\"Phan Thanh T\\u00e2n\"},\"hocky\":[{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 N\\u0103m h\\u1ecdc 2010\",\"chitiet\":[{\"sothutu\":\"1\",\"mamonhoc\":\"841001\",\"tenmonhoc\":\"Gi\\u1ea3i t\\u00edch 1\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"4.9\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"2\",\"mamonhoc\":\"841004\",\"tenmonhoc\":\"Nh\\u1eadp m\\u00f4n m\\u00e1y t\\u00ednh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"3\",\"mamonhoc\":\"841005\",\"tenmonhoc\":\"\\u0110i\\u1ec7n t\\u1eed c\\u0103n b\\u1ea3n\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"6.8\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"4\",\"mamonhoc\":\"841006\",\"tenmonhoc\":\"To\\u00e1n r\\u1eddi r\\u1ea1c\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.6\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"5\",\"mamonhoc\":\"841020\",\"tenmonhoc\":\"C\\u01a1 s\\u1edf l\\u1eadp tr\\u00ecnh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"4.0\",\"diemthilanmot\":\"1.0\",\"diemtongkethemuoi\":\"2.5\",\"diemtongketdiemchu\":\"F\"},{\"sothutu\":\"6\",\"mamonhoc\":\"862001\",\"tenmonhoc\":\"Gi\\u00e1o d\\u1ee5c th\\u1ec3 ch\\u1ea5t (1)\",\"sotinchi\":\"1\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.0\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"7\",\"mamonhoc\":\"862006\",\"tenmonhoc\":\"GD Qu\\u1ed1c ph\\u00f2ng - An ninh (1)\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"4.2\",\"diemtongketdiemchu\":\"D\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"5.71\",\"diemtrungbinhhockyhebon\":\"1.64\",\"diemtrungbinhtichluyhemuoi\":\"5.71\",\"diemtrungbinhtichluyhebon\":\"1.64\",\"sotinchidat\":\"11\",\"sotinchitichluy\":\"11\",\"diemtrungbinhrenluyenhocky\":\"60\",\"phanloaidtbrenluyenhocky\":\"Trung b\\u00ecnh kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 2 N\\u0103m h\\u1ecdc 2010\",\"chitiet\":[{\"sothutu\":\"8\",\"mamonhoc\":\"841002\",\"tenmonhoc\":\"Gi\\u1ea3i t\\u00edch 2\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"5.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"3.6\",\"diemtongketdiemchu\":\"F\"},{\"sothutu\":\"9\",\"mamonhoc\":\"841003\",\"tenmonhoc\":\"\\u0110\\u1ea1i s\\u1ed1 tuy\\u1ebfn t\\u00ednh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"4.2\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"10\",\"mamonhoc\":\"841021\",\"tenmonhoc\":\"Ki\\u1ebfn tr\\u00fac m\\u00e1y t\\u00ednh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"3.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"4.5\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"11\",\"mamonhoc\":\"841040\",\"tenmonhoc\":\"K\\u0129 thu\\u1eadt l\\u1eadp tr\\u00ecnh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.5\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"12\",\"mamonhoc\":\"841042\",\"tenmonhoc\":\"C\\u1ea5u tr\\u00fac d\\u1eef li\\u1ec7u v\\u00e0 gi\\u1ea3i thu\\u1eadt\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.0\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"13\",\"mamonhoc\":\"862002\",\"tenmonhoc\":\"Gi\\u00e1o d\\u1ee5c th\\u1ec3 ch\\u1ea5t (2)\",\"sotinchi\":\"1\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"8.0\",\"diemtongkethemuoi\":\"7.5\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"14\",\"mamonhoc\":\"862007\",\"tenmonhoc\":\"GD Qu\\u1ed1c ph\\u00f2ng - An ninh (2)\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"5.9\",\"diemtongketdiemchu\":\"C\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"6.06\",\"diemtrungbinhhockyhebon\":\"2.00\",\"diemtrungbinhtichluyhemuoi\":\"5.93\",\"diemtrungbinhtichluyhebon\":\"1.87\",\"sotinchidat\":\"22\",\"sotinchitichluy\":\"33\",\"diemtrungbinhrenluyenhocky\":\"57\",\"phanloaidtbrenluyenhocky\":\"Trung b\\u00ecnh\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 3 N\\u0103m h\\u1ecdc 2010\",\"chitiet\":[{\"sothutu\":\"15\",\"mamonhoc\":\"861001\",\"tenmonhoc\":\"Nh\\u1eefng ng/l\\u00fd c\\u01a1 b\\u1ea3n c\\u1ee7a CN M\\u00e1c-L\\u00eanin\",\"sotinchi\":\"5\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"8.0\",\"diemtongkethemuoi\":\"8.0\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"16\",\"mamonhoc\":\"864001\",\"tenmonhoc\":\"X\\u00e1c su\\u1ea5t th\\u1ed1ng k\\u00ea A\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"5.6\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"17\",\"mamonhoc\":\"865006\",\"tenmonhoc\":\"Ph\\u00e1p lu\\u1eadt \\u0111\\u1ea1i c\\u01b0\\u01a1ng\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.4\",\"diemtongketdiemchu\":\"B\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"\",\"diemtrungbinhhockyhebon\":\"\",\"diemtrungbinhtichluyhemuoi\":\"\",\"diemtrungbinhtichluyhebon\":\"\",\"sotinchidat\":\"\",\"sotinchitichluy\":\"\",\"diemtrungbinhrenluyenhocky\":\"\",\"phanloaidtbrenluyenhocky\":\"\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 N\\u0103m h\\u1ecdc 2011\",\"chitiet\":[{\"sothutu\":\"18\",\"mamonhoc\":\"841020\",\"tenmonhoc\":\"C\\u01a1 s\\u1edf l\\u1eadp tr\\u00ecnh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"10.0\",\"diemtongkethemuoi\":\"9.5\",\"diemtongketdiemchu\":\"A\"},{\"sothutu\":\"19\",\"mamonhoc\":\"841024\",\"tenmonhoc\":\"M\\u1ea1ng m\\u00e1y t\\u00ednh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"20\",\"mamonhoc\":\"841041\",\"tenmonhoc\":\"Ph\\u00e1t tri\\u1ec3n \\u1ee9ng d\\u1ee5ng Web 1\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"6.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"6.0\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"21\",\"mamonhoc\":\"841043\",\"tenmonhoc\":\"C\\u01a1 s\\u1edf d\\u1eef li\\u1ec7u\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"10.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"22\",\"mamonhoc\":\"841044\",\"tenmonhoc\":\"L\\u1eadp tr\\u00ecnh h\\u01b0\\u1edbng \\u0111\\u1ed1i t\\u01b0\\u1ee3ng\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"5.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"4.5\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"23\",\"mamonhoc\":\"862008\",\"tenmonhoc\":\"GD Qu\\u1ed1c ph\\u00f2ng - An ninh (3)\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"9.0\",\"diemtongketdiemchu\":\"A\"},{\"sothutu\":\"24\",\"mamonhoc\":\"865001\",\"tenmonhoc\":\"Ti\\u1ebfng Vi\\u1ec7t th\\u1ef1c h\\u00e0nh\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.0\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"25\",\"mamonhoc\":\"866001\",\"tenmonhoc\":\"Ti\\u1ebfng Anh (1)\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"6.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"4.8\",\"diemtongketdiemchu\":\"D\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"6.37\",\"diemtrungbinhhockyhebon\":\"2.11\",\"diemtrungbinhtichluyhemuoi\":\"6.28\",\"diemtrungbinhtichluyhebon\":\"2.06\",\"sotinchidat\":\"19\",\"sotinchitichluy\":\"52\",\"diemtrungbinhrenluyenhocky\":\"70\",\"phanloaidtbrenluyenhocky\":\"Kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 2 N\\u0103m h\\u1ecdc 2011\",\"chitiet\":[{\"sothutu\":\"26\",\"mamonhoc\":\"841002\",\"tenmonhoc\":\"Gi\\u1ea3i t\\u00edch 2\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"30\",\"phantramthi\":\"70\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"6.9\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"27\",\"mamonhoc\":\"841022\",\"tenmonhoc\":\"H\\u1ec7 \\u0111i\\u1ec1u h\\u00e0nh\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.0\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"28\",\"mamonhoc\":\"841023\",\"tenmonhoc\":\"L\\u00fd thuy\\u1ebft \\u0111\\u1ed3 th\\u1ecb\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"6.0\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"29\",\"mamonhoc\":\"841025\",\"tenmonhoc\":\"Ti\\u1ebfng Anh chuy\\u00ean ng\\u00e0nh 1\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"8.0\",\"diemtongkethemuoi\":\"8.4\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"30\",\"mamonhoc\":\"841046\",\"tenmonhoc\":\"Ph\\u00e1t tri\\u1ec3n \\u1ee9ng d\\u1ee5ng web 2\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"31\",\"mamonhoc\":\"861002\",\"tenmonhoc\":\"T\\u01b0 t\\u01b0\\u1edfng H\\u1ed3 Ch\\u00ed Minh\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.4\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"32\",\"mamonhoc\":\"862009\",\"tenmonhoc\":\"GD Qu\\u1ed1c ph\\u00f2ng - An ninh (4)\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"9.0\",\"diemtongketdiemchu\":\"A\"},{\"sothutu\":\"33\",\"mamonhoc\":\"866002\",\"tenmonhoc\":\"Ti\\u1ebfng Anh (2)\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.2\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"34\",\"mamonhoc\":\"BOCH01\",\"tenmonhoc\":\"B\\u00f3ng chuy\\u1ec1n c\\u01a1 b\\u1ea3n\",\"sotinchi\":\"1\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"10.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"10.0\",\"diemtongketdiemchu\":\"A\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"6.73\",\"diemtrungbinhhockyhebon\":\"2.40\",\"diemtrungbinhtichluyhemuoi\":\"6.49\",\"diemtrungbinhtichluyhebon\":\"2.22\",\"sotinchidat\":\"25\",\"sotinchitichluy\":\"77\",\"diemtrungbinhrenluyenhocky\":\"60\",\"phanloaidtbrenluyenhocky\":\"Trung b\\u00ecnh kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 3 N\\u0103m h\\u1ecdc 2011\",\"chitiet\":[{\"sothutu\":\"35\",\"mamonhoc\":\"861003\",\"tenmonhoc\":\"\\u0110\\u01b0\\u1eddng l\\u1ed1i c\\u00e1ch m\\u1ea1ng c\\u1ee7a \\u0110CS VN\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.4\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"36\",\"mamonhoc\":\"865002\",\"tenmonhoc\":\"C\\u01a1 s\\u1edf v\\u0103n h\\u00f3a Vi\\u1ec7t Nam\",\"sotinchi\":\"2\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"37\",\"mamonhoc\":\"866003\",\"tenmonhoc\":\"Ti\\u1ebfng Anh (3)\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"5.6\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"38\",\"mamonhoc\":\"BOCH02\",\"tenmonhoc\":\"B\\u00f3ng chuy\\u1ec1n n\\u00e2ng cao 1\",\"sotinchi\":\"1\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"10.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"10.0\",\"diemtongketdiemchu\":\"A\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"\",\"diemtrungbinhhockyhebon\":\"\",\"diemtrungbinhtichluyhemuoi\":\"\",\"diemtrungbinhtichluyhebon\":\"\",\"sotinchidat\":\"\",\"sotinchitichluy\":\"\",\"diemtrungbinhrenluyenhocky\":\"\",\"phanloaidtbrenluyenhocky\":\"\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 N\\u0103m h\\u1ecdc 2012\",\"chitiet\":[{\"sothutu\":\"39\",\"mamonhoc\":\"841047\",\"tenmonhoc\":\"C\\u00f4ng ngh\\u1ec7 ph\\u1ea7n m\\u1ec1m\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"6.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"40\",\"mamonhoc\":\"841048\",\"tenmonhoc\":\"P/t\\u00edch thi\\u1ebft k\\u1ebf h\\u1ec7 th\\u1ed1ng th\\u00f4ng tin\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.5\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"41\",\"mamonhoc\":\"841050\",\"tenmonhoc\":\"Ki\\u1ec3m th\\u1eed ph\\u1ea7n m\\u1ec1m\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"5.0\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"42\",\"mamonhoc\":\"841065\",\"tenmonhoc\":\"C\\u00e1c h\\u1ec7 qu\\u1ea3n tr\\u1ecb c\\u01a1 s\\u1edf d\\u1eef li\\u1ec7u\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"43\",\"mamonhoc\":\"841106\",\"tenmonhoc\":\"Ti\\u1ebfng Anh chuy\\u00ean ng\\u00e0nh 2\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.4\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"44\",\"mamonhoc\":\"841112\",\"tenmonhoc\":\"Ph\\u00e2n t\\u00edch v\\u00e0 thi\\u1ebft k\\u1ebf gi\\u1ea3i thu\\u1eadt\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"6.60\",\"diemtrungbinhhockyhebon\":\"2.19\",\"diemtrungbinhtichluyhemuoi\":\"6.52\",\"diemtrungbinhtichluyhebon\":\"2.21\",\"sotinchidat\":\"21\",\"sotinchitichluy\":\"98\",\"diemtrungbinhrenluyenhocky\":\"65\",\"phanloaidtbrenluyenhocky\":\"Trung b\\u00ecnh kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 2 N\\u0103m h\\u1ecdc 2012\",\"chitiet\":[{\"sothutu\":\"45\",\"mamonhoc\":\"841051\",\"tenmonhoc\":\"Thi\\u1ebft k\\u1ebf giao di\\u1ec7n\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"5.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"5.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"46\",\"mamonhoc\":\"841058\",\"tenmonhoc\":\"H\\u1ec7 \\u0111i\\u1ec1u h\\u00e0nh m\\u00e3 ngu\\u1ed3n m\\u1edf\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.0\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"47\",\"mamonhoc\":\"841107\",\"tenmonhoc\":\"L\\u1eadp tr\\u00ecnh Java\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"6.5\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"48\",\"mamonhoc\":\"841110\",\"tenmonhoc\":\"C\\u01a1 s\\u1edf tr\\u00ed tu\\u1ec7 nh\\u00e2n t\\u1ea1o\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"6.0\",\"diemtongketdiemchu\":\"C\"},{\"sothutu\":\"49\",\"mamonhoc\":\"841111\",\"tenmonhoc\":\"Ph\\u00e2n t\\u00edch thi\\u1ebft k\\u1ebf h\\u01b0\\u1edbng \\u0111\\u1ed1i t\\u01b0\\u1ee3ng\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"6.0\",\"diemthilanmot\":\"4.0\",\"diemtongkethemuoi\":\"5.0\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"50\",\"mamonhoc\":\"841120\",\"tenmonhoc\":\"An to\\u00e0n v\\u00e0 b\\u1ea3o m\\u1eadt d\\u1eef li\\u1ec7u trong HTTT\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"6.0\",\"diemthilanmot\":\"5.0\",\"diemtongkethemuoi\":\"5.5\",\"diemtongketdiemchu\":\"C\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"5.76\",\"diemtrungbinhhockyhebon\":\"1.81\",\"diemtrungbinhtichluyhemuoi\":\"6.38\",\"diemtrungbinhtichluyhebon\":\"2.14\",\"sotinchidat\":\"21\",\"sotinchitichluy\":\"119\",\"diemtrungbinhrenluyenhocky\":\"70\",\"phanloaidtbrenluyenhocky\":\"Kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 3 N\\u0103m h\\u1ecdc 2012\",\"chitiet\":[{\"sothutu\":\"51\",\"mamonhoc\":\"862106\",\"tenmonhoc\":\"Gi\\u00e1o d\\u1ee5c Qu\\u1ed1c ph\\u00f2ng - An ninh (I)\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"40\",\"phantramthi\":\"60\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"3.0\",\"diemtongkethemuoi\":\"5.0\",\"diemtongketdiemchu\":\"D\"},{\"sothutu\":\"52\",\"mamonhoc\":\"BOCH03\",\"tenmonhoc\":\"B\\u00f3ng chuy\\u1ec1n n\\u00e2ng cao 2\",\"sotinchi\":\"1\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"9.0\",\"diemtongketdiemchu\":\"A\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"\",\"diemtrungbinhhockyhebon\":\"\",\"diemtrungbinhtichluyhemuoi\":\"\",\"diemtrungbinhtichluyhebon\":\"\",\"sotinchidat\":\"\",\"sotinchitichluy\":\"\",\"diemtrungbinhrenluyenhocky\":\"\",\"phanloaidtbrenluyenhocky\":\"\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 N\\u0103m h\\u1ecdc 2013\",\"chitiet\":[{\"sothutu\":\"53\",\"mamonhoc\":\"841052\",\"tenmonhoc\":\"X/d\\u1ef1ng p/m\\u1ec1m theo m\\u00f4 h\\u00ecnh ph\\u00e2n l\\u1edbp\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"8.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.0\",\"diemtongketdiemchu\":\"B\"},{\"sothutu\":\"54\",\"mamonhoc\":\"841068\",\"tenmonhoc\":\"H\\u1ec7 th\\u1ed1ng th\\u00f4ng tin doanh nghi\\u1ec7p\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"8.0\",\"diemtongkethemuoi\":\"8.5\",\"diemtongketdiemchu\":\"A\"},{\"sothutu\":\"55\",\"mamonhoc\":\"841113\",\"tenmonhoc\":\"Ph\\u00e1t tri\\u1ec3n ph\\u1ea7n m\\u1ec1m m\\u00e3 ngu\\u1ed3n m\\u1edf\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"7.0\",\"diemthilanmot\":\"7.0\",\"diemtongkethemuoi\":\"7.0\",\"diemtongketdiemchu\":\"B\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"7.50\",\"diemtrungbinhhockyhebon\":\"3.33\",\"diemtrungbinhtichluyhemuoi\":\"6.46\",\"diemtrungbinhtichluyhebon\":\"2.23\",\"sotinchidat\":\"9\",\"sotinchitichluy\":\"128\",\"diemtrungbinhrenluyenhocky\":\"78\",\"phanloaidtbrenluyenhocky\":\"Kh\\u00e1\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 2 N\\u0103m h\\u1ecdc 2013\",\"chitiet\":[{\"sothutu\":\"56\",\"mamonhoc\":\"841070\",\"tenmonhoc\":\"Th\\u1ef1c t\\u1eadp t\\u1ed1t nghi\\u1ec7p (DCT)\",\"sotinchi\":\"6\",\"phantramkiemtra\":\"100\",\"phantramthi\":\"0\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"9.0\",\"diemtongketdiemchu\":\"A\"},{\"sothutu\":\"57\",\"mamonhoc\":\"841114\",\"tenmonhoc\":\"Ph\\u00e1t tri\\u1ec3n \\u1ee9ng d\\u1ee5ng tr\\u00ean thi\\u1ebft b\\u1ecb di \\u0111\\u1ed9ng\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"9.0\",\"diemthilanmot\":\"6.0\",\"diemtongkethemuoi\":\"7.5\",\"diemtongketdiemchu\":\"B\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"8.50\",\"diemtrungbinhhockyhebon\":\"3.67\",\"diemtrungbinhtichluyhemuoi\":\"6.60\",\"diemtrungbinhtichluyhebon\":\"2.32\",\"sotinchidat\":\"9\",\"sotinchitichluy\":\"137\",\"diemtrungbinhrenluyenhocky\":\"83\",\"phanloaidtbrenluyenhocky\":\"T\\u1ed1t\"}},{\"tenhocky\":\"H\\u1ecdc k\\u1ef3 1 N\\u0103m h\\u1ecdc 2014\",\"chitiet\":[{\"sothutu\":\"58\",\"mamonhoc\":\"841071\",\"tenmonhoc\":\"D\\u1ecbch v\\u1ee5 web v\\u00e0 \\u1ee9ng d\\u1ee5ng\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"\",\"diemtongketdiemchu\":\"\"},{\"sothutu\":\"59\",\"mamonhoc\":\"841072\",\"tenmonhoc\":\"C\\u00e1c c\\u00f4ng ngh\\u1ec7 l\\u1eadp tr\\u00ecnh hi\\u1ec7n \\u0111\\u1ea1i\",\"sotinchi\":\"3\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"\",\"diemtongketdiemchu\":\"\"},{\"sothutu\":\"60\",\"mamonhoc\":\"841073\",\"tenmonhoc\":\"Seminar chuy\\u00ean \\u0111\\u1ec1\",\"sotinchi\":\"4\",\"phantramkiemtra\":\"50\",\"phantramthi\":\"50\",\"diemkiemtra\":\"\",\"diemthilanmot\":\"\",\"diemtongkethemuoi\":\"\",\"diemtongketdiemchu\":\"\"}],\"tongquat\":{\"diemtrungbinhhockyhemuoi\":\"\",\"diemtrungbinhhockyhebon\":\"\",\"diemtrungbinhtichluyhemuoi\":\"\",\"diemtrungbinhtichluyhebon\":\"\",\"sotinchidat\":\"\",\"sotinchitichluy\":\"\",\"diemtrungbinhrenluyenhocky\":\"\",\"phanloaidtbrenluyenhocky\":\"\"}}]}<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE--> <center><a href=\"http://somee.com\">Web hosting by Somee.com</a></center> </textarea></xml></script></noframes></noscript></object></layer></style></title></applet> <script language=\"JavaScript\" src=\"http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js\"></script> <!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE--> ";
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
                        String tenHocKy = data["hocky"][i]["tenhocky"];//Học kỳ 1 Năm học 2010
                        String namHoc = tenHocKy.Substring(tenHocKy.Length - 4, 4);//2010
                        String _ten1 = tenHocKy.Substring(0, 9);//Học kỳ 1 (có space)
                        String _ten2 = tenHocKy.Substring(8, 13);// Năm học 2010            
                        String maHK = tenHocKy.Substring(7, 1);//1
                        tenHocKy = _ten1 + "-" + _ten2 + "-" + (int.Parse(namHoc) + 1).ToString();//cho giống tenHocKy bên LichThi + TKB
                        String maHocKy = namHoc + maHK;//nếu muốn cập nhật theo mahocky => if tại đây
                        dynamic dataHocKy = data["hocky"][i];
                        if (mahocky != null)
                        {
                            if (maHocKy == mahocky)
                            {
                                Luu_MaHocky(maSinhVien, maHocKy, tenHocKy, dataHocKy);
                                dem++;
                            }
                        }
                        else
                        {
                            Luu_MaHocky(maSinhVien, maHocKy, tenHocKy, dataHocKy);
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
        public void Luu_MaHocky(String maSinhVien, String maHocKy, String tenHocKy, dynamic dataHocKy)
        {
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
                TTDTEntities entity = new TTDTEntities();
                entity.HocKy.Attach(hk);
                entity.Entry(hk).State = EntityState.Modified;
                entity.SaveChanges();
            }
            if (maHocKy.Substring(maHocKy.Length - 1, 1) != "3")//học kỳ 3 không lưu điểm tổng quát
            {
                Diem_TongQuat diem_tq = new Diem_TongQuat();
                diem_tq.MaSinhVien = maSinhVien;
                diem_tq.MaHocKy = maHocKy;
                diem_tq.DiemTrungBinhHocKyHeMuoi = dataHocKy["tongquat"]["diemtrungbinhhockyhemuoi"];
                diem_tq.DiemTrungBinhHocKyHeBon = dataHocKy["tongquat"]["diemtrungbinhhockyhebon"];
                diem_tq.DiemTrungBinhTichLuyHeMuoi = dataHocKy["tongquat"]["diemtrungbinhtichluyhemuoi"];
                diem_tq.DiemTrungBinhTichLuyHeBon = dataHocKy["tongquat"]["diemtrungbinhtichluyhebon"];
                diem_tq.SoTinChiDat = dataHocKy["tongquat"]["sotinchidat"];
                diem_tq.SoTinChiTichLuy = dataHocKy["tongquat"]["sotinchitichluy"];
                diem_tq.DiemTrungBinhRenLuyenHocKy = dataHocKy["tongquat"]["diemtrungbinhrenluyenhocky"];
                diem_tq.PhanLoaiDTBRenLuyenHocKy = dataHocKy["tongquat"]["phanloaidtbrenluyenhocky"];
                //lưu điểm tổng quát
                try//lưu mới
                {
                    TTDTEntities entity1 = new TTDTEntities();
                    entity1.Diem_TongQuat.Add(diem_tq);
                    entity1.SaveChanges();
                }
                catch//update
                {
                    TTDTEntities entity1 = new TTDTEntities();
                    entity1.Diem_TongQuat.Attach(diem_tq);
                    entity1.Entry(diem_tq).State = EntityState.Modified;
                    entity1.SaveChanges();
                }
            }

            int monhoc = ((ICollection)dataHocKy["chitiet"]).Count;//số môn học trong học kỳ
            for (int i = 0; i < monhoc; i++)
            {
                dynamic dataMonHoc = dataHocKy["chitiet"][i];
                MonHoc mh = new MonHoc();
                mh.MaMonHoc = dataMonHoc["mamonhoc"];
                mh.TenMonHoc = dataMonHoc["tenmonhoc"];
                try//lưu MonHoc => k đc => đã có
                {
                    TTDTEntities entity = new TTDTEntities();
                    entity.MonHoc.Add(mh);
                    entity.SaveChanges();
                }
                catch//update
                {
                    TTDTEntities entity = new TTDTEntities();
                    entity.MonHoc.Attach(mh);
                    entity.Entry(mh).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Diem_ChiTiet diem_ct = new Diem_ChiTiet();
                diem_ct.MaSinhVien = maSinhVien;
                diem_ct.MaHocKy = maHocKy;
                diem_ct.SoThuTu = int.Parse(dataMonHoc["sothutu"]);
                diem_ct.MaMonHoc = dataMonHoc["mamonhoc"];
                diem_ct.SoTinChi = int.Parse(dataMonHoc["sotinchi"]);
                diem_ct.PhanTramKiemTra = int.Parse(dataMonHoc["phantramkiemtra"]);
                diem_ct.PhanTramThi = int.Parse(dataMonHoc["phantramthi"]);
                diem_ct.DiemKiemTra = dataMonHoc["diemkiemtra"];
                diem_ct.DiemThiLanMot = dataMonHoc["diemthilanmot"];
                diem_ct.DiemTongKetHeMuoi = dataMonHoc["diemtongkethemuoi"];
                diem_ct.DiemTongKetDiemChu = dataMonHoc["diemtongketdiemchu"];
                //Lưu điểm chi tiết
                try//lưu mới
                {
                    TTDTEntities entity2 = new TTDTEntities();
                    entity2.Diem_ChiTiet.Add(diem_ct);
                    entity2.SaveChanges();
                }
                catch//update
                {
                    TTDTEntities entity2 = new TTDTEntities();
                    entity2.Diem_ChiTiet.Attach(diem_ct);
                    entity2.Entry(diem_ct).State = EntityState.Modified;
                    entity2.SaveChanges();
                }
            }
        }
    }
}
