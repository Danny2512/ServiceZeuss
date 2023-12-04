using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceZeuss.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo CategoryFK es obligatorio.")]
        [ForeignKey("Category")]
        public Guid CategoryFK { get; set; }

        [Required(ErrorMessage = "El campo StrName es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El campo StrName no puede tener más de 30 caracteres.")]
        public string StrName { get; set; }

        [Required(ErrorMessage = "El campo DePrice es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo DePrice debe ser mayor que 0.")]
        public decimal DePrice { get; set; }

        [Required(ErrorMessage = "El campo StrImageUrl es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El campo StrImageUrl no puede tener más de 250 caracteres.")]
        public string StrImageUrl { get; set; }

        [Required(ErrorMessage = "El campo BiActive es obligatorio.")]
        public bool BiActive { get; set; }

        public Category Category { get; set; }
    }
}
