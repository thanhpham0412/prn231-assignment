using System.ComponentModel.DataAnnotations;

namespace BookManagementClient.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [MinLength(8, ErrorMessage = "Password must containt more than 8 characters")]
        public string Password { get; set; }
        public RoleModel Role { get; set; }
    }
}
