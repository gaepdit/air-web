using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.MemRepository.Repositories;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;

namespace AirWeb.MemRepository.ComplianceRepositories;

public class CaseFileCommentMemRepository()
    : CommentMemRepository<CaseFile, CaseFileComment>(CaseFileData.GetData), ICaseFileCommentRepository;

public class ComplianceWorkCommentMemRepository()
    : CommentMemRepository<ComplianceWork, ComplianceWorkComment>(ComplianceWorkData.GetData),
        IComplianceWorkCommentRepository;

public class FceCommentMemRepository()
    : CommentMemRepository<Fce, FceComment>(FceData.GetData), IFceCommentRepository;
