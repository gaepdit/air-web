﻿using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);
    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber, bool forceRefresh = false);
    Task<List<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId, bool forceRefresh = false);
}
