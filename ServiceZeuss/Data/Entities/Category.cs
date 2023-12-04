using System.ComponentModel.DataAnnotations;

namespace ServiceZeuss.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo StrName es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El campo StrName no puede tener más de 30 caracteres.")]
        public string StrName { get; set; }

        [Required(ErrorMessage = "El campo BiActive es obligatorio.")]
        public bool BiActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
