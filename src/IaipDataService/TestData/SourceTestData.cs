using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;
using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;

namespace IaipDataService.TestData;

public static class SourceTestData
{
    private const string ReportStatement = "The following test has been reviewed and was conducted " +
                                           "in an acceptable fashion for the purpose intended.";

    private const string EpdDirector = "A. Director";

    private static IEnumerable<BaseSourceTestReport> StackTestReportSeedItems =>
    [
        new SourceTestReportOneStack
        {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 201100541,
            Facility = new FacilitySummary(FacilityData.GetFacility((FacilityId)"00100001")),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            IaipComplianceComplete = true,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff =
            [
                StaffData.GetRandomStaff().Name,
                StaffData.GetRandomStaff().Name,
            ],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = [new ValueWithUnits("0.018", "lb/ton")],
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns =
            [
                new StackTestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAcfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },

                new StackTestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAcfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "0000000",
                },

                new StackTestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAcfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                    ConfidentialParametersCode = "0101010",
                }
            ],
            AvgPollutantConcentration = new ValueWithUnits("17.1", "ppm"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "1B000000000000000000000001100000000000011000001100000110101",
        },
        new SourceTestReportOneStack
        {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 202001297,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            IaipComplianceComplete = true,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = [new ValueWithUnits("0.018", "lb/ton")],
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns =
            [
                new StackTestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAcfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },

                new StackTestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAcfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "",
                },

                new StackTestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAcfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                    ConfidentialParametersCode = "",
                }
            ],
            AvgPollutantConcentration = new ValueWithUnits("17.1", "µg/m3"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new SourceTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackStandard,
            ReferenceNumber = 201600525,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2016, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2016, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2016, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            IaipComplianceComplete = true,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates =
            [
                new ValueWithUnits("1", "lb/TON"),
                new ValueWithUnits("20", "LB/HR"),
            ],
            ControlEquipmentInfo = TextData.None,
            StackOneName = "Inner",
            StackTwoName = "Outer",
            TestRuns =
            [
                new TwoStackTestRun
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },

                new TwoStackTestRun
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },

                new TwoStackTestRun
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                }
            ],
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03", "GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003", "GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00", "LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00", "LB/HR"),
            SumAvgEmissionRate = new ValueWithUnits("7.00", "LB/HR"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new SourceTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackDre,
            ReferenceNumber = 200400473,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2016, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2016, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2016, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates =
            [
                new ValueWithUnits("1", "lb/TON"),
                new ValueWithUnits("20", "LB/HR"),
            ],
            ControlEquipmentInfo = TextData.None,
            StackOneName = "Inlet",
            StackTwoName = "Outlet",
            TestRuns =
            [
                new TwoStackTestRun
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },

                new TwoStackTestRun
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },

                new TwoStackTestRun
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                }
            ],
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03", "GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003", "GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00", "LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00", "LB/HR"),
            DestructionEfficiency = "75.0",
            ConfidentialParametersCode = "0",
        },
        new SourceTestReportLoadingRack
        {
            DocumentType = DocumentType.LoadingRack,
            ReferenceNumber = 201901149,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Volatile Organic Compounds",
            Source = "Tank Truck Loading Rack",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.ShortMultiline,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("400,000,000", "GPY"),
            OperatingCapacity = new ValueWithUnits("90,000", "GPY"),
            AllowableEmissionRates = [new ValueWithUnits("18", "mg/L")],
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestDuration = new ValueWithUnits("6", "Hours"),
            PollutantConcentrationIn = new ValueWithUnits("20", "%"),
            PollutantConcentrationOut = new ValueWithUnits("120", "PPM"),
            EmissionRate = new ValueWithUnits("9.9", "mg/L"),
            DestructionReduction = new ValueWithUnits("98.2", "%"),
            ConfidentialParametersCode = "1F000000000000000000000001000000010001",
        },
        new SourceTestReportPondTreatment
        {
            DocumentType = DocumentType.PondTreatment,
            ReferenceNumber = 200400023,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Methanol",
            Source = "Pond",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns =
            [
                new PondTreatmentTestRun
                {
                    RunNumber = "1",
                    PollutantCollectionRate = "7",
                    TreatmentRate = "7.0",
                    ConfidentialParametersCode = "",
                },

                new PondTreatmentTestRun
                {
                    RunNumber = "2",
                    PollutantCollectionRate = "7.4",
                    TreatmentRate = "37",
                    ConfidentialParametersCode = "",
                },

                new PondTreatmentTestRun
                {
                    RunNumber = "3",
                    PollutantCollectionRate = "7.8",
                    TreatmentRate = "39",
                    ConfidentialParametersCode = "",
                }
            ],
            AvgPollutantCollectionRate = new ValueWithUnits("7.4", "lb/ODTP"),
            AvgTreatmentRate = new ValueWithUnits("7.2", "lb/ODTP"),
            DestructionEfficiency = "97.3",
            ConfidentialParametersCode = "",
        },
        new SourceTestReportGasConcentration
        {
            DocumentType = DocumentType.GasConcentration,
            ReferenceNumber = 200400009,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Nitrogen Oxides",
            Source = "Combustion Turbine",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Short,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            AllowableEmissionRates = [new ValueWithUnits("25", "PPM @ 15% O2")],
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns =
            [
                new GasConcentrationTestRun
                {
                    RunNumber = "1",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "",
                },

                new GasConcentrationTestRun
                {
                    RunNumber = "2",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "",
                },

                new GasConcentrationTestRun
                {
                    RunNumber = "3",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "",
                }
            ],
            AvgPollutantConcentration = new ValueWithUnits("25", "PPM"),
            AvgEmissionRate = new ValueWithUnits("22", "PPM @ 15% O2"),
            PercentAllowable = "90",
            ConfidentialParametersCode = "",
        },
        new SourceTestReportFlare
        {
            DocumentType = DocumentType.Flare,
            ReferenceNumber = 200400407,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Volatile Organic Compounds",
            Source = "Flare",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            MaxOperatingCapacity = new ValueWithUnits("100", "%"),
            OperatingCapacity = new ValueWithUnits("100", "%"),
            AllowableEmissionRates =
            [
                new ValueWithUnits("80", "ft/sec", "Velocity less than"),
                new ValueWithUnits("200", "BTU/scf", "Heat Content greater than or equal to"),
            ],
            ControlEquipmentInfo = TextData.Short,
            TestRuns =
            [
                new FlareTestRun
                {
                    RunNumber = "1",
                    HeatingValue = "400",
                    EmissionRateVelocity = "35",
                    ConfidentialParametersCode = "",
                },

                new FlareTestRun
                {
                    RunNumber = "2",
                    HeatingValue = "450",
                    EmissionRateVelocity = "37",
                    ConfidentialParametersCode = "",
                },

                new FlareTestRun
                {
                    RunNumber = "3",
                    HeatingValue = "425",
                    EmissionRateVelocity = "39",
                    ConfidentialParametersCode = "",
                }
            ],
            AvgHeatingValue = new ValueWithUnits("425", "BTU/scf"),
            AvgEmissionRateVelocity = new ValueWithUnits("37", "ft/sec"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "",
        },
        new SourceTestReportRata
        {
            DocumentType = DocumentType.Rata,
            ReferenceNumber = 201200095,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Nitrogen Oxides",
            Source = "Boiler",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            ApplicableStandard = TextData.Short,
            Diluent = "Oxygen",
            Units = "LB/MMBTU",
            RelativeAccuracyCode = "AppStandard",
            RelativeAccuracyPercent = "5.5",
            RelativeAccuracyRequiredPercent = "10",
            RelativeAccuracyRequiredLabel = "% of the applicable standard (when the average of " +
                                            "the RM test data is less than 50% of the applicable standard).",
            TestRuns =
            [
                new RataTestRun
                {
                    RunNumber = "1",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },

                new RataTestRun
                {
                    RunNumber = "2",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = true,
                    ConfidentialParametersCode = "",
                },

                new RataTestRun
                {
                    RunNumber = "3",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },

                new RataTestRun
                {
                    RunNumber = "4",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },

                new RataTestRun
                {
                    RunNumber = "5",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },

                new RataTestRun
                {
                    RunNumber = "6",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = true,
                    ConfidentialParametersCode = "",
                }
            ],
            ConfidentialParametersCode = "",
        },
        new SourceTestMemorandum
        {
            DocumentType = DocumentType.MemorandumStandard,
            ReferenceNumber = 200600289,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Methanol",
            Source = "System",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            Comments = TextData.LongMultiline,
            ConfidentialParametersCode = "",
        },
        new SourceTestMemorandum
        {
            DocumentType = DocumentType.MemorandumToFile,
            ReferenceNumber = 201500570,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Opacity",
            Source = "Monitor",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            Comments = TextData.LongMultiline,
            MonitorManufacturer = TextData.Short,
            MonitorSerialNumber = TextData.VeryShort,
            ConfidentialParametersCode = "",
        },
        new SourceTestMemorandum
        {
            DocumentType = DocumentType.PTE,
            ReferenceNumber = 200400476,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "VOC",
            Source = "System",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            Comments = TextData.LongMultiline,
            MaxOperatingCapacity = new ValueWithUnits("100000", "Units"),
            OperatingCapacity = new ValueWithUnits("50000", "Units"),
            AllowableEmissionRates = [new ValueWithUnits("100", "%")],
            ControlEquipmentInfo = TextData.Long,
            ConfidentialParametersCode = "",
        },
        new SourceTestReportOpacity
        {
            DocumentType = DocumentType.Method9Multi,
            ReferenceNumber = 201801068,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Opacity",
            Source = "Kiln",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.VeryShort,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            ControlEquipmentInfo = TextData.ShortMultiline,
            OpacityStandard = "Highest 6-minute average",
            MaxOperatingCapacityUnits = "Tons/Day",
            OperatingCapacityUnits = "Tons/Day",
            AllowableEmissionRateUnits = "% Opacity",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "230",
                    AllowableEmissionRate = "40",
                    Opacity = "15",
                    EquipmentItem = TextData.Short,
                    ConfidentialParametersCode = "",
                },
                new OpacityTestRun
                {
                    RunNumber = "2",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "200",
                    AllowableEmissionRate = "40",
                    Opacity = "12",
                    EquipmentItem = TextData.Short,
                    ConfidentialParametersCode = "",
                },
                new OpacityTestRun
                {
                    RunNumber = "3",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "210",
                    AllowableEmissionRate = "40",
                    Opacity = "20",
                    EquipmentItem = TextData.Short,
                    ConfidentialParametersCode = "",
                },
                new OpacityTestRun
                {
                    RunNumber = "4",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "190",
                    AllowableEmissionRate = "40",
                    Opacity = "19",
                    EquipmentItem = TextData.Short,
                    ConfidentialParametersCode = "",
                },
                new OpacityTestRun
                {
                    RunNumber = "5",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "210",
                    AllowableEmissionRate = "40",
                    Opacity = "21",
                    EquipmentItem = TextData.Short,
                    ConfidentialParametersCode = "",
                },
            },
            ConfidentialParametersCode = "",
        },
        new SourceTestReportOpacity
        {
            DocumentType = DocumentType.Method22,
            ReferenceNumber = 200600052,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Opacity",
            Source = "Bin",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = ReportStatement,
            ReportClosed = true,
            ComplianceStatus = "Not In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            DateTestReviewComplete = new DateTime(2020, 11, 5, 0, 0, 0, DateTimeKind.Local),
            IaipComplianceAssignment = StaffData.GetRandomStaff().EmailAddress,
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            ControlEquipmentInfo = TextData.ShortMultiline,
            TestDuration = "60 minutes",
            MaxOperatingCapacityUnits = "Tons/HR",
            OperatingCapacityUnits = "Tons/HR",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "3",
                    OperatingCapacity = "3",
                    AllowableEmissionRate = "0 % Opacity",
                    AccumulatedEmissionTime = "20:00",
                    ConfidentialParametersCode = "",
                },
            },
            ConfidentialParametersCode = "",
        },
        new SourceTestReportOpacity
        {
            DocumentType = DocumentType.Method9Single,
            ReferenceNumber = 200700192,
            Facility = new FacilitySummary(FacilityData.GetRandomFacility()),
            Pollutant = "Opacity",
            Source = "Scrubber",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = ReportStatement,
            ReportClosed = false,
            ComplianceStatus = "In Compliance",
            TestDates = new DateRange(
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Local),
            ReviewedByStaff = StaffData.GetRandomStaff().Name,
            WitnessedByStaff = [],
            ComplianceManager = StaffData.GetRandomStaff().Name,
            TestingUnitManager = StaffData.GetRandomStaff().Name,
            EpdDirector = EpdDirector,

            ControlEquipmentInfo = TextData.ShortMultiline,
            OpacityStandard = "30-minute average",
            TestDuration = "180 minutes",
            MaxOperatingCapacityUnits = "UNITS",
            OperatingCapacityUnits = "UNITS",
            AllowableEmissionRateUnits = "%",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "10.1",
                    OperatingCapacity = "9.9",
                    AllowableEmissionRate = "40.0",
                    Opacity = "0.1",
                    ConfidentialParametersCode = "",
                },
            },
            ConfidentialParametersCode = "",
        },
    ];

    public static List<BaseSourceTestReport> GetData
    {
        get
        {
            if (field is not null) return field;
            field = StackTestReportSeedItems.ToList();
            return field;
        }
    }
}
