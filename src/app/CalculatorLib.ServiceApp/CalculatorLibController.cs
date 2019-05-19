using NLog;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CalculatorLib.ServiceApp
{
    //http://localhost:10080/api/CalculatorLib/...
    public class CalculatorLibController : ApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        //http://localhost:10080/api/CalculatorLib/getbyid?id=someid
        [HttpGet]
        public Task<HttpResponseMessage> GetById(string id)
        {
            Log.Trace("Get by id");
            return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, id));
        }
    }
}