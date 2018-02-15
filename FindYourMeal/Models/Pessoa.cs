using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FindYourMeal.Views.Shared;

namespace FindYourMeal.Models
{
    public class Pessoa : BaseEntity
    {
        public Pessoa()
        {
            this.Preferencias = new List<Restaurante>();
        }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Nome obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve conter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        public string Telefone { get; set; }

        public List<Restaurante> Preferencias { get; set; }
    }
}
