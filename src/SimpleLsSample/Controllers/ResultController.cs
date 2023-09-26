using Microsoft.AspNetCore.Mvc;
using SimpleLsSample.Interfaces;

namespace SimpleLsSample.Controllers
{
    public class ResultController : MvcBaseContoller
    {
        private readonly IResultBl _resultBL;

        public ResultController(IResultBl resultBL)
        {
            _resultBL = resultBL;
        }

        // GET: Result/
        public IActionResult Index()
        {
            string viewName = _resultBL.GetIndexViewName();

            return View(viewName);
        }

        // GET: Result/Find/search
        public IActionResult Find(string? search)
        {
            ViewBag.showResults = true;
            var res = _resultBL.PingDomain(search);
            if (res == null)
            {
                return NotFound();
            }

            return View("Find", res);
        }



    }
}
