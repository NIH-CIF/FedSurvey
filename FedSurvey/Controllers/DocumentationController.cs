using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace FedSurvey.Controllers
{
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        [Route("api/[controller]")]
        [HttpGet]
        public IActionResult List()
        {
            return new JsonResult(
                Directory.GetFiles(@"./ClientApp/public/docs")
                .Select(file => file.Replace("./ClientApp/public/docs\\", ""))
            );
        }
    }
}
