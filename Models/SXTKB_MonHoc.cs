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
    
    public partial class SXTKB_MonHoc
    {
        public SXTKB_MonHoc()
        {
            this.SXTKB_NhomMonHoc = new HashSet<SXTKB_NhomMonHoc>();
        }
    
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
    
        public virtual ICollection<SXTKB_NhomMonHoc> SXTKB_NhomMonHoc { get; set; }
    }
}
