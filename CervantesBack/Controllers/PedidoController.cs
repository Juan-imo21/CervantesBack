using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoRepository _repo = new();

        [HttpGet]
        public ActionResult<List<Pedido>> Get()
        {
            return Ok(_repo.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<Pedido> Get(int id)
        {
            var pedido = _repo.Listar().Find(p => p.ID_PEDIDO == id);
            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpPost]
        public ActionResult Post([FromBody] PedidoRequest pedidoReq)
        {
            var clienteExiste = new ClienteRepository().Listar()
                .Any(c => c.ID_CLIENTES == pedidoReq.ID_CLIENTE);

            if (!clienteExiste)
                return BadRequest(new { mensaje = "El cliente especificado no existe." });

            var pedido = new Pedido
            {
                ID_CLIENTE = pedidoReq.ID_CLIENTE,
                FECHA = pedidoReq.FECHA ?? DateTime.Now,
                Estado = pedidoReq.Estado ?? "Pendiente",
                Total = pedidoReq.Total
            };

            _repo.Agregar(pedido);

            if (pedido.ID_PEDIDO == 0)
                return BadRequest(new { mensaje = "No se pudo crear el pedido." });

            foreach (var detalle in pedidoReq.Detalle)
            {
                var detallePedido = new DetallePedido
                {
                    ID_PEDIDO = pedido.ID_PEDIDO,
                    ID_PRODUCTO = detalle.ID_PRODUCTO,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                new DetallePedidoRepository().Agregar(detallePedido);
            }

            return Ok(new
            {
                mensaje = "Pedido y detalle agregados correctamente",
                idPedido = pedido.ID_PEDIDO
            });
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PedidoRequest pedidoReq)
        {
            var existe = _repo.Listar().Exists(p => p.ID_PEDIDO == id);
            if (!existe)
                return NotFound();

            var pedido = new Pedido
            {
                ID_PEDIDO = id,
                ID_CLIENTE = pedidoReq.ID_CLIENTE,
                FECHA = pedidoReq.FECHA ?? DateTime.Now,
                Estado = pedidoReq.Estado ?? "Pendiente",
                Total = pedidoReq.Total
            };

            _repo.Editar(pedido);

            var detalleRepo = new DetallePedidoRepository();
            detalleRepo.EliminarPorPedido(id);

            foreach (var detalle in pedidoReq.Detalle)
            {
                var nuevoDetalle = new DetallePedido
                {
                    ID_PEDIDO = id,
                    ID_PRODUCTO = detalle.ID_PRODUCTO,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                detalleRepo.Agregar(nuevoDetalle);
            }

            return Ok(new { mensaje = "Pedido y su detalle actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existe = _repo.Listar().Exists(p => p.ID_PEDIDO == id);
            if (!existe)
                return NotFound();

            new DetallePedidoRepository().EliminarPorPedido(id);

            _repo.Eliminar(id);

            return Ok(new { mensaje = "Pedido y su detalle eliminados correctamente" });
        }
        [HttpGet("conDetalle/{id}")]
        public ActionResult GetConDetalle(int id)
        {
            var pedido = _repo.Listar().FirstOrDefault(p => p.ID_PEDIDO == id);
            if (pedido == null)
                return NotFound();

            var detalle = new DetallePedidoRepository().ListarPorPedido(id);

            return Ok(new
            {
                pedido.ID_PEDIDO,
                pedido.ID_CLIENTE,
                pedido.FECHA,
                pedido.Estado,
                pedido.Total,
                Detalle = detalle.Select(d => new
                {
                    d.ID_PRODUCTO,
                    d.Cantidad,
                    d.PrecioUnitario
                })
            });
        }
    }
}