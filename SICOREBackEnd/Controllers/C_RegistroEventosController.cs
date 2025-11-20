using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_RegistroEventosController : ControllerBase
    {
        RegistroEventos clsFunciones = new RegistroEventos();

        [HttpPost("IngresaEvento")]
        public async Task<IActionResult> IngresaEvento([FromBody] ModeloRegistroEventos pEvento)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsFunciones.IngresaRegistros(pEvento);

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
