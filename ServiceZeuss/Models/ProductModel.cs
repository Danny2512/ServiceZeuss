using System.ComponentModel.DataAnnotations;

namespace ServiceZeuss.Models
{
    public class ProductModel
    {
        [Required(ErrorMessage = "El campo CategoryId es obligatorio.")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "El campo Name es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El campo Name no puede tener más de 30 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Price es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo Price debe ser mayor que 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo ImageUrl es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El campo ImageUrl no puede tener más de 250 caracteres.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "El campo Active es obligatorio.")]
        public bool Active { get; set; }
    }
}
