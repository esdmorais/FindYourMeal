using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FindYourMeal.Views.Shared;
using System.ComponentModel;

namespace FindYourMeal.ViewModels
{
    public class PessoaViewModel
    {
        public PessoaViewModel()
        {
            this.Preferencias = new List<CheckBoxListItem>();
        }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Nome obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve conter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        [StringLength(20, ErrorMessage = "O telefone deve conter no máximo 20 caracteres.")]
        public string Telefone { get; set; }

        [DisplayName("Restaurantes preferidos: ")]
        public List<CheckBoxListItem> Preferencias { get; set; }
    }
}
