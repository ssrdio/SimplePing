using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleLsSample.DAL.DAO;
using System.Text.RegularExpressions;

namespace SimpleLsSample.Controllers
{
    [Produces("application/json")]
    [Route("api/pingerlogs/[action]")]
    public class PingerLogsController : ControllerBase
    {
        private PingerDBContext _dbContext;
        public PingerLogsController(PingerDBContext pingerDBContext, IServiceProvider sc):base(sc)
        {
            _dbContext = pingerDBContext;
        }
        public IActionResult GetLogs(string from)
        {
            string response = "";
            try
            {
                var resp = _dbContext.Pings.Where(t => t.DateTime > DateTime.Parse(from)).Select(t => new
                {
                    DateTime = t.DateTime,
                    Domain = t.Domain
                }).ToList();
                response = JsonConvert.SerializeObject(resp);
            }

            catch (Exception ex)
            {
                response = LogErrorAndGetFriendlyMessage(ex.InnerException.Message);
            }
            return Ok(response);
        }
    }
}
