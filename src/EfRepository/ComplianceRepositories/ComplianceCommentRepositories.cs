using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.CommonRepositories;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.ComplianceRepositories;

public class CaseFileCommentRepository(AppDbContext context)
    : CommentRepository<CaseFile, CaseFileComment, AppDbContext>(context), ICaseFileCommentRepository;

public class ComplianceWorkCommentRepository(AppDbContext context)
    : CommentRepository<ComplianceWork, ComplianceWorkComment, AppDbContext>(context), IComplianceWorkCommentRepository;

public class FceCommentRepository(AppDbContext context)
    : CommentRepository<Fce, FceComment, AppDbContext>(context), IFceCommentRepository;
