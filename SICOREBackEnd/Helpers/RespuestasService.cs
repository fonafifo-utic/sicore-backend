using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SicUCCBackEnd.Helpers
{
    public class RespuestasService : IRespuestasServices
    {
        public RespuestaGenerica GenerarOk(string msj, object result)
        {
            return new RespuestaGenerica
            {
                Ok = true,
                Msg = msj,
                Result = result
            };
        }

        public IActionResult GenerarError(string msjError)
        {
            return new BadRequestObjectResult(new
            {
                ok = false,
                msg = msjError,
                result = ""
            });
        }
    }
}
