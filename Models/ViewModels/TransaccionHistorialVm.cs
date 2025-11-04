using TeamPoints.Models;

namespace TeamPoints.Models.ViewModels
{
    public class TransaccionHistorialVm
    {
        public DateTime Fecha { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public int Puntos { get; set; }
        public string? Referencia { get; set; }
    }
}
