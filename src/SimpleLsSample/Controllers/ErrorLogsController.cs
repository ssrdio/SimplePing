using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimpleLsSample.DAL.DAO;
using System.Text.RegularExpressions;

namespace SimpleLsSample.Controllers
{
    [Produces("application/json")]
    [Route("api/errorlogs/[action]")]
    public class ErrorLogsController : ControllerBase
    {
        private PingerDBContext _dbContext;
        public ErrorLogsController(PingerDBContext pingerDBContext, IServiceProvider sc):base(sc)
        {
            _dbContext = pingerDBContext;
        }
        public IActionResult GetLogs(string from)
        {
            string response = "";
            try
            {
                //_dbContext.FriendlyMessages.FromSqlRaw
                var resp = _dbContext.Errors.FromSqlRaw<Error>($"Select * from Errors where DateTime > '{DateTime.Parse(from).ToString()}'").Select(t => new
                {
                    DateTime = t.Message,
                    Domain = t.DateTime
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
