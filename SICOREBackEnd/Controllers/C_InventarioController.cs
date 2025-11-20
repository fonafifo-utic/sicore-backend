using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_InventarioController : ControllerBase
    {
        Inventario clsInventario = new Inventario();

        [HttpGet("ListarInventario")]
        public async Task<IActionResult> ListarInventario()
        {
            IEnumerable<ModeloInventario> resultado = null;

            resultado = await clsInventario.ObtenerListadoInventario();

            return Ok(resultado);
        }

        [HttpGet("ListaInventarioPorId/{id}")]
        public async Task<IActionResult> ListaInventarioPorId(int id)
        {
            IEnumerable<ModeloInventario> resultado = null;

            resultado = await clsInventario.ObtenerInventarioPorId(id);

            return Ok(resultado);
        }

        [HttpPost("IngresaInventario")]
        public async Task<IActionResult> IngresaInventario ([FromBody] iIngresaMovimiento pInventario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsInventario.IngresaInventario(pInventario);

            if(respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            } else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpPut("ActualizaInventario")]
        public async Task<IActionResult> ActualizaInventario([FromBody] iIngresaMovimiento pInventario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsInventario.ActualizaInventario(pInventario);

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

        [HttpPut("ActualizaInventarioAumento")]
        public async Task<IActionResult> ActualizaInventarioAumento([FromBody] iIngresaMovimiento pInventario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsInventario.ActualizaInventarioAumento(pInventario);

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

        [HttpGet("ListarMovimientos/{id}")]
        public async Task<IActionResult> ListarMovimientos(int id)
        {
            IEnumerable<iMovimiento> resultado = null;

            resultado = await clsInventario.ObtenerListadoMovimientos(id);

            return Ok(resultado);
        }

        [HttpPut("CambiaEstadoInventario")]
        public async Task<IActionResult> CambiaEstadoInventario([FromBody] ModeloInventario pInventario)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsInventario.CambiaEstadoInventario(pInventario);

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
