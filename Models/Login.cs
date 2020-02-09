using System.ComponentModel.DataAnnotations;

namespace bank.Models
{
    public class Login
    {
        [Required, StringLength(8)]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
        [Display(Name = "LockNum")]
        [Range(0, 3, ErrorMessage = "Wrong number")]
        public int LockNum { get; set; }
        [Display(Name = "Status")]
        [Range(0, 1, ErrorMessage = "Wrong number")]
        public int Status { get; set; }
    }
}
