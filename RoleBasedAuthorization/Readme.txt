Helper document for the solution RoleBasedAuthorization

In order to run this example, open the solution in any IDE (Visual studio preferably)

Press F5 or click on run

To test various scenarios, use these urls:
To Assign a user role on a resource: http://localhost:56025/Resource/AssignUserRole?resourceId=<resourceId>&userId=<userId>&roleId=<roleId>
Ex: http://localhost:56025/Resource/AssignUserRole?resourceId=361b1ea6-1358-48ca-a015-c6e96ab7c65f&userId=1&roleId=1

To Remove a user role on a resource: http://localhost:56025/Resource/RemoveUserRole?resourceId=<resourceId>&userId=<userId>&roleId=<roleId>
Ex: http://localhost:56025/Resource/RemoveUserRole?resourceId=361b1ea6-1358-48ca-a015-c6e96ab7c65f&userId=1&roleId=1

To Check whether user can perform an action on resource: http://localhost:56025/Resource/IsAuthorized?resourceId=<resourceId>&userId=<userId>&actionType=<actionType>
Ex: http://localhost:56025/Resource/IsAuthorized?resourceId=361b1ea6-1358-48ca-a015-c6e96ab7c65f&userId=1&actionType=Read


Assumptions and Notes:

1. The user has different roles on different resources. I have built this RBAC somewhat similar to RBAC in SQL Server in which the role allocation is done on a resource level.
2. For easy testing purpose, I haven't used HTTP Put in the controller methods in the Resource Controller and I have directly passed the parameters in the query string instead of passing them in request body.
3. There is no database, so I have utilized Http Context Session variable to store the values of resources, users and roles.
4. There is some code duplicacy in the Resource controller in terms of input validation which can be removed but I kept it for now because the inputs for the methods are slightly different.
5. There is no view for the resource controller. We need to test the methods by entering the url in the browser itself.
6. We are not supporting adding new users and roles right now.