namespace BookManagementClient.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleModel Role { get; set; }
    }
}
