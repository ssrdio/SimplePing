using Microsoft.AspNetCore.Mvc;
using SimpleLsSample.Services;
using System.Text.RegularExpressions;

namespace SimpleLsSample.Controllers
{
    public class ControllerBase: Controller
    {
        LoggingService _logService;
        public ControllerBase(IServiceProvider sc)
        {
            _logService = sc.GetService<LoggingService>();
        }
        public string LogErrorAndGetFriendlyMessage(string error)
        {
            return _logService.LogErrorAndGetFriendlyMessage(error);
        }
    }
}
