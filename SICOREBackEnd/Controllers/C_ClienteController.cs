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
    public class C_ClienteController : ControllerBase
    {
        Cliente clsCliente = new Cliente();
        
        [HttpGet("ListarClientes")]
        public async Task<IActionResult> ListarClientes()
        {
            IEnumerable<iCliente> resultado = null;

            resultado = await clsCliente.ObtenerListadoClientes();

            return Ok(resultado);
        }

        [HttpGet("ListaClientePorId/{idCliente}")]
        public async Task<IActionResult> ListaClientePorId(int idCliente)
        {
            IEnumerable<iCliente> resultado = null;

            resultado = await clsCliente.ObtenerClientePorId (idCliente);

            return Ok(resultado);
        }

        [HttpGet("ListaClientePorIdSector/{idSector}")]
        public async Task<IActionResult> ListaClientePorIdSector(int idSector)
        {
            IEnumerable<iCliente> resultado = null;

            resultado = await clsCliente.ObtenerClientePorIdSector(idSector);

            return Ok(resultado);
        }

        [HttpGet("ListarClientesSectorTurismo")]
        public async Task<IActionResult> ListarClientesSectorTurismo()
        {
            IEnumerable<iCliente> resultado = null;

            resultado = await clsCliente.ObtenerListadoClientesSectorTurismo();

            return Ok(resultado);
        }

        [HttpGet("ListarSectores")]
        public async Task<IActionResult> ListarSectores()
        {
            IEnumerable<iSector> resultado = null;

            resultado = await clsCliente.ObtenerListadoSectores();

            return Ok(resultado);
        }

        [HttpGet("ListarActividadComercial")]
        public async Task<IActionResult> ListarActividadComercial()
        {
            IEnumerable<iActividadComercial> resultado = null;

            resultado = await clsCliente.ObtenerListadoActividadComercial();

            return Ok(resultado);
        }

        [HttpGet("ListarTipoEmpresa")]
        public async Task<IActionResult> ListarTipoEmpresa()
        {
            IEnumerable<iTipoEmpresa> resultado = null;

            resultado = await clsCliente.ObtenerListadoTipoEmpresas();

            return Ok(resultado);
        }

        [HttpGet("ListarTipoEmpresaPorId/{idSector}")]
        public async Task<IActionResult> ListarTipoEmpresaPorId(int idSector)
        {
            IEnumerable<iTipoEmpresa> resultado = null;

            resultado = await clsCliente.ObtenerListadoTipoEmpresasPorId(idSector);

            return Ok(resultado);
        }

        [HttpPost("IngresaCliente")]
        public async Task<IActionResult> IngresaCliente([FromBody] iCliente pCliente)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCliente.IngresaCliente(pCliente);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else if (respuesta == "2")
            {
                resultado.valor = "2";
                resultado.descripcion = "Correo Electrónico o Número de Cédula existentes.";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpPut("ActualizaCliente")]
        public async Task<IActionResult> ActualizaCliente([FromBody] iCliente pCliente)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCliente.ActualizaCliente(pCliente);

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

        [HttpPut("ActualizaEstadoCliente")]
        public async Task<IActionResult> ActualizaEstadoCliente([FromBody] iCliente pCliente)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCliente.ActualizaEstadoCliente(pCliente);

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

        [HttpGet("ListarSectoresCompleto")]
        public async Task<IActionResult> ListarSectoresCompleto()
        {
            IEnumerable<iSector> resultado = null;

            resultado = await clsCliente.ObtenerListadoCompletoSectores();

            return Ok(resultado);
        }
    }
}
