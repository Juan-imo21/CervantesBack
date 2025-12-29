using System.Collections.Generic;
using System.Data.SqlClient;

public class UsuarioRepository
{
    private string connectionString = Conexion.Cadena;

    public List<Usuario> Listar()
    {
        var lista = new List<Usuario>();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM USUARIOS", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    ID_USER = (int)reader["ID_USER"],
                    Nombre = reader["Nombre"].ToString(),
                    Rol = reader["Rol"].ToString(),
                    AliasLogin = reader["AliasLogin"].ToString(),
                    Contrasena = reader["Contraseña"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }
        }

        return lista;
    }

    public void Agregar(Usuario usuario)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO USUARIOS (Nombre, Rol, AliasLogin, Contraseña, Email) VALUES (@Nombre, @Rol, @AliasLogin, @Contrasena, @Email)", conn);

            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@AliasLogin", usuario.AliasLogin);
            string hashFinal = SeguridadHelper.ProcesarContrasena(usuario.Contrasena);
            cmd.Parameters.AddWithValue("@Contrasena", hashFinal);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);

            cmd.ExecuteNonQuery();
        }
    }

    public void Editar(Usuario usuario)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE USUARIOS SET Nombre=@Nombre, Rol=@Rol, AliasLogin=@AliasLogin, Contraseña=@Contrasena, Email=@Email WHERE ID_USER=@ID", conn);

            cmd.Parameters.AddWithValue("@ID", usuario.ID_USER);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@AliasLogin", usuario.AliasLogin);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);

            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM USUARIOS WHERE ID_USER = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.ExecuteNonQuery();
        }
    }
}
public class Usuario
{
    public int ID_USER { get; set; }
    public string Nombre { get; set; }
    public string Rol { get; set; }
    public string AliasLogin { get; set; }
    public string Contrasena { get; set; }
    public string Email { get; set; }
}