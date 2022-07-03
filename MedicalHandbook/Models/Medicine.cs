using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalHandbook.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите название лекарства")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите название действующего вещества")]
        [Display(Name = "Действующее вещество")]
        public string ActiveSubstance { get; set; }

        [Required(ErrorMessage = "Введите название фармокологической группы")]
        [Display(Name = "Фармокологическая группа")]
        public string Group { get; set; }

        [Required(ErrorMessage = "Напишите описание к лекарству")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}
