using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_DobleFactorController : ControllerBase
    {
        private readonly IConfiguration _config;

        public C_DobleFactorController(IConfiguration config)
        {
            this._config = config;
        }

        DobleFactor cls = new DobleFactor();

        [HttpGet("ObtenerCodigoSeguridad")]
        public async Task<IActionResult> ObtenerCodigoSeguridad (string idPersona, string opcionEnvio, string nombreSistema, string correoUsuario, string telefonoUsuario)
        {
            int numeroRandom = 0;
            string ahora = string.Empty;
            string resp = string.Empty;

            Resultado resultado = new Resultado();

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);

                int result = BitConverter.ToInt32(randomNumber, 0) & 0x7FFFFFFF;
                numeroRandom = result % 1000000;
                if(numeroRandom < 100000)
                {
                    byte[] numeroAleatorio = new byte[4];
                    rng.GetBytes(numeroAleatorio);

                    int resultadoPositivo = BitConverter.ToInt32(numeroAleatorio, 0) & 0x7FFFFFFF;
                    int digito = (resultadoPositivo % 9) + 1;

                    numeroRandom = int.Parse(digito.ToString() + numeroRandom.ToString("D5"));
                }
            }

            resp = await cls.EnviarCodigoVerificacion(idPersona, numeroRandom.ToString(), opcionEnvio, nombreSistema, correoUsuario, telefonoUsuario);

            if(resp == "1")
            {
                ahora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                resultado.valor = "1|" + numeroRandom.ToString() + "|" + ahora + "|" + _config.GetSection(Constantes.APP_SETTINGS_DOBLE_FACTOR).Value;
                resultado.descripcion = "";
            } else
            {
                resultado.valor = "-1|" + resp;
                resultado.descripcion = "ERROR: No se pudo enviar el código de verificación.";
            }

            return Ok(resultado);
        }

    }
}
