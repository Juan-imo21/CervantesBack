using System.Collections.Generic;
using System.Data.SqlClient;

public class ProductoRepository
{
    private string connectionString = Conexion.Cadena;

    public List<Producto> Listar()
    {
        var lista = new List<Producto>();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM PRODUCTO", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Producto
                {
                    ID_PRODUCTO = (int)reader["ID_PRODUCTO"],
                    Nombre = reader["Nombre"].ToString(),
                    Tipo = reader["Tipo"].ToString(),
                    Precio = (decimal)reader["Precio"],
                    StockActual = (int)reader["StockActual"],
                    StockMinimo = (int)reader["StockMinimo"]
                });
            }
        }

        return lista;
    }

    public void Agregar(Producto producto)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"INSERT INTO PRODUCTO 
                (Nombre, Tipo, Precio, StockActual, StockMinimo) 
                VALUES (@Nombre, @Tipo, @Precio, @StockActual, @StockMinimo)", conn);

            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@Tipo", producto.Tipo);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@StockActual", producto.StockActual);
            cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

            cmd.ExecuteNonQuery();
        }
    }

    public void Editar(Producto producto)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"UPDATE PRODUCTO SET 
                Nombre=@Nombre, Tipo=@Tipo, Precio=@Precio, 
                StockActual=@StockActual, StockMinimo=@StockMinimo 
                WHERE ID_PRODUCTO=@ID", conn);

            cmd.Parameters.AddWithValue("@ID", producto.ID_PRODUCTO);
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@Tipo", producto.Tipo);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@StockActual", producto.StockActual);
            cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM PRODUCTO WHERE ID_PRODUCTO = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.ExecuteNonQuery();
        }
    }
}

public class Producto
{
    public int ID_PRODUCTO { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public decimal Precio { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
}