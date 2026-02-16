using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Comments;

namespace AirWeb.AppServices.Comments;

public class CaseFileCommentService(ICaseFileCommentRepository repository, IUserService userService)
    : CommentService<CaseFileComment>(repository, userService), ICaseFileCommentService;

public interface IComplianceWorkCommentService : ICommentService;

public class ComplianceWorkCommentService(IComplianceWorkCommentRepository repository, IUserService userService)
    : CommentService<ComplianceWorkComment>(repository, userService), IComplianceWorkCommentService;

public interface IFceCommentService : ICommentService;

public class FceCommentService(IFceCommentRepository repository, IUserService userService)
    : CommentService<FceComment>(repository, userService), IFceCommentService;
