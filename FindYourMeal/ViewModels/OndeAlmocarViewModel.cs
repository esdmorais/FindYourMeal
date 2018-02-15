using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FindYourMeal.Views.Shared;
using System.ComponentModel;
using FindYourMeal.Models;

namespace FindYourMeal.ViewModels
{
    public class OndeAlmocarViewModel
    {
        public OndeAlmocarViewModel()
        {
            this.Pessoas = new List<CheckBoxListItem>();
            this.OpcoesDeRestaurante = new List<Restaurante>();
        }

        [DisplayName("Quem vai almoçar hoje? ")]
        public List<CheckBoxListItem> Pessoas { get; set; }

        [DisplayName("Opções para almoçar? ")]
        public List<Restaurante> OpcoesDeRestaurante;
    }
}
