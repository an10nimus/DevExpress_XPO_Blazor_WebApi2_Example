using DxSample.Server.Services;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace DxSample.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class XPOController : ControllerBase {
        readonly XpoDataStoreService DataStoreService;
        public XPOController(XpoDataStoreService dataStoreService) {
            this.DataStoreService = dataStoreService;
        }
        [HttpPost("{method}")]
        public ActionResult<XPOContentHolder> Get(string method, [FromBody]XPOContentHolder args) {
            return new XPOContentHolder {
                content = DataStoreService.Handle(method, args.content)
            };
        }
    }

    public class XPOContentHolder {
        public string content { get; set; }
    }
}