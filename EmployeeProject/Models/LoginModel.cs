using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EmployeeProject.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
