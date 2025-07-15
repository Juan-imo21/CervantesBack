using System.Data.SqlClient;
public static class Conexion
{
    public static string Cadena = "Server=JuanImoberdorf\\SQLEXPRESS;Database=PPADMBEBIDAS;Trusted_Connection=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(Cadena);
    }
}