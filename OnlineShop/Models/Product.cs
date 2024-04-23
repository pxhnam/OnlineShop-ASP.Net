namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Favorites = new HashSet<Favorite>();
            InforProducts = new HashSet<InforProduct>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public decimal SalePrice { get; set; }

        [Required]
        [StringLength(255)]
        public string Picture { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public int CategoryID { get; set; }

        public bool Status { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favorite> Favorites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InforProduct> InforProducts { get; set; }
    }
}
