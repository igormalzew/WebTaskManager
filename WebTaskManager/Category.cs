//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTaskManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public int CategoryId { get; set; }
        public int CategoryTypeId { get; set; }
        public int TaskId { get; set; }
    
        public virtual Task Task { get; set; }
        public virtual CategoryType CategoryType { get; set; }
        public virtual Task Task1 { get; set; }
    }
}
