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
            ViewData["Positions"] = _otherService.GetPositions();
            ViewData["Countries"] = _otherService.GetCountries();
            return View(new PersonCreateBindModel());
        }

        [HttpPost]
        public IActionResult Create(PersonCreateBindModel bindModel)
        {
            ViewData["Positions"] = _otherService.GetPositions();
            ViewData["Countries"] = _otherService.GetCountries();

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
    }
}