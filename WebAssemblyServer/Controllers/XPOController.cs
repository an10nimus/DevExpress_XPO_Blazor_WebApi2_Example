using WebAssemblyServer.Services;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace WebAssemblyServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class XPOController : ControllerBase {
        readonly XpoDataStoreService DataStoreService;
        public XPOController(XpoDataStoreService dataStoreService) {
            this.DataStoreService = dataStoreService;
        }
        [HttpPost("{method}")]
        public ActionResult<string> Get(string method, [FromBody]string args) {
            return DataStoreService.Handle(method, args);
        }
    }
}