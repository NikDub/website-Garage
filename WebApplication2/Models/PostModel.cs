using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class PostModel
    {
            [Required(ErrorMessage = "Не указан Email")]
            public string Headline { get; set; }

            [Required(ErrorMessage = "Не указан пароль")]
            public string MainText { get; set; }
    }
}
