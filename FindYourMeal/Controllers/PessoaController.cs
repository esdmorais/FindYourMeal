using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindYourMeal.Models;
using FindYourMeal.Repositories;
using FindYourMeal.ViewModels;
using FindYourMeal.Views.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindYourMeal.Controllers
{
    public class PessoaController : Controller
    {
        private readonly PessoaRepository pessoaRepository;
        private readonly RestauranteRepository restauranteRepository;

        public PessoaController(IConfiguration configuration)
        {
            pessoaRepository = new PessoaRepository(configuration);
            restauranteRepository = new RestauranteRepository(configuration);
        }

        // GET: Pessoas
        public ActionResult Pessoas()
        {
            return View(pessoaRepository.FindAll());
        }

        // GET: Pessoa/Detalhes/5
        public ActionResult Detalhes(int id)
        {
            return View();
        }

        // GET: Pessoa/AdicionarPessoa
        public ActionResult AdicionarPessoa()
        {
            PessoaViewModel pessoaViewModel = new PessoaViewModel();
            PreencherListaRestaurantesDisponiveis(pessoaViewModel);
            return View(pessoaViewModel);
        }

        private void PreencherListaRestaurantesDisponiveis(PessoaViewModel pessoaViewModel)
        {
            var restaurantes = restauranteRepository.FindAll();
            var restaurantesPreferidos = new List<CheckBoxListItem>();
            foreach (var restaurante in restaurantes)
            {
                restaurantesPreferidos.Add(new CheckBoxListItem()
                {
                    ID = (int)restaurante.ID,
                    Display = restaurante.Nome,
                    IsChecked = false
                });
            }

            pessoaViewModel.Preferencias = restaurantesPreferidos;
        }

        // POST: Pessoa/Salvar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Salvar(PessoaViewModel pessoaViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pessoa = new Pessoa()
                    {
                        ID = pessoaViewModel.ID,
                        Nome = pessoaViewModel.Nome,
                        Telefone = pessoaViewModel.Telefone
                    };

                    foreach (var restauranteViewModel in pessoaViewModel.Preferencias)
                    {
                        if (restauranteViewModel.IsChecked)
                        {
                            var restaurante = new Restaurante()
                            {
                                ID = restauranteViewModel.ID,
                                Nome = restauranteViewModel.Display
                            };
                            pessoa.Preferencias.Add(restaurante);
                        }
                    }

                    if (pessoaViewModel.ID != 0)
                    {
                        pessoaRepository.Update(pessoa);
                    }
                    else
                    {
                        pessoaRepository.Add(pessoa);
                    }

                    return RedirectToAction(nameof(Pessoas));
                }
                return View(pessoaViewModel);
            }
            catch
            {
                return View("EditarPessoa");
            }
        }

        // GET: Pessoa/Editar/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Pessoa pessoa = pessoaRepository.FindByID(id.Value);
            if (pessoa == null)
            {
                return NotFound();
            }
            var pessoaViewModel = new PessoaViewModel()
            {
                ID = pessoa.ID,
                Nome = pessoa.Nome,
                Telefone = pessoa.Telefone
            };
            PreencherListaRestaurantesDisponiveis(pessoaViewModel);
            foreach (var restaurantes in pessoa.Preferencias)
            {
                pessoaViewModel.Preferencias.Find(m => m.ID == restaurantes.ID).IsChecked = true;
            }
            return View("EditarPessoa", pessoaViewModel);
        }

        // POST: Pessoa/Apagar/5
        public ActionResult Apagar(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                pessoaRepository.Remove(id.Value);
                return RedirectToAction(nameof(Pessoas));
            }
            catch
            {
                return View(nameof(Pessoas));
            }
        }
    }
}