﻿using Microsoft.AspNetCore.Mvc;
using NarutoUniverseProject.Models.AbilityModels;
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
        private readonly PersonService _personService;

        public AbilityController(AbilityService abilityService, PersonService personService)
        {
            _abilityService = abilityService;
            _personService = personService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _abilityService.GetAbilities();
            model.Styles = _personService.GetItemsForCheckboxes("styles");
            model.Positions = _personService.GetItemsForCheckboxes("positions");
            model.Countries = _personService.GetItemsForCheckboxes("countries");
            model.PowerSources = _personService.GetItemsForCheckboxes("power_sources");
            ViewBag.SortingOptions = _abilityService.GetInfoForSort();
            model.MinTimeToCast = _abilityService.GetScalarTimeToCast("MIN");
            model.MaxTimeToCast = _abilityService.GetScalarTimeToCast("MAX");
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(BoxOfAbilitySummaryViewModel bindModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured adding the ability to person");
                return View(bindModel);
            }

            ViewBag.SortingOptions = _abilityService.GetInfoForSort();
            _abilityService.GetFilteredAbilities(bindModel);
            return View(bindModel);
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

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Styles = _personService.GetSelectListItems("styles");
            return View(new AbilityCreateBindModel());
        }

        [HttpPost]
        public IActionResult Create(AbilityCreateBindModel bindModel)
        {
            ViewBag.Styles = _personService.GetSelectListItems("styles");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the ability");
                return View(bindModel);
            }

            var id = _abilityService.CreateAbility(bindModel);
            if (id == -1)
            {
                ModelState.AddModelError(string.Empty, "An error occured creating the ability");
                return View(bindModel);
            }
            return RedirectToAction(nameof(View), new { id = id });
        }

        [HttpGet]
        public IActionResult Edit(Int32 id)
        {
            ViewBag.Styles = _personService.GetSelectListItems("styles");

            var model = _abilityService.GetAbilityForUpdate(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(AbilityUpdateBindModel bindModel)
        {
            ViewBag.Styles = _personService.GetSelectListItems("styles");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the person");
                return View(bindModel);
            }

            _abilityService.UpdateAbility(bindModel);

            return RedirectToAction(nameof(View), new { id = bindModel.Id });
        }

        [HttpPost]
        public IActionResult Delete(Int32 id)
        {
            _abilityService.DeleteAbility(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
