using Microsoft.AspNetCore.Mvc;

namespace ContactProMVC.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Demo()
        {
            return Ok();
        }
    }
}
