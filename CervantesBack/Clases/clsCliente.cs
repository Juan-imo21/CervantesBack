using System.Collections.Generic;
using System.Data.SqlClient;

public class ClienteRepository
{
    private string connectionString = Conexion.Cadena;

    public List<Cliente> Listar()
    {
        var lista = new List<Cliente>();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM CLIENTE", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Cliente
                {
                    ID_CLIENTES = (int)reader["ID_CLIENTES"],
                    RazonSocial = reader["RazonSocial"]?.ToString(),
                    CUIT = reader["CUIT"]?.ToString(),
                    Direccion = reader["Direccion"]?.ToString(),
                    Telefono = reader["Telefono"]?.ToString(),
                    Email = reader["Email"]?.ToString(),
                    Nombre = reader["Nombre"]?.ToString()
                });
            }
        }

        return lista;
    }

    public void Agregar(Cliente cliente)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"INSERT INTO CLIENTE 
                (RazonSocial, CUIT, Direccion, Telefono, Email, Nombre)
                VALUES (@RazonSocial, @CUIT, @Direccion, @Telefono, @Email, @Nombre)", conn);

            cmd.Parameters.AddWithValue("@RazonSocial", cliente.RazonSocial ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CUIT", cliente.CUIT);
            cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }

    public void Editar(Cliente cliente)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"UPDATE CLIENTE SET 
                RazonSocial = @RazonSocial,
                CUIT = @CUIT,
                Direccion = @Direccion,
                Telefono = @Telefono,
                Email = @Email,
                Nombre = @Nombre
                WHERE ID_CLIENTES = @ID", conn);

            cmd.Parameters.AddWithValue("@ID", cliente.ID_CLIENTES);
            cmd.Parameters.AddWithValue("@RazonSocial", cliente.RazonSocial ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CUIT", cliente.CUIT);
            cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM CLIENTE WHERE ID_CLIENTES = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.ExecuteNonQuery();
        }
    }
    
}
public class Cliente
    {
        public int ID_CLIENTES { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
    }