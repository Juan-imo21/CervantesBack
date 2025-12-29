using CervantesBack.Helpers;
using Microsoft.AspNetCore.Mvc;
using static CervantesBack.Helpers.recuperacionHelper;
using static CervantesBack.Models.clsLogin;
using static CervantesBack.Models.clsRecuperacion;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuarioRepository _repo = new();

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        string contrasenaHash = SeguridadHelper.ProcesarContrasena(request.Contrasena);

        var usuario = _repo.Listar()
            .FirstOrDefault(u => u.AliasLogin == request.AliasLogin && u.Contrasena == contrasenaHash);

        if (usuario == null)
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        return Ok(new LoginResponse
        {
            Nombre = usuario.Nombre,
            Rol = usuario.Rol,
            Email = usuario.Email
        });
    }

    [HttpPost("recuperar")]
    public async Task<ActionResult> EnviarCodigo([FromBody] RecuperacionRequest request)
    {
        var usuario = _repo.Listar().FirstOrDefault(x => x.Email == request.Email);
        if (usuario == null)
            return NotFound(new { mensaje = "Email no registrado" });

        string codigo = new Random().Next(100000, 999999).ToString();
        CodigoRecuperacionStorage.GuardarCodigo(request.Email, codigo);

        await new SendGridEmailHelper().EnviarCodigoAsync(request.Email, codigo);

        return Ok(new { mensaje = "Código enviado al correo" });
    }

    [HttpPost("cambiar-contrasena")]
    public ActionResult CambiarContrasena([FromBody] CambioPasswordRequest request)
    {
        if (!CodigoRecuperacionStorage.ValidarCodigo(request.Email, request.Codigo))
            return BadRequest(new { mensaje = "Código inválido o expirado" });

        var usuario = _repo.Listar().FirstOrDefault(x => x.Email == request.Email);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        usuario.Contrasena = SeguridadHelper.ProcesarContrasena(request.NuevaContrasena);
        _repo.Editar(usuario);

        return Ok(new { mensaje = "Contraseña actualizada correctamente" });
    }
}