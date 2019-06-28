using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Не заполнено поле логин")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина логина 3-50 символов")]
        [RegularExpression(@"[A-Za-z0-9._-]{3,16}", ErrorMessage = "Некорректный логин")]
        public string Login { get; set; }

         
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не заполнено поле пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля 6-50 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
