using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoleBasedAuthorization.Models;
namespace RoleBasedAuthorization.Controllers
{
    [Route("Resource")]
    public class ResourceController : Controller
    {
        private List<Resource> _resources;
        private List<User> _users;
        private List<Role> _roles;
        
        /// <summary>
        /// Initializes the resources, users and roles
        /// </summary>
        private void InitializeResources()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Resources")))
            {
                // Create users, roles and resources and add entries.
                // In case of database, we'll fetch the entries from the database
                List<Resource> resources = new List<Resource>();
                List<User> users = new List<User>();
                List<Role> roles = new List<Role>();
                roles.Add(new Role(1, "Reader", new List<Enumerations.Action>() { Enumerations.Action.Read }));
                roles.Add(new Role(2, "Writer", new List<Enumerations.Action>() { Enumerations.Action.Write }));
                users.Add(new User(1, "User1"));
                users.Add(new User(2, "User2"));
                resources.Add(new Resource(new Guid("361b1ea6-1358-48ca-a015-c6e96ab7c65f"), "Resource1"));
                resources.Add(new Resource(new Guid("31708111-e5c2-48c9-bd7a-15cf116b31ae"), "Resource2"));

                HttpContext.Session.SetString("Resources", JsonConvert.SerializeObject(resources));
                HttpContext.Session.SetString("Users", JsonConvert.SerializeObject(users));
                HttpContext.Session.SetString("Roles", JsonConvert.SerializeObject(roles));

                _resources = resources;
                _roles = roles;
                _users = users;
            } else
            {
                string resourcesString = HttpContext.Session.GetString("Resources");
                _resources = JsonConvert.DeserializeObject<List<Resource>>(resourcesString);
                _roles = JsonConvert.DeserializeObject<List<Role>>(HttpContext.Session.GetString("Roles"));
                _users = JsonConvert.DeserializeObject<List<User>>(HttpContext.Session.GetString("Users"));
            }
        }

        /// <summary>
        /// Assigns a role to the user on the resource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Route("AssignUserRole")]
        public IActionResult AssignUserRoleOnResource(Guid resourceId, int userId, int roleId)
        {
            try
            {
                InitializeResources();
                #region Input Validations
                // Check whether resource exists or not
                Resource resource = _resources.Where(x => x.ResourceId == resourceId).FirstOrDefault();
                if (resource == null)
                {
                    return StatusCode(400, "Invalid Resource");
                }
                // Check whether the user exists or not
                User user = _users.Where(x => x.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    return StatusCode(400, "Invalid user");
                }
                // Check if that role exists or not
                Role role = _roles.Where(x => x.RoleId == roleId).FirstOrDefault();
                if (role == null)
                {
                    return StatusCode(400, "Invalid role");
                }
                #endregion
                UserRole userRolesOnResource = resource.ResourceUserRoles.Where(x => x.User.UserId == user.UserId).FirstOrDefault();
                if (userRolesOnResource == null)
                {
                    resource.ResourceUserRoles.Add(new UserRole(user, new List<Role>() { role }));
                }
                else
                {
                    // Check if that role already exists for this user
                    if (userRolesOnResource.Roles.Where(x => x.RoleId == role.RoleId).FirstOrDefault() == null)
                    {
                        userRolesOnResource.Roles.Add(role);
                    }
                }
                // Update Db. In our case HttpContext
                HttpContext.Session.SetString("Resources", JsonConvert.SerializeObject(_resources));
                return Ok("Successfully assigned role to the user");
            }
            catch(Exception)
            {
                // Log exception
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Removes a role from the user on the resource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Route("RemoveUserRole")]
        public IActionResult RemoveUserRole(Guid resourceId, int userId, int roleId)
        {
            try
            {
                InitializeResources();
                #region Input Validations
                // Check whether resource exists or not
                Resource resource = _resources.Where(x => x.ResourceId == resourceId).FirstOrDefault();
                if (resource == null)
                {
                    return StatusCode(400, "Invalid Resource");
                }
                // Check whether the user exists or not
                User user = _users.Where(x => x.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    return StatusCode(400, "Invalid user");
                }
                // Check if that role exists or not
                Role role = _roles.Where(x => x.RoleId == roleId).FirstOrDefault();
                if (role == null)
                {
                    return StatusCode(400, "Invalid role");
                }
                #endregion
                // Gets the user role entry for the passed userid and role id
                UserRole rolesForCurrentUser = resource.ResourceUserRoles.Where(x => x.User.UserId == user.UserId).FirstOrDefault();
                if (rolesForCurrentUser == null)
                {
                    return StatusCode(404);
                }
                if (rolesForCurrentUser.Roles.Where(x => x.RoleId == role.RoleId).Count() > 0)
                {
                    Role curRole = rolesForCurrentUser.Roles.Where(x => x.RoleId == role.RoleId).First();
                    rolesForCurrentUser.Roles.Remove(curRole);
                }
                else
                {
                    return StatusCode(404);
                }
                // Update Db. In our case HttpContext
                HttpContext.Session.SetString("Resources", JsonConvert.SerializeObject(_resources));
                return Ok("Successfully removed role from the user");
            }
            catch (Exception)
            {
                // Log exception
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Checks whether the user is authorized to perform the action on the resource or not.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        [Route("IsAuthorized")]
        public IActionResult IsAuthorizedToPerformActionOnResource(Guid resourceId, int userId, string actionType)
        {
            try
            {
                Enumerations.Action action = Enum.Parse<Enumerations.Action>(actionType);
                InitializeResources();
                #region Input Validations
                // Check whether resource exists or not
                Resource resource = _resources.Where(x => x.ResourceId == resourceId).FirstOrDefault();
                if (resource == null)
                {
                    return StatusCode(400, "Invalid Resource");
                }
                // Check whether the user exists or not
                User user = _users.Where(x => x.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    return StatusCode(400, "Invalid user");
                }
                #endregion
                // Gets the user role entry for the passed userid and role id
                UserRole rolesForCurrentUser = resource.ResourceUserRoles.Where(x => x.User.UserId == user.UserId).FirstOrDefault();
                if (rolesForCurrentUser == null)
                {
                    return StatusCode(403, "User doesn't have the required roles to perform this action");
                }
                foreach (var curRole in rolesForCurrentUser.Roles)
                {
                    // If any of the user roles contains that action then return authorized
                    if (curRole.RoleActions.Contains(action))
                    {
                        // User is authorized to perform action
                        return Ok("Success!");
                    }
                }
                return StatusCode(403, "User doesn't have the required roles to perform this action");
            }
            catch (Exception)
            {
                // Log exception
                return StatusCode(500);
            }
        }
    }
}