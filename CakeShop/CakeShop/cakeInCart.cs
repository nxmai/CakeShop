//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CakeShop
{
    using System;
    using System.Collections.Generic;
    
    public partial class cakeInCart
    {
        public int cicId { get; set; }
        public Nullable<int> cakeId { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<int> cartId { get; set; }
    
        public virtual cake cake { get; set; }
        public virtual cart cart { get; set; }
    }
}
