using System.ComponentModel.DataAnnotations;

namespace CourseManagerAPI.DTOs.Auth
{
    public class RegisterDto
    {
   
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
      
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required, min 8 characters")]
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; }
    }
}
