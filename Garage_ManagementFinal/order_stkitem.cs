//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Garage_ManagementFinal
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_stkitem
    {
        public int Id { get; set; }
        public double quantity { get; set; }
        public bool status { get; set; }
        public int Entry_FormId { get; set; }
        public int StockId { get; set; }
        public string Alpha { get; set; }
        public Nullable<System.DateTime> date { get; set; }

        public string Orderedby { get; set; }
    
        public virtual Entry_Form Entry_Form { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
