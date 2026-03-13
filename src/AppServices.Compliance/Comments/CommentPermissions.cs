using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Core.BaseEntities;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Comments;

public static class CommentPermissions
{
    extension(ClaimsPrincipal user)
    {
        public bool CanAddComment(IIsDeleted item) => !item.IsDeleted && user.IsStaff();

        public bool CanDeleteComment<T>(T item) where T : IIsDeleted, IHasOwner =>
            !item.IsDeleted && (user.IsManager() || user.IsOwner(item));
    }
}
