//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NHST.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_FeeOrder
    {
        public int ID { get; set; }
        public Nullable<int> UserLevelID { get; set; }
        public string LevelName { get; set; }
        public Nullable<double> PercentService { get; set; }
        public Nullable<int> KhoHNID { get; set; }
        public Nullable<double> FeeWeightHN { get; set; }
        public Nullable<int> KhoSGID { get; set; }
        public Nullable<double> FeeWeightSG { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<double> PercentDeposit { get; set; }
    }
}