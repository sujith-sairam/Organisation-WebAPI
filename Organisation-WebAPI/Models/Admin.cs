namespace Organisation_WebAPI.Models
{
    public class Admin
    {
        public int Id { get; set; }
        // Navigation property for User
        public int UserId { get; set; }
        public User User {  get; set; }
    }
}
