namespace CervantesBack.Models
{
    public class clsRecuperacion
    {
        public class RecuperacionRequest
        {
            public string Email { get; set; }
        }

        public class CodigoVerificacionRequest
        {
            public string Email { get; set; }
            public string Codigo { get; set; }
        }

        public class CambioPasswordRequest
        {
            public string Email { get; set; }
            public string Codigo { get; set; }
            public string NuevaContrasena { get; set; }
        }
    }
}
