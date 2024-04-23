namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int ID { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        public DateTime PostedDate { get; set; }

        public int ParentID { get; set; }

        public int? UserID { get; set; }

        public virtual User User { get; set; }
    }
}
