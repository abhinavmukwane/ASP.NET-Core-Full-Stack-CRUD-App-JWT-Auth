using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Fullstack_API.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        public required string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Qty { get; set; }

    }
}
