using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_CorreoController : ControllerBase
    {
        Correo clscorreo = new Correo();

        [HttpPost("EnviarCorreo")]
        public async Task<IActionResult> EnviarCorreo([FromBody] ModeloCorreo objCorreo)
        {
            Resultado resultado = new Resultado();
            string respuesta = string.Empty;
            respuesta = await clscorreo.Enviar(objCorreo);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "OK";
            }
            else
            {
                if (respuesta == "-1")
                {
                    resultado.valor = "-1";
                    resultado.descripcion = "ATENCIÓN: El correo o la cédula digitada no tiene una cuenta de usuario registrada en el sistema. Favor verificar.";
                }
                else
                {
                    resultado.valor = "-2";
                    resultado.descripcion = respuesta;
                }

            }

            return Ok(resultado);
        }
    }
}
