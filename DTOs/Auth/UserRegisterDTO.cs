namespace FoodOrderSystemAPI.DTOs.Auth
{
    public class UserRegisterDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
