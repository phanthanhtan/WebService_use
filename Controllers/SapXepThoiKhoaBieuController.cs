using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService_use.Models;

namespace WebService_use.Models
{
    public class SapXepThoiKhoaBieuController : Controller
    {
        //
        // GET: /SapThoiKhoaBieu/

        public ActionResult Index()
        {
            try
            {
                TTDTEntities entity = new TTDTEntities();
                ViewBag.SXTKB_MonHoc = entity.SXTKB_MonHoc.ToList();
                int count = ((List<SXTKB_MonHoc>)ViewBag.SXTKB_MonHoc).Count;
                if (count != 0)
                    ViewBag.Ok = "Index1";//có dữ liệu
                else
                    ViewBag.Ok = "Index2";//không có dữ liệu       
            }
            catch
            {
                ViewBag.Ok = "Index3";//lỗi
            }
            return View();
        }
        [HttpPost]
        public ActionResult CheckBox(FormCollection fc)
        {
            try
            {
                String strNgay = "", strMonHoc = "";
                if (fc["MonHoc"] != null)
                {
                    strMonHoc = fc["MonHoc"].ToString();
                    if (fc["Ngay"] != null)
                        strNgay = fc["Ngay"].ToString();
                    //1. tạo mảng 2 chiều 2-8 1-13
                    //2. đánh dấu các ngày bận trong mảng 2 chiều
                    //3. tạo mảng chứa các môn (nhóm) cần sắp TKB => kiểm tra môn ở mảng 2 chiều
                    //List<List<String>> MonNhom = new List<List<String>>();
                    //4. mảng tmp chứa các trường hợp
                    //5. kiểm tra mảng tmp với mảng 2 chiều => hợp lý => mảng chứa các TKB thỏa điều kiện

                    //1. //2.   public List<List<String>> List2Chieu(String strNgay)
                    //3.
                    List<List<String>> MonNhom = new List<List<String>>();
                    String[] _strMonHoc = strMonHoc.Split(',');
                    foreach (String strMonHoc1 in _strMonHoc)//MaMonHoc
                    {
                        TTDTEntities entity = new TTDTEntities();
                        ViewBag.ListNhom = entity.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc.Equals(strMonHoc1)).Select(item => new SXTKB_NhomMonHoc_Nhom { MaMonHoc = item.MaMonHoc, Nhom = item.Nhom}).Distinct().ToList();
                        List<String> ListNhomMonHoc_Nhom = new List<String>();
                        foreach (SXTKB_NhomMonHoc_Nhom nhom in (List<SXTKB_NhomMonHoc_Nhom>)ViewBag.ListNhom)
                        {
                            TTDTEntities entity1 = new TTDTEntities();
                            ViewBag.ListThu = entity1.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc.Equals(strMonHoc1) && item.Nhom.Equals(nhom.Nhom)).Distinct().ToList();
                            int dem = 0;
                            foreach (SXTKB_NhomMonHoc mh in (List<SXTKB_NhomMonHoc>)ViewBag.ListThu)
                            {
                                String[] _strNgay = strNgay.Split(',');//ngày bận
                                foreach (String strNgay1 in _strNgay)//kiểm tra thứ của nhóm có trùng ngày bận không?!?
                                {
                                    if (mh.Thu.ToString() == strNgay1)
                                    {
                                        dem++;
                                        break;
                                    }
                                }
                                if (dem != 0)
                                    break;
                            }
                            if (dem == 0)//nhóm ok, không trùng ngày bận => add
                            {
                                ListNhomMonHoc_Nhom.Add(strMonHoc1 + "-" + nhom.Nhom);//MaMonHoc-Nhom
                            }
                        }
                        if (ListNhomMonHoc_Nhom.Count > 0)//ListMonHoc_Nhom có nhóm hợp lệ, không trùng ngày bận
                        {//cho dù count = 0, thì cũng != null => nên if theo count
                            MonNhom.Add(ListNhomMonHoc_Nhom);
                        }
                        else//môn chọn trùng ngày bận => không xét các môn khác nữa
                        {
                            break;
                        }
                    }
                    if (MonNhom.Count() == _strMonHoc.Count())//MonNhom != null  và có đủ môn học đã chọn => Sắp TKB
                    {//cho dù count = 0, thì cũng != null => nên if theo count
                        //4.
                        List<String> tmp = new List<String>();//tmp chứa các trường hợp
                        for (int i = 0; i < MonNhom.Count; i++)
                        {
                            tmp = Nhan2Mang(tmp, MonNhom[i]);
                        }
                        //5.
                        List<String> ListTKB = new List<String>();
                        foreach (String strTruongHop in tmp)//841071-01,841072-02,...
                        {
                            int check = 0;
                            //Sử dụng Mang2Chieu nhiều lần để tìm ra strTruongHop thỏa điều kiện
                            List<List<String>> Mang2Chieu = List2Chieu(strNgay);
                            String[] MonHoc_Nhom = strTruongHop.Split(',');//841071-01
                            foreach (String str in MonHoc_Nhom)
                            {
                                String[] Nhom = str.Split('-');
                                if (Nhom.Count() == 2)
                                {
                                    String _MaMonHoc = Nhom[0];
                                    String _Nhom = Nhom[1];
                                    TTDTEntities entity = new TTDTEntities();
                                    List<SXTKB_NhomMonHoc> Thu_Tiet = entity.SXTKB_NhomMonHoc.Where(item => item.MaMonHoc.Equals(_MaMonHoc) && item.Nhom.Equals(_Nhom)).ToList();
                                    foreach (SXTKB_NhomMonHoc nmh in Thu_Tiet)
                                    {
                                        int thu = int.Parse(nmh.Thu.ToString());
                                        int tietbatdau = int.Parse(nmh.TietBatDau.ToString());
                                        int sotiet = int.Parse(nmh.SoTiet.ToString());
                                        for (int i = 0; i < sotiet; i++)
                                        {
                                            if (Mang2Chieu[thu - 2][tietbatdau - 1] == "0")//tiết trống
                                            {
                                                Mang2Chieu[thu - 2][tietbatdau - 1] = "1";
                                                tietbatdau++;
                                            }
                                            else//tiết đã có TKB => dừng => wa trường hợp TKB khác
                                            {
                                                check++;
                                                break;
                                            }
                                        }
                                        if (check != 0)//=> wa trường hợp TKB khác
                                            break;
                                    }
                                }
                                else//có môn sai cấu trúc, MaMonHoc = xyz-abc hoặc Nhom = xyz-abc
                                {
                                    check++;
                                    break;
                                }
                                if (check != 0)//=> wa trường hợp TKB khác
                                    break;
                            }
                            if (check == 0)
                                ListTKB.Add(strTruongHop);//add trường hợp TKB thỏa điều kiện
                        }
                        if (ListTKB != null)
                        {
                            ViewBag.ListTKB = ListTKB;
                            ViewBag.Ok = "CheckBox1";//ok có TKB
                        }
                        else
                            ViewBag.Ok = "CheckBox2";//Không sắp đc TKB
                    }
                    else//MonNhom == null hoặc không đủ số môn chọn => không Sắp TKB
                    {
                        ViewBag.Ok = "CheckBox3";//có môn học trùng ngày bận => không sắp đc TKB
                    }
                }
                else
                    ViewBag.Ok = "CheckBox4";//chưa chọn môn học
            }
            catch
            {
                ViewBag.Ok = "CheckBox5";//lỗi
            }
            return View("Index");
        }
        public List<string> Nhan2Mang(List<String> a, List<String> b)
        {
            if (a.Count == 0) return b;
            if (b.Count == 0) return a;
            List<String> tmp = new List<String>();
            foreach (var item in a)
            {
                foreach (var item2 in b)
                {
                    tmp.Add(item + "," + item2);
                }
            }
            return tmp;
        }
        public List<List<String>> List2Chieu(String strNgay)
        {
            List<List<String>> Mang2Chieu = new List<List<String>>();//0-6 0-12
            for (int i = 0; i < 7; i++)//gán tất cả giá trị mảng 2 chiều là 0
            {
                List<String> item = new List<String>();
                for (int j = 0; j < 13; j++)
                    item.Add("0");
                Mang2Chieu.Add(item);
            }
            String[] _strNgay = strNgay.Split(',');
            foreach (String strNgay1 in _strNgay)
            {
                for (int i = 0; i < 13; i++)
                    Mang2Chieu[int.Parse(strNgay1) - 2][i] = "1";
                //_Mang2Chieu[int.Parse(strNgay1) - 2][i] = "1";
            }
            return Mang2Chieu;
        }
        public void SapTKB()//khong co xai thang nay
        {
            //List<List<string>> mang = new List<List<string>>();
            //mang[0][0];
            /*
            $t[](
			$t[0]=1-1 1-2
			$t[1]=4-1			
			$t[2]=2-1 2-2 2-3
			$t[3]=3-1 3-2
		    ) 
             */
            List<List<String>> mang = new List<List<String>>();
            List<String> item = new List<String>();
            item.Add("1-01");
            item.Add("1-02");
            mang.Add(item);
            item = new List<String>();
            item.Add("4-01");
            mang.Add(item);
            item = new List<String>();
            item.Add("2-01");
            item.Add("2-02");
            item.Add("2-03");
            mang.Add(item);
            item = new List<String>();
            item.Add("3-01");
            item.Add("3-02");
            mang.Add(item);
            List<String> tmp = new List<String>();
            for (int i = 0; i < mang.Count; i++)
            {
                tmp = Nhan2Mang(tmp, mang[i]);
            }
            ViewBag.tmp = tmp;            
        }
    }
}
