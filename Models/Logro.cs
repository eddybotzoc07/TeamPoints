using System.ComponentModel.DataAnnotations;

namespace TeamPoints.Models
{
    public class Logro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Los puntos deben ser mayores a 0")]
        public int Puntos { get; set; }
    }
}
