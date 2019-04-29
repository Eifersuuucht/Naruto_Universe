using Microsoft.AspNetCore.Mvc;
using NarutoUniverseProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoUniverseProject.Controllers
{
    public class AbilityController : Controller
    {
        private readonly AbilityService _abilityService;

        public AbilityController(AbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        public IActionResult Index()
        {
            var model = _abilityService.GetAbilities();
            return View(model);
        }

        public IActionResult View(Int32 id)
        {
            var model = _abilityService.GetAbility(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
