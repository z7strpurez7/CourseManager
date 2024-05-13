using System.ComponentModel.DataAnnotations;

namespace CourseManagerAPI.DTOs.Auth
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        public RoleType NewRole { get; set; }
    }



    public enum RoleType
    {
        ADMIN,
        LECTURER,
        USER
    }
}
