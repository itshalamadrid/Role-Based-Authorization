namespace RoleBasedAuthorization.Models
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public class User
    {
        public User(int userId, string name)
        {
            UserId = userId;
            Name = name;
        }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
