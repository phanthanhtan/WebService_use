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
    
    public partial class HeDaoTao
    {
        public HeDaoTao()
        {
            this.ThongTin = new HashSet<ThongTin>();
        }
    
        public string MaHeDaoTao { get; set; }
        public string TenHeDaoTao { get; set; }
    
        public virtual ICollection<ThongTin> ThongTin { get; set; }
    }
}
