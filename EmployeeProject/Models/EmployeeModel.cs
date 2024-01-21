using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeProject.Models
{
    [Table("EmployeeLogin")]
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your User Name.")]
        [Display(Name = "User Name")]
        [MaxLength(15)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Password.")]
        [DataType(DataType.Password)]
        [MaxLength(15)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your Date of Birth.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }


    }
}
