using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebService_use.Models
{
    public class Select_Index
    {
        public string MaSinhVien { get; set; }
        public string TenSinhVien { get; set; }
    }
    public class LichThi_Xem
    {
        public string MaSinhVien { get; set; }
        public string MaHocKy { get; set; }
        public string TenHocKy { get; set; }
        public Nullable<int> SoMonThi { get; set; }
    }
    public class ThoiKhoaBieu_Xem
    {
        public string MaSinhVien { get; set; }
        public string MaHocKy { get; set; }
        public string TenHocKy { get; set; }
    }
    public class ThoiKhoaBieu_Xem_MaHocKy
    {
        public string MaSinhVien { get; set; }
        public string MaHocKy { get; set; }
        public string TenHocKy { get; set; }
        public string MaTuan { get; set; }
        public string TenTuan { get; set; }
    }
    public class Diem_ChiTiet_Xem
    {
        public string MaSinhVien { get; set; }
        public string MaHocKy { get; set; }
        public string TenHocKy { get; set; }
    }
    public class SXTKB_NhomMonHoc_Nhom
    {
        public string MaMonHoc { get; set; }
        public string Nhom { get; set; }
    }
}
