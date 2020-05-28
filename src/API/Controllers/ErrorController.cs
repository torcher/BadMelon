using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // Retrieve error information in case of internal errors
            var error = HttpContext
                      .Features
                      .Get<IExceptionHandlerFeature>();

            if (error != null)
            {
                var exception = error.Error;
                using (StreamWriter sw = new StreamWriter("error.log", true))
                {
                    sw.WriteLine();
                    sw.WriteLine("*******************************************************");
                    sw.WriteLine("Exception occured at " + DateTime.Now.ToString());
                    sw.WriteLine("Message: " + exception.Message);
                    sw.WriteLine("Inner Message: " + exception.InnerException?.Message);
                    sw.WriteLine("Stack: " + exception.StackTrace);
                    sw.WriteLine("*******************************************************");
                }
            }

            return StatusCode(500);
        }
    }
}