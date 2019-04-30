﻿using Microsoft.AspNetCore.Mvc;
using NarutoUniverseProject.Models.PersonModels;
using NarutoUniverseProject.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;


namespace NarutoUniverseProject.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonService _personService;
        private readonly OtherService _otherService;

        public PersonController(PersonService personService, OtherService otherService)
        {
            _personService = personService;
            _otherService = otherService;
        }
        public IActionResult Index()
        {
            var models = _personService.GetPersons();
            return View(models);
        }

        public IActionResult View(Int32 id)
        {
            var model = _personService.GetPerson(id);
            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Positions = _personService.GetSelectListItems("positions");
            ViewBag.Countries = _personService.GetSelectListItems("countries");
            return View(new PersonCreateBindModel());
        }

        [HttpPost]
        public IActionResult Create(PersonCreateBindModel bindModel)
        {
            ViewBag.Positions = _personService.GetSelectListItems("positions");
            ViewBag.Countries = _personService.GetSelectListItems("countries");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the person");
                return View(bindModel);
            }

            var id = _personService.CreatePerson(bindModel);
            if(id == -1)
            {
                ModelState.AddModelError(string.Empty, "An error occured creating the person");
                return View(bindModel);
            }
            return RedirectToAction(nameof(View), new { id = id });
        }

        [HttpGet]
        public IActionResult Edit(Int32 id)
        {
            ViewBag.Positions = _personService.GetSelectListItems("positions");
            ViewBag.Countries = _personService.GetSelectListItems("countries");

            var model = _personService.GetPersonForUpdate(id);
            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PersonUpdateBindModel bindModel)
        {
            ViewBag.Positions = _personService.GetSelectListItems("positions");
            ViewBag.Countries = _personService.GetSelectListItems("countries");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the person");
                return View(bindModel);
            }

            _personService.UpdatePerson(bindModel);
            
            return RedirectToAction(nameof(View), new { id = bindModel.Id });
        }

        [HttpPost]
        public IActionResult Delete(Int32 id)
        {
            _personService.DeletePerson(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddAbility(Int32 id)
        {
            var model = _personService.GetAbilitiesForAdding(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddAbility(PersonAddAbilityBindModel bindModel)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occured adding the ability to person");
                return View(bindModel);
            }
            _personService.AddAbility(bindModel);
            return RedirectToAction(nameof(View), new { id = bindModel.Id });
        }

        [HttpPost]
        public IActionResult DeleteAbility(Int32 id, Int32 personId)
        {
            _personService.DeleteAbilityOfPerson(id, personId);
            return RedirectToAction(nameof(View), new { id = personId });
        }
    }
}
