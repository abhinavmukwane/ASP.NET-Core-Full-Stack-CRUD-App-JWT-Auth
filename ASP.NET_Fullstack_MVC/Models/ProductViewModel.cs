using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Fullstack_MVC.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Qty { get; set; }

    }
}
