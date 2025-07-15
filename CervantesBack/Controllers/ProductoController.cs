using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoRepository _repo;

        public ProductoController()
        {
            _repo = new ProductoRepository();
        }

        // GET: api/producto
        [HttpGet]
        public ActionResult<List<Producto>> Get()
        {
            return Ok(_repo.Listar());
        }

        // GET: api/producto/5
        [HttpGet("{id}")]
        public ActionResult<Producto> Get(int id)
        {
            var producto = _repo.Listar().Find(p => p.ID_PRODUCTO == id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // POST: api/producto
        [HttpPost]
        public ActionResult Post([FromBody] Producto producto)
        {
            _repo.Agregar(producto);
            return Ok(new { mensaje = "Producto agregado correctamente" });
        }

        // PUT: api/producto/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Producto producto)
        {
            var existente = _repo.Listar().Exists(p => p.ID_PRODUCTO == id);
            if (!existente)
                return NotFound();

            producto.ID_PRODUCTO = id;
            _repo.Editar(producto);
            return Ok(new { mensaje = "Producto actualizado correctamente" });
        }

        // DELETE: api/producto/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existente = _repo.Listar().Exists(p => p.ID_PRODUCTO == id);
            if (!existente)
                return NotFound();

            _repo.Eliminar(id);
            return Ok(new { mensaje = "Producto eliminado correctamente" });
        }
    }
}