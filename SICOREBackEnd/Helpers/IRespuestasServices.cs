using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SicUCCBackEnd.Helpers
{
    public interface IRespuestasServices
    {
        RespuestaGenerica GenerarOk(string errorMessage, object result = null);
        IActionResult GenerarError(string errorMessage);
    }
}
public class RespuestaGenerica
{
    public bool Ok { get; set; }
    public string Msg { get; set; }
    public object Result { get; set; }
}
