namespace RoleBasedAuthorization.Models
{
    /// <summary>
    /// Denotes the actions that can be performed on a resource
    /// </summary>
    public class Enumerations
    {
        /// <summary>
        /// Represents the actions that can be performed on a resource
        /// </summary>
        public enum Action
        {
            Read = 1,
            Write, 
            Delete
        }
    }
}
