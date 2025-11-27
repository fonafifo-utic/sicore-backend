using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        public static IWebHostEnvironment _environment;
        Usuario clsFunciones = new Usuario();

        public C_UsuarioController(IConfiguration config, IWebHostEnvironment environment)
        {
            this._config = config;
            _environment = environment;
        }

        [HttpPost("DoLogin")]
        public async Task<IActionResult> DoLogin(LoginIngreso objLogin)
        {
            LoginSalida resultado = new LoginSalida();
            IEnumerable<LoginSalida> resultadoConsulta = await clsFunciones.DoLogin(objLogin);
            
            try
            {
                foreach(var item in resultadoConsulta)
                {
                    if(item.idPersona != 0)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, item.idPersona.ToString()),
                            new Claim(ClaimTypes.Name, item.nombreCompleto)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection(Constantes.APP_SETTINGS_TOKEN).Value));
                        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = credenciales
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        resultado = item;
                        resultado.token = tokenHandler.WriteToken(token);
                    } else
                    {
                        resultado = item;
                    }
                }

            } catch (Exception ex)
            {
                resultado.token = ex.Message;
            }

            return Ok(resultado);
        }

        [HttpGet("ListarUsuariosRegistro")]
        public async Task<IActionResult> ListarUsuariosRegistro()
        {
            IEnumerable<ListaUsuariosRegistrar> resultado = null;
            string idPerfil = string.Empty;
            resultado = await clsFunciones.ObtenerListadoUsuariosARegistrar();

            return Ok(resultado);
        }

        [HttpPost("RegistraUsuario")]
        public async Task<IActionResult> IngresaInventario([FromBody] iUsuarioElegidoParaRegistrar pUsuario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsFunciones.RegistraUsuario(pUsuario);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [Authorize]
        [HttpPut("ActualizarClave")]
        public async Task<IActionResult> ActualizarClave(ParamLogin_ClienteTask objUsuario)
        {
            Resultado resultado = new Resultado();
            string respuesta = "";

            respuesta = await clsFunciones.ActualizarClave(objUsuario);
            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "La contraseña se actualizó exitosamente. Ahora puede ingresar utilizando su nueva contraseña.";
            }
            else
            {
                if (respuesta == "-1")
                {
                    resultado.valor = "-1";
                    resultado.descripcion = "La contraseña ingresada ya fue utilizada dentro de las últimas 6. Favor digitar otra.";
                }
                else
                {
                    if (respuesta == "2")
                    {
                        resultado.valor = "2";
                        resultado.descripcion = "ATENCIÓN: No puede utilizar la misma contraseña temporal. Debe utilizar otra";
                    }
                    else
                    {
                        resultado.valor = "-2";
                        resultado.descripcion = respuesta;
                    }

                }
            }

            return Ok(resultado);
        }

        [HttpGet("ListarUsuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            IEnumerable<IUsuario> resultado = null;
            string idPerfil = string.Empty;
            resultado = await clsFunciones.ObtenerListadoUsuarios(idPerfil);

            return Ok(resultado);
        }

        [HttpGet("ListaUsuarioPorId/{idUsuario}")]
        public async Task<IActionResult> ListaUsuarioPorId(int idUsuario)
        {
            IEnumerable<IUsuarioSugerido> resultado = null;
            resultado = await clsFunciones.ObtenerUsuarioPorId(idUsuario);

            return Ok(resultado);
        }

        [HttpGet("ListarPerfiles")]
        public async Task<IActionResult> ListarPerfiles()
        {
            IEnumerable<IPerfil> resultado = null;
            resultado = await clsFunciones.ObtenerListadoPerfil();

            return Ok(resultado);
        }

        [HttpGet("TraePersona/{idDocumento}")]
        public async Task<IActionResult> TraePersona(string idDocumento)
        {
            IEnumerable<IUsuarioSugerido> resultado = null;
            resultado = await clsFunciones.ObtenerPersona(idDocumento);

            return Ok(resultado);
        }

        [HttpPost("ActualizaUsuario")]
        public async Task<IActionResult> ActualizaUsuario([FromBody] ParamAdminUsuario pUsuario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsFunciones.ActualizaUsuario(pUsuario);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpPost("CambiaEstadoUsuario")]
        public async Task<IActionResult> CambiaEstadoUsuario([FromBody] iUsuarioElegidoParaRegistrar pUsuario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsFunciones.CambiaEstadoUsuario(pUsuario);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpPost("CambiaPerfilUsuario")]
        public async Task<IActionResult> CambiaPerfilUsuario([FromBody] iUsuarioElegidoParaRegistrar pUsuario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsFunciones.CambiaPerfilUsuario(pUsuario);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }
    }
}
