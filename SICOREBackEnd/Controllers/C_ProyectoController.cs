using Microsoft.AspNetCore.Http;
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
    public class C_ProyectoController : ControllerBase
    {
        Proyecto clsProyecto = new Proyecto();

        [HttpPost("IngresaUnProyecto")]
        public async Task<IActionResult> IngresaUnProyecto([FromBody] iProyecto pProyecto)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsProyecto.IngresaUnProyecto(pProyecto);

            if (respuesta == "1" || respuesta == "2")
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

        [HttpGet("ListarProyectos")]
        public async Task<IActionResult> ListarProyectos()
        {
            IEnumerable<iProyecto> resultado = null;

            resultado = await clsProyecto.ObtenerListadoProyectos();

            return Ok(resultado);
        }

        [HttpGet("ListarProyectosActivos")]
        public async Task<IActionResult> ListarProyectosActivos()
        {
            IEnumerable<iProyecto> resultado = null;

            resultado = await clsProyecto.ObtenerListadoProyectosActivos();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoProyectosSinInventario")]
        public async Task<IActionResult> ObtenerListadoProyectosSinInventario()
        {
            IEnumerable<iProyecto> resultado = null;

            resultado = await clsProyecto.ObtenerListadoProyectosSinInventario();

            return Ok(resultado);
        }

        [HttpGet("ListarProyectosConRemanente")]
        public async Task<IActionResult> ListarProyectosConRemanente()
        {
            IEnumerable<iProyecto> resultado = null;

            resultado = await clsProyecto.ObtenerListadoProyectosConRemanente();

            return Ok(resultado);
        }

        [HttpGet("ListaProyectoPorId/{id}")]
        public async Task<IActionResult> ListaProyectoPorId(int id)
        {
            IEnumerable<iProyecto> resultado = null;

            resultado = await clsProyecto.ObtenerProyectoPorId(id);

            return Ok(resultado);
        }

        [HttpPut("ActualizaProyecto")]
        public async Task<IActionResult> ActualizaProyecto([FromBody] iProyecto pProyecto)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsProyecto.ActualizaProyecto(pProyecto);

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

        [HttpPut("ActualizaEstadoProyecto")]
        public async Task<IActionResult> ActualizaEstadoProyecto([FromBody] iEstadoProyecto pProyecto)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsProyecto.ActualizaEstadoProyecto(pProyecto);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else if (respuesta == "2")
            {
                resultado.valor = respuesta;
                resultado.descripcion = "El proyecto tiene asignado un remanente.";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpGet("TraeRutaExpedientePorId/{idProyecto}")]
        public async Task<IActionResult> TraeRutaExpedientePorId(int idProyecto)
        {
            IEnumerable<iRutaExpediente> resultado = null;

            resultado = await clsProyecto.ObtenerRutaExpedientePorId(idProyecto);

            return Ok(resultado);
        }


    }
}
