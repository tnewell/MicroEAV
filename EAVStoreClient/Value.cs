//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EAVStoreClient
{
    using System;
    using System.Collections.Generic;
    
    public partial class Value
    {
        public int Attribute_ID { get; set; }
        public int Instance_ID { get; set; }
        public string Raw_Value { get; set; }
        public Nullable<int> Unit_ID { get; set; }
    
        public virtual Attribute Attribute { get; set; }
        public virtual Instance Instance { get; set; }
        public virtual Unit Unit { get; set; }
    }
}