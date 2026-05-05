using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.MemRepository.CommonRepositories;

namespace AirWeb.MemRepository.ComplianceRepositories;

public class CaseFileCommentMemRepository(ICaseFileRepository repository)
    : CommentMemRepository<CaseFile, CaseFileComment>(repository), ICaseFileCommentRepository;

public class ComplianceWorkCommentMemRepository(IComplianceWorkRepository repository)
    : CommentMemRepository<ComplianceWork, ComplianceWorkComment>(repository),
        IComplianceWorkCommentRepository;

public class FceCommentMemRepository(IFceRepository repository)
    : CommentMemRepository<Fce, FceComment>(repository), IFceCommentRepository;
