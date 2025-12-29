using System.Data.SqlClient;
using System.Collections.Generic;

public class PedidoRepository
{
    private string connectionString = Conexion.Cadena;

    public List<Pedido> Listar()
    {
        var lista = new List<Pedido>();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT P.*, C.RazonSocial, C.Nombre, C.CUIT, C.Email
                FROM PEDIDO P
                INNER JOIN CLIENTE C ON P.ID_CLIENTE = C.ID_CLIENTES", conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Pedido
                {
                    ID_PEDIDO = (int)reader["ID_PEDIDO"],
                    ID_CLIENTE = (int)reader["ID_CLIENTE"],
                    FECHA = reader["FECHA"] == DBNull.Value ? null : (DateTime?)reader["FECHA"],
                    Estado = reader["Estado"]?.ToString(),
                    Total = (decimal)reader["Total"],
                    Cliente = new Cliente
                    {
                        ID_CLIENTES = (int)reader["ID_CLIENTE"],
                        RazonSocial = reader["RazonSocial"]?.ToString(),
                        Nombre = reader["Nombre"]?.ToString(),
                        CUIT = reader["CUIT"]?.ToString(),
                        Email = reader["Email"]?.ToString()
                    }
                });
            }
        }

        return lista;
    }

    public void Agregar(Pedido pedido)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"
            INSERT INTO PEDIDO (ID_CLIENTE, FECHA, Estado, Total) 
            OUTPUT INSERTED.ID_PEDIDO
            VALUES (@ID_CLIENTE, @FECHA, @Estado, @Total)", conn);

            cmd.Parameters.AddWithValue("@ID_CLIENTE", pedido.ID_CLIENTE);
            cmd.Parameters.AddWithValue("@FECHA", (object?)pedido.FECHA ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", (object?)pedido.Estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Total", pedido.Total);

            // Captura el ID generado
            pedido.ID_PEDIDO = (int)cmd.ExecuteScalar();
        }
    }
    public void Editar(Pedido pedido)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"UPDATE PEDIDO SET 
                ID_CLIENTE = @ID_CLIENTE,
                FECHA = @FECHA,
                Estado = @Estado,
                Total = @Total
                WHERE ID_PEDIDO = @ID_PEDIDO", conn);

            cmd.Parameters.AddWithValue("@ID_PEDIDO", pedido.ID_PEDIDO);
            cmd.Parameters.AddWithValue("@ID_CLIENTE", pedido.ID_CLIENTE);
            cmd.Parameters.AddWithValue("@FECHA", (object?)pedido.FECHA ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", (object?)pedido.Estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Total", pedido.Total);

            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM PEDIDO WHERE ID_PEDIDO = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.ExecuteNonQuery();
        }
    }
}
public class DetallePedidoRepository
{
    private string connectionString = Conexion.Cadena;

    public void Agregar(DetallePedido detalle)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"INSERT INTO DETALLE_PEDIDO 
                (ID_PEDIDO, ID_PRODUCTO, Cantidad, PrecioUnitario)
                VALUES (@ID_PEDIDO, @ID_PRODUCTO, @Cantidad, @PrecioUnitario)", conn);

            cmd.Parameters.AddWithValue("@ID_PEDIDO", detalle.ID_PEDIDO);
            cmd.Parameters.AddWithValue("@ID_PRODUCTO", detalle.ID_PRODUCTO);
            cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

            cmd.ExecuteNonQuery();
        }
    }
    public void EliminarPorPedido(int idPedido)
    {
        using var conn = new SqlConnection(Conexion.Cadena);
        conn.Open();

        var cmd = new SqlCommand("DELETE FROM DETALLE_PEDIDO WHERE ID_PEDIDO = @id", conn);
        cmd.Parameters.AddWithValue("@id", idPedido);
        cmd.ExecuteNonQuery();
    }
    public List<DetallePedido> ListarPorPedido(int idPedido)
    {
        var lista = new List<DetallePedido>();

        using (var conn = new SqlConnection(Conexion.Cadena))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM DETALLE_PEDIDO WHERE ID_PEDIDO = @id", conn);
            cmd.Parameters.AddWithValue("@id", idPedido);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new DetallePedido
                {
                    ID_DETALLE = Convert.ToInt32(reader["ID_DETALLE"]),
                    ID_PEDIDO = Convert.ToInt32(reader["ID_PEDIDO"]),
                    ID_PRODUCTO = Convert.ToInt32(reader["ID_PRODUCTO"]),
                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"])
                });
            }
        }

        return lista;
    }
}

public class Pedido
{
    public int ID_PEDIDO { get; set; }
    public int ID_CLIENTE { get; set; }
    public DateTime? FECHA { get; set; }
    public string Estado { get; set; }
    public decimal Total { get; set; }
    public Cliente Cliente { get; set; }
}
public class PedidoRequest
{
    public int ID_CLIENTE { get; set; }
    public DateTime? FECHA { get; set; } = DateTime.Now;
    public string Estado { get; set; }
    public decimal Total { get; set; }
    public List<DetalleRequest> Detalle { get; set; }
}

public class DetalleRequest
{
    public int ID_PRODUCTO { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
public class DetallePedido
{
    public int ID_DETALLE { get; set; }        // opcional si es autoincremental
    public int ID_PEDIDO { get; set; }
    public int ID_PRODUCTO { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}