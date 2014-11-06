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
    public class LichThiController : Controller
    {
        //
        // GET: /LichThi/

        public ActionResult Index(String mssv)//mssv la alert
        {
            try
            {
                if (mssv != null)
                    ViewBag.Alert = mssv;
                else
                    ViewBag.Alert = "";
                TTDTEntities entity = new TTDTEntities();
                ViewBag.LichThi = entity.LichThi.Select(item => new Select_Index { MaSinhVien = item.MaSinhVien, TenSinhVien = item.ThongTin.TenSinhVien }).Distinct().ToList();
                int count = ((List<Select_Index>)ViewBag.LichThi).Count;
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
                ViewBag.LichThi = entity.LichThi.Where(item => item.MaSinhVien.Equals(mssv)).ToList();
                int count = ((List<LichThi>)ViewBag.LichThi).Count;
                if (count != 0)//có dữ liệu
                {
                    int dem = 0;
                    foreach (LichThi lt in (List<LichThi>)ViewBag.LichThi)
                    {
                        if (mahocky == null)//xóa tất cả học kỳ của 1 sinh viên
                        {
                            Xoa_MaHocKy(lt);
                            dem++;
                        }
                        else//xóa theo mã học kỳ của 1 svinh viên
                            if (lt.MaHocKy == mahocky)
                            {
                                Xoa_MaHocKy(lt);
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
        public void Xoa_MaHocKy(LichThi lt)
        {
            TTDTEntities entity = new TTDTEntities();
            LichThi _lt = entity.LichThi.Single(item => item.MaSinhVien.Equals(lt.MaSinhVien) && item.MaHocKy.Equals(lt.MaHocKy) && item.SoThuTu.Equals(lt.SoThuTu));
            entity.LichThi.Remove(_lt);
            entity.SaveChanges();
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
                TTDTEntities entity = new TTDTEntities();
                int count;
                if (mahocky == null)//tất cả học kỳ
                {
                    ViewBag.LichThi = entity.LichThi.Where(item => item.MaSinhVien.Equals(mssv)).Select(item => new LichThi_Xem { MaSinhVien = item.MaSinhVien, MaHocKy = item.MaHocKy, TenHocKy = item.HocKy.TenHocKy, SoMonThi = item.SoMonThi }).Distinct().OrderByDescending(item=> item.MaHocKy).ToList();
                    count = ((List<LichThi_Xem>)ViewBag.LichThi).Count;
                    ViewBag.Check = 1;//theo mã sv
                }
                else//mahocky
                {
                    ViewBag.LichThi = entity.LichThi.Where(item => item.MaSinhVien.Equals(mssv) && item.MaHocKy.Equals(mahocky)).ToList();
                    count = ((List<LichThi>)ViewBag.LichThi).Count;
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
        public ActionResult CapNhat(String mssv, String mahocky) //mahocky = null
        {
            try
            {
                thongtindaotao.Service ttdt = new thongtindaotao.Service();
                String str = ttdt.XemLichThi(mssv).ToString();
                //String str = "{\"taikhoan\":{\"masinhvien\":\"3110410126\",\"tensinhvien\":\"Phan Thanh T\\u00e2n\"},\"hocky\":[{\"mahocky\":\"20133\",\"tenhocky\":\"H\\u1ecdc k\\u1ef3 3 - N\\u0103m h\\u1ecdc 2013-2014\",\"monthi\":null},{\"mahocky\":\"20132\",\"tenhocky\":\"H\\u1ecdc k\\u1ef3 2 - N\\u0103m h\\u1ecdc 2013-2014\",\"monthi\":[{\"sothutu\":\"1\",\"mamonhoc\":\"841114\",\"tenmonhoc\":\"Ph\\u00e1t tri\\u1ec3n \\u1ee9ng d\\u1ee5ng tr\\u00ean thi\\u1ebft b\\u1ecb di \\u0111\\u1ed9ng\",\"ghepthi\":\"01\",\"tothi\":\"002\",\"soluong\":\"34\",\"ngaythi\":\"29/05/2014\",\"giobatdau\":\"12g00\",\"sophut\":\"0\",\"phong\":\"C.A502\",\"hinhthucthi\":\"\"}]}]}<!--SCRIPTGENERATEDBYSERVER!PLEASEREMOVE--><center><ahref=\"http://somee.com\">WebhostingbySomee.com</a></center></textarea></xml></script></noframes></noscript></object></layer></style></title></applet><scriptlanguage=\"JavaScript\"src=\"http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js\"></script><!--SCRIPTGENERATEDBYSERVER!PLEASEREMOVE-->";
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
            LichThi lt = new LichThi();
            lt.MaSinhVien = maSinhVien;
            lt.MaHocKy = maHocKy;
            String tenHocKy = dataHocKy["tenhocky"];
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
            if (dataHocKy["monthi"] != null)
            {
                int monthi = ((ICollection)dataHocKy["monthi"]).Count;
                lt.SoMonThi = monthi;
                for (int j = 0; j < monthi; j++)
                {
                    lt.SoThuTu = int.Parse(dataHocKy["monthi"][j]["sothutu"]);
                    String maMonHoc = dataHocKy["monthi"][j]["mamonhoc"];
                    lt.MaMonHoc = maMonHoc;
                    String tenMonHoc = dataHocKy["monthi"][j]["tenmonhoc"];
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
                        TTDTEntities entity = new TTDTEntities();
                        entity.MonHoc.Attach(mh);
                        entity.Entry(mh).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    lt.GhepThi = dataHocKy["monthi"][j]["ghepthi"];
                    lt.ToThi = dataHocKy["monthi"][j]["tothi"];
                    lt.SoLuong = int.Parse(dataHocKy["monthi"][j]["soluong"]);
                    lt.NgayThi = dataHocKy["monthi"][j]["ngaythi"];
                    lt.GioBatDau = dataHocKy["monthi"][j]["giobatdau"];
                    lt.SoPhut = int.Parse(dataHocKy["monthi"][j]["sophut"]);
                    lt.Phong = dataHocKy["monthi"][j]["phong"];
                    lt.HinhThucThi = dataHocKy["monthi"][j]["hinhthucthi"];
                    LuuLichThi(lt); //lưu LichThi
                }
            }
            else//null => không có lịch thi
            {
                lt.SoMonThi = 0;
                lt.SoThuTu = 1;//để sau này có lịch thi thì update
                LuuLichThi(lt);//lưu LichThi
            }
        }
        public void LuuLichThi(LichThi lt)
        {
            TTDTEntities entity = new TTDTEntities();
            try //không kiểm tra CSDL => chưa có mã sv => try => insert
            {
                entity.LichThi.Add(lt);
                entity.SaveChanges();
            }
            catch//có mã sv rồi => catch => update
            {
                entity.LichThi.Attach(lt);
                entity.Entry(lt).State = EntityState.Modified;
                entity.SaveChanges();
            }
        }
    }
}
