using System.Collections.Generic;
namespace RoleBasedAuthorization.Models
{
    /// <summary>
    /// Represents a role 
    /// </summary>
    public class Role
    {
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public List<Enumerations.Action> RoleActions { get; set; }
        public Role(int roleId, string roleName, List<Enumerations.Action> actions)
        {
            RoleId = roleId;
            RoleName = roleName;
            RoleActions = actions;
        }
    }
}
