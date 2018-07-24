using System;
using System.Collections.Generic;
namespace RoleBasedAuthorization.Models
{
    /// <summary>
    /// Represents a resource
    /// </summary>
    public class Resource
    {
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; }
        public List<UserRole> ResourceUserRoles { get; set; }
        public Resource(Guid resourceId, string name)
        {
            ResourceId = resourceId;
            ResourceName = name;
            ResourceUserRoles = new List<UserRole>();
        }
    }
}
