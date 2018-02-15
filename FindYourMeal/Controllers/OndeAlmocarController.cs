using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FindYourMeal.Models;
using FindYourMeal.Repositories;
using FindYourMeal.ViewModels;
using FindYourMeal.Views.Shared;
using Microsoft.Extensions.Configuration;

namespace FindYourMeal.Controllers
{
    public class OndeAlmocarController : Controller
    {
        private readonly PessoaRepository pessoaRepository;
        private readonly RestauranteRepository restauranteRepository;

        public OndeAlmocarController(IConfiguration configuration)
        {
            pessoaRepository = new PessoaRepository(configuration);
            restauranteRepository = new RestauranteRepository(configuration);
        }

        public IActionResult OndeAlmocar()
        {
            OndeAlmocarViewModel ondeAlmocar = new OndeAlmocarViewModel();
            PreencherCheckboxDePessoas(ondeAlmocar);
            return View(ondeAlmocar);
        }

        private void PreencherCheckboxDePessoas(OndeAlmocarViewModel ondeAlmocar)
        {
            IEnumerable<Pessoa> pessoas = pessoaRepository.FindAll();

            foreach (var pessoa in pessoas)
            {
                ondeAlmocar.Pessoas.Add(new CheckBoxListItem()
                {
                    ID = (int)pessoa.ID,
                    Display = pessoa.Nome,
                    IsChecked = false
                });
            }
        }

        // POST: OndeAlmocar/BuscarRestaurantes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuscarRestaurantes(OndeAlmocarViewModel ondeAlmocar)
        {
            List<Pessoa> pessoas = new List<Pessoa>();

            foreach (var pessoaSelecionada in ondeAlmocar.Pessoas.Where(x => x.IsChecked))
            {
                pessoas.Add(new Pessoa()
                {
                    ID = pessoaSelecionada.ID
                });
            }

            ondeAlmocar.OpcoesDeRestaurante = restauranteRepository.FindOpcoesDasPessoas(pessoas);

            // TODO : Arrumar uma forma de trazer os itens dos checkboxes do post, para não ter que buscar no banco.
            foreach (var pessoa in pessoaRepository.FindAll())
            {
                ondeAlmocar.Pessoas.Where(x => x.ID == pessoa.ID).First<CheckBoxListItem>().Display = pessoa.Nome;
            }

            return View("OndeAlmocar", ondeAlmocar);
        }
    }
}