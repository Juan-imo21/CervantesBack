namespace CervantesBack.Models
{
    public class clsLogin
    {
        public class LoginRequest
        {
            public string AliasLogin { get; set; }
            public string Contrasena { get; set; }
        }

        public class LoginResponse
        {
            public string Nombre { get; set; }
            public string Rol { get; set; }
            public string Email { get; set; }
        }
    }
}
