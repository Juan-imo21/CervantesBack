using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteRepository _repo = new();

        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {
            return Ok(_repo.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> Get(int id)
        {
            var cliente = _repo.Listar().Find(c => c.ID_CLIENTES == id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Cliente cliente)
        {
            _repo.Agregar(cliente);
            return Ok(new { mensaje = "Cliente agregado correctamente" });
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Cliente cliente)
        {
            var existe = _repo.Listar().Exists(c => c.ID_CLIENTES == id);
            if (!existe)
                return NotFound();

            cliente.ID_CLIENTES = id;
            _repo.Editar(cliente);
            return Ok(new { mensaje = "Cliente actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existe = _repo.Listar().Exists(c => c.ID_CLIENTES == id);
            if (!existe)
                return NotFound();

            _repo.Eliminar(id);
            return Ok(new { mensaje = "Cliente eliminado correctamente" });
        }
    }
}