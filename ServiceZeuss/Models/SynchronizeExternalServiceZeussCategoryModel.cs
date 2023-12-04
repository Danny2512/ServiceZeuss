using System.ComponentModel.DataAnnotations;

namespace ServiceZeuss.Models
{
    public class SynchronizeExternalServiceZeussCategoryModel
    {
        [Required(ErrorMessage = "El campo Name es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El campo Name no puede tener más de 30 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Activo es obligatorio.")]
        public bool Active { get; set; }
        public List<ProductModel>? Products { get; set; }
    }
}
