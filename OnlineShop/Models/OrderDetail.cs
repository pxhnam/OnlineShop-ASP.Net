namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        public int ID { get; set; }

        public int IDProduct { get; set; }

        public int IDBill { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual InforProduct InforProduct { get; set; }

        public virtual Order Order { get; set; }
    }
}
