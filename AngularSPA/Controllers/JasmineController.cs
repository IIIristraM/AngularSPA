using System;
using System.Web.Mvc;

namespace App.AudioSearcher.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
