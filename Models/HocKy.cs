//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebService_use.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HocKy
    {
        public HocKy()
        {
            this.Diem_ChiTiet = new HashSet<Diem_ChiTiet>();
            this.Diem_TongQuat = new HashSet<Diem_TongQuat>();
            this.LichThi = new HashSet<LichThi>();
            this.ThoiKhoaBieu = new HashSet<ThoiKhoaBieu>();
        }
    
        public string MaHocKy { get; set; }
        public string TenHocKy { get; set; }
    
        public virtual ICollection<Diem_ChiTiet> Diem_ChiTiet { get; set; }
        public virtual ICollection<Diem_TongQuat> Diem_TongQuat { get; set; }
        public virtual ICollection<LichThi> LichThi { get; set; }
        public virtual ICollection<ThoiKhoaBieu> ThoiKhoaBieu { get; set; }
    }
}