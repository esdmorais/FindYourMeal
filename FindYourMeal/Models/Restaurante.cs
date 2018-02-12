using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FindYourMeal.Models
{
    public class Restaurante : BaseEntity
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string Nome { get; set; }
    }
}
