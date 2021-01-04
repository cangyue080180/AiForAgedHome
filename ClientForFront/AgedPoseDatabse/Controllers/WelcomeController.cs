using Microsoft.AspNetCore.Mvc;

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