using RoleBasedAuthorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization
{
    /// <summary>
    /// Represents a user having certain set of roles
    /// </summary>
    public class UserRole
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public UserRole(User user, List<Role> roles)
        {
            User = user;
            Roles = roles;
        }
    }
}
