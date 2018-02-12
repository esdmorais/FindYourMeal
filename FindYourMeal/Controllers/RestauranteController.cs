using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FindYourMeal.Models;
using FindYourMeal.Repositories;

namespace FindYourMeal.Controllers
{
    public class RestauranteController : Controller
    {
        private readonly RestauranteRepository restauranteRepository;

        public RestauranteController(IConfiguration configuration)
        {
            restauranteRepository = new RestauranteRepository(configuration);
        }

        public IActionResult Index()
        {
            return View(restauranteRepository.FindAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurante/Create
        [HttpPost]
        public IActionResult Create(Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                restauranteRepository.Add(restaurante);
                return RedirectToAction("Index");
            }
            return View(restaurante);
        }

        // GET: /Restaurante/Edit/1
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Restaurante restaurante = restauranteRepository.FindByID(id.Value);
            if (restaurante == null)
            {
                return NotFound();
            }
            return View(restaurante);
        }

        // POST: /Restaurante/Edit   
        [HttpPost]
        public IActionResult Edit(Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                restauranteRepository.Update(restaurante);
                return RedirectToAction("Index");
            }
            return View(restaurante);
        }

        // GET:/Restaurante/Delete/1
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            restauranteRepository.Remove(id.Value);
            return RedirectToAction("Index");
        }
    }
}