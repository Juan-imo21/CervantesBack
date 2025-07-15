namespace CervantesBack.Helpers
{
    public class recuperacionHelper
    {
        public static class CodigoRecuperacionStorage
        {
            private static Dictionary<string, (string Codigo, DateTime Expira)> _codigos = new();

            public static void GuardarCodigo(string email, string codigo)
            {
                _codigos[email] = (codigo, DateTime.UtcNow.AddMinutes(10));
            }

            public static bool ValidarCodigo(string email, string codigo)
            {
                if (_codigos.TryGetValue(email, out var info))
                {
                    if (info.Codigo == codigo && DateTime.UtcNow <= info.Expira)
                    {
                        _codigos.Remove(email); // Usar una sola vez
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
