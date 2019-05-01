using Microsoft.AspNetCore.Mvc;
using NarutoUniverseProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Controllers
{
    public class OtherController : Controller
    {
        private readonly OtherService _otherService;

        public OtherController(OtherService otherService)
        {
            _otherService = otherService;
        }
        public IActionResult Index()
        {
            var model = _otherService.GetOtherTables();

            return View(model);
        }
    }
}
