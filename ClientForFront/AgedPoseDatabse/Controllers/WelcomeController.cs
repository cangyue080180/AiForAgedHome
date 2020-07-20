using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AgedPoseDatabse.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Welcome()
        {
            return "Hello,MyDatabaseServer is Start!";
        }
    }
}
