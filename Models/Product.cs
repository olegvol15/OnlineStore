using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва товару є обов'язковою.")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ціна є обов'язковою.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна повинна бути більше 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Опис є обов'язковим.")]
        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів.")]
        public string Description { get; set; }
    }
}
