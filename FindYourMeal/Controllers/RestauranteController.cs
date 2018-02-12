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

        public IActionResult Restaurantes()
        {
            return View(restauranteRepository.FindAll());
        }

        public IActionResult NovoRestaurante()
        {
            return View();
        }

        // GET: /Restaurante/Editar/1
        public IActionResult Editar(int? id)
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
            return View("EditarRestaurante", restaurante);
        }

        // POST: /Restaurante/Salvar   
        [HttpPost]
        public IActionResult Salvar(Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                if (restaurante.ID != 0)
                {
                    restauranteRepository.Update(restaurante);
                }
                else
                {
                    restauranteRepository.Add(restaurante);
                }

                return RedirectToAction("Restaurantes");
            }
            return View(restaurante);
        }

        // GET:/Restaurante/Delete/1
        public IActionResult Apagar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            restauranteRepository.Remove(id.Value);
            return RedirectToAction("Restaurantes");
        }
    }
}