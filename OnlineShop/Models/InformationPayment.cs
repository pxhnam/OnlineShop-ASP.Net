using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class InformationPayment
    {
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string Notes { get; set; }

        [Required]
        public int Payment { get; set; }
        public IEnumerable<Payment> Payments { get; set; }

        [Required]
        public bool terms { get; set; }
    }
}