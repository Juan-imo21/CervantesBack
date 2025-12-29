using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsuariosController : ControllerBase
	{
		private readonly UsuarioRepository _repo;

		public UsuariosController()
		{
			_repo = new UsuarioRepository();
		}

		// GET: api/usuarios
		[HttpGet]
		public ActionResult<List<Usuario>> Get()
		{
			var usuarios = _repo.Listar();
			return Ok(usuarios);
		}

		// GET: api/usuarios/5
		[HttpGet("{id}")]
		public ActionResult<Usuario> Get(int id)
		{
			var usuarios = _repo.Listar();
			var usuario = usuarios.Find(x => x.ID_USER == id);
			if (usuario == null)
				return NotFound();

			return Ok(usuario);
		}

		// POST: api/usuarios
		[HttpPost]
		public ActionResult Post([FromBody] Usuario usuario)
		{
			_repo.Agregar(usuario);
			return Ok(new { mensaje = "Usuario agregado correctamente" });
		}

		// PUT: api/usuarios/5
		[HttpPut("{id}")]
		public ActionResult Put(int id, [FromBody] Usuario usuario)
		{
			var usuarios = _repo.Listar();
			var existe = usuarios.Exists(x => x.ID_USER == id);

			if (!existe)
				return NotFound();

			usuario.ID_USER = id;
			_repo.Editar(usuario);

			return Ok(new { mensaje = "Usuario actualizado correctamente" });
		}

		// DELETE: api/usuarios/5
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			var usuarios = _repo.Listar();
			var existe = usuarios.Exists(x => x.ID_USER == id);

			if (!existe)
				return NotFound();

			_repo.Eliminar(id);
			return Ok(new { mensaje = "Usuario eliminado correctamente" });
		}
	}
}