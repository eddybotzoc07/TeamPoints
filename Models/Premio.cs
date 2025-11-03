using System.ComponentModel.DataAnnotations;

namespace TeamPoints.Models
{
    public class Premio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
        public int CostoPuntos { get; set; }
    }
}
