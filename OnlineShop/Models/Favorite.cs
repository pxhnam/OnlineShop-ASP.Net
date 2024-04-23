namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favorite
    {
        public int ID { get; set; }

        public int IDProduct { get; set; }

        public int UserID { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
