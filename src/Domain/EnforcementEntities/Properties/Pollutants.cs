﻿namespace AirWeb.Domain.EnforcementEntities.Properties;

// Source of data from IAIP airbranch DB:
//
// ```sql
// select i.ICIS_POLLUTANT_CODE as Code,
//        iif(l.STRPOLLUTANTDESCRIPTION is null, i.ICIS_POLLUTANT_DESC, l.STRPOLLUTANTDESCRIPTION) as Description
// from dbo.LK_ICIS_POLLUTANT i
//     left join dbo.LOOKUPPOLLUTANTS l
//     on i.LGCY_POLLUTANT_CODE = l.STRPOLLUTANTCODE
// where ICIS_POLLUTANT_CODE is not null
// ```

public static partial class Data
{
    public static List<Pollutant> Pollutants { get; } =
    [
        new("300000331", "Chromium"),
        new("300000025", "Cadmium"),
        new("300000333", "Acetaldehyde "),
        new("300000334", "Trimethylamine"),
        new("300000335", "Nitro-o-phenylenediamine"),
        new("300000336", "1-Hydroxyacetone"),
        new("300000337", "Dibenzo-p-dioxin"),
        new("300000338", "1,3-Dimethylnaphthalene"),
        new("300000339", "1,6-Dimethylnaphthalene"),
        new("300000340", "2,7-Dimethylnaphthalene"),
        new("300000341", "2-Propanamine, N,N-dimethyl-"),
        new("300000342", "Benzene-d6"),
        new("300000343", "Hydrobromic acid"),
        new("300000344", "Ethylbenzene-d5"),
        new("300000345", "2,3,7,8-Tetrachlorodibenzofuran"),
        new("300000346", "Benzo[a]pyrene "),
        new("300000347", "Cyanide"),
        new("300000348", "Formic acid"),
        new("300000349", "Isopropanol"),
        new("300000350", "Methyl mercaptan"),
        new("300000351", "Phenanthrene"),
        new("300000352", "Fluorene"),
        new("300000353", "1-Methylnaphthalene"),
        new("300000354", "2-Methylnaphthalene"),
        new("300000355", "o-Dichlorobenzene"),
        new("300000356", "1,3-Dichloro-2-propanol"),
        new("300000357", "Methyl acrylate"),
        new("300000358", "Propylene glycol 1-methyl ether"),
        new("300000359", "Bromobenzene"),
        new("300000360", "Dimethyl ether"),
        new("300000361", "Diphenylamine"),
        new("300000362", "Pyrene"),
        new("300000364", "Naled"),
        new("300000365", "N-Methyl-2-pyrrolidone"),
        new("300000366", "Chromium[VI] trioxide"),
        new("300000367", "Silver"),
        new("300000368", "Cobalt"),
        new("300000369", "Vanadium"),
        new("300000370", "Phosphoric acid"),
        new("300000371", "Bromine"),
        new("300000372", "Nitrogen"),
        new("300000373", "Gasoline, natural"),
        new("300000374", "Chlorine dioxide"),
        new("300000375", "1,2,3,4,6,7,8-Heptachlorodibenzo-p-dioxin"),
        new("300000165", "1,1-Dichloroethylene"),
        new("300000129", "1, 2, 4 - Trimethylebenzene AKA - Pseudoc"),
        new("300000133", "2,4-D"),
        new("300000050", "Xylenol"),
        new("300000204", "2-Acetylaminofluorene"),
        new("300000259", "1-Dimethylamino-2-propanol"),
        new("300000363", "Butyl acrylate"),
        new("300000151", "Triethylamine"),
        new("300000261", "Trans-Crotonaldehyde"),
        new("300000040", "Cis-Crotonaldehyde"),
        new("300000281", "Hydrazine Monohydrate"),
        new("300000047", "Asbestos"),
        new("300000173", "Acetaldehyde"),
        new("300000123", "Acetophenone"),
        new("300000187", "Acetone"),
        new("300000245", "Acetamide"),
        new("300000172", "Acetonitrile"),
        new("300000292", "Acetylenes (Alkynes)"),
        new("300000149", "Acrylic acid"),
        new("300000150", "Acrylamide"),
        new("300000107", "Acrolein"),
        new("300000104", "Acrylonitrile"),
        new("300000328", "Administration"),
        new("300000214", "Silver Compounds"),
        new("300000039", "Aluminum (TSP)"),
        new("300000293", "Aluminum Compounds"),
        new("300000294", "Aldehydes"),
        new("300000193", "Aniline"),
        new("300000252", "o-Anisidine"),
        new("300000030", "Antimony"),
        new("300000295", "Aromatics"),
        new("300000215", "ARSENIC COMPOUNDS"),
        new("300000028", "Arsenic"),
        new("300000215", "Arsenic Compounds"),
        new("300000027", "Barium"),
        new("300000217", "Barium Compounds"),
        new("300000091", "Propoxur"),
        new("300000092", "Bis(2-chloroethyl) ether"),
        new("300000026", "Beryllium"),
        new("300000296", "Beryllium Compounds"),
        new("300000118", "Benzyl chloride"),
        new("300000134", "Benzidine"),
        new("300000297", "BERYLLIUM COMPOUNDS (E649947)"),
        new("300000255", "Benzotrichloride"),
        new("300000135", "Biphenyl"),
        new("300000167", "Tribromomethane"),
        new("300000209", "Benzene, ethylbenzene, toluene, xylene combination"),
        new("300000108", "1,3-Butadiene"),
        new("300000183", "Benzene"),
        new("300000332", "Perfluoroethane"),
        new("300000064", "Perfluorobutane"),
        new("300000063", "Perfluorohexane"),
        new("300000286", "5-ETHYLIDENE-2-NORBORNENE"),
        new("300000148", "Chloroacetic acid"),
        new("300000260", "Catechol"),
        new("300000067", "Calcium cyanamide"),
        new("300000218", "CADMIUM COMPOUNDS"),
        new("300000169", "Carbon disulfide"),
        new("300000114", "Caprolactam"),
        new("300000071", "Captan"),
        new("300000190", "Carbaryl"),
        new("300000203", "Carbon tetrachloride"),
        new("300000025", "Cadmium"),
        new("300000218", "Cadmium Compounds"),
        new("300000220", "Coke Oven Emissions"),
        new("300000158", "Tetrafluoromethane"),
        new("300000240", "Chlorofluorocarbons (CFC's)"),
        new("300000180", "Methane"),
        new("300000268", "2-Chloroacetophenone "),
        new("300000059", "Chlorobenzilate"),
        new("300000096", "Chlorobenzene"),
        new("300000075", "Chloroprene"),
        new("10339", "Chlorodifluoromethane"),
        new("10343", "Bis(chloromethyl) ether"),
        new("300000070", "Chloramben"),
        new("300000186", "Chloroform"),
        new("300000198", "Chlordane"),
        new("300000106", "Allyl chloride"),
        new("300000210", "Chromium compounds"),
        new("300000011", "Chlorine"),
        new("300000042", "Chlorinated Dioxin"),
        new("300000298", "Chlorinated Dioxin And Furans 2, 3, 7, 8 Congeners Only (TEQ)"),
        new("300000175", "Chloroethane"),
        new("300000299", "Chlorophenols"),
        new("300000068", "Cyanide Compounds"),
        new("10193", "Carbon Monoxide"),
        new("300000076", "Carbon dioxide"),
        new("300000213", "Carbon dioxide equivalent"),
        new("300000221", "COLBALT & COBALT COMPOUNDS"),
        new("300000222", "Cobalt Compounds"),
        new("300000300", "COKE OVEN COMPOUNDS (E649830)"),
        new("300000060", "Carbonyl sulfide"),
        new("300000002", "Chromium VI"),
        new("300000210", "Chromium Compounds"),
        new("300000049", "Cresol"),
        new("300000098", "m-Cresol"),
        new("300000131", "o-Cresol"),
        new("300000112", "p-Cresol"),
        new("300000024", "Copper (TSP)"),
        new("300000223", "Copper Compounds"),
        new("300000117", "4,4'-Methylenebis(2-chloroaniline)"),
        new("300000126", "1,2-Dibromo-3-chloropropane"),
        new("300000072", "Dibenzofuran"),
        new("300000111", "p-Dichlorobenzene"),
        new("300000136", "3,3'-Dichlorobenzidine"),
        new("300000166", "1,1-Dichloroethane"),
        new("300000154", "1,2-Dichloropropane"),
        new("300000057", "1,3-Dichloropropene"),
        new("300000279", "DICHLORODIPHENYLDICHLOROETHYLENE"),
        new("300000192", "Dichlorvos"),
        new("300000089", "Di(2-ethylhexyl) phthalate"),
        new("300000246", "Sulfuric acid, diethyl ester"),
        new("300000263", "Diazomethane"),
        new("300000093", "Diethanolamine"),
        new("300000081", "N,N-Dimethylaniline"),
        new("300000195", "4-Dimethylaminoazobenzene"),
        new("300000087", "3,3'-Dimethylbenzidine"),
        new("300000116", "4,4'-Methylenedi(phenyl isocyanate)"),
        new("300000184", "N,N-Dimethylformamide"),
        new("300000200", "1,1-Dimethylhydrazine"),
        new("300000191", "N-Nitrosodimethylamine"),
        new("300000073", "Dimethyl phthalate"),
        new("300000250", "Dimethyl Sulfate"),
        new("300000251", "n,n-Dimethylcarbamoyl Chloride"),
        new("300000087", "3,3'-Dimethoxybenzidine"),
        new("300000143", "Dibutyl phthalate"),
        new("300000206", "2,4-Dinitrophenol"),
        new("300000083", "2,4-Dinitrotoluene"),
        new("300000058", "4, 6 - Dinitro-O-Cresol Including Salts"),
        new("300000128", "2,4-Toluenediamine"),
        new("300000077", "1,4-Dioxane"),
        new("300000120", "Ethylbenzene AKA-Phenylethane"),
        new("300000110", "Epichlorohydrin"),
        new("300000109", "Ethylene dibromide"),
        new("300000105", "1,2-Dichloroethane"),
        new("300000168", "Ethylene Oxide"),
        new("300000257", "1,2-Epoxybutane"),
        new("300000069", "Ethyl acrylate"),
        new("300000103", "Ethylene glycol"),
        new("300000189", "Ethanol"),
        new("300000178", "Ethylene AKA-Ethane"),
        new("300000262", "Ethyleneimine"),
        new("300000125", "Ethylene thiourea"),
        new("300000329", "Facility Wide"),
        new("300000301", "Fugitive Dust"),
        new("300000302", "Fugitive Emissions"),
        new("300000004", "Fluorides"),
        new("300000303", "Fine Mineral Fibers"),
        new("300000207", "Formaldehyde"),
        new("300000208", "Furan"),
        new("300000201", "Glycerine"),
        new("300000304", "Glycol Ethers"),
        new("300000046", "Hydrogen"),
        new("300000010", "Hydrogen Sulfide"),
        new("300000305", "Total Hydrocarbons"),
        new("300000141", "Hexachlorobutadiene"),
        new("300000153", "Methyl Ethyl Ketone"),
        new("300000074", "Tetrachloroethylene (Perchloroethylene)"),
        new("300000048", "Xylene (s)"),
        new("300000088", "Hexachlorobenzene"),
        new("300000197", "1, 2, 3, 4, 5, 6 - Hexachlorocyclohexane (AKA Lindane)"),
        new("300000156", "Hexachlorocyclopentadiene"),
        new("300000185", "Hexachloroethane"),
        new("300000018", "Hydrogen Chloride"),
        new("300000176", "Hydrogen cyanide"),
        new("300000079", "Hydroquinone"),
        new("300000080", "1,2-Diphenylhydrazine"),
        new("300000066", "Hydrazine"),
        new("300000017", "Hydrofluoric Acid"),
        new("300000283", "HAFNIUM CARBIDE"),
        new("300000265", "1,1,1,3,3,-pentaflurobutane"),
        new("300000051", "HFC-236ea (1,1,1,2,3,3-hexafluoropropane)"),
        new("300000266", "1,1,1,2,3-Pentafluoropropane"),
        new("300000052", "HFC-236fa (1,1,1,3,3,3-hexafluoropropane)"),
        new("300000267", "1, 1, 1, 3, 3-Pentafluoropropane"),
        new("300000274", "1,1,2,2,3-Pentafluoropropane"),
        new("300000287", "1,1,2,3,3-pentafluoropropane"),
        new("10255", "HFC-134a"),
        new("300000160", "HFC-23 (Trifluoromethane)"),
        new("300000062", "HFC-143a"),
        new("300000170", "HFC-32"),
        new("300000239", "HFC-134a"),
        new("300000270", "Methyl Flouride"),
        new("300000164", "HFC-152a"),
        new("300000264", "HFC-161"),
        new("300000065", "HFC-125"),
        new("300000000", "HFC-4310mee (1,1,1,2,2,3,4,5,5,5-decafluoropentane)"),
        new("300000061", "HFC-227ea (1,1,1,2,3,3,3-heptafluoropropane)"),
        new("300000285", "HAFNIUM CHLORIDE OXIDE HFCL2O"),
        new("300000284", "HAFNIUM CHLORIDE HFCL4"),
        new("300000306", "HYDROFLUOROCARBONS"),
        new("300000036", "Mercury"),
        new("300000307", "Mercury Compounds"),
        new("300000053", "Hexamethylphosphoramide"),
        new("300000014", "Nitric Acid"),
        new("300000157", "Heptachlor"),
        new("300000015", "Sulfuric Acid"),
        new("300000277", "HEXAMETHYLENE DIISOCYANATE"),
        new("300000094", "Hexane"),
        new("300000248", "2-Methylpropane (Isobutane)"),
        new("300000124", "Isopropylbenzene AKA-Cumene"),
        new("300000155", "Isophorone"),
        new("300000308", "Ketones"),
        new("300000224", "Lead compounds"),
        new("300000226", "MAGANESE COMPOUNDS (94 & 95 - OFF-SITE)"),
        new("300000171", "Methylene Chloride"),
        new("300000211", "MALEIC ANHYDRIDE"),
        new("300000115", "4,4'-Methylenedianiline"),
        new("300000179", "Methyl bromide"),
        new("300000258", "Methyl Chloromethyl Ether"),
        new("300000177", "Chloromethane"),
        new("300000272", "Methyl Isocyanate"),
        new("300000309", "MERCURY COMPOUNDS (E650028)"),
        new("300000291", "METAL HAP"),
        new("300000145", "Methyl methacrylate"),
        new("300000194", "Methyl hydrazine"),
        new("300000037", "Manganese"),
        new("300000226", "Manganese Compounds"),
        new("300000275", "N-NITROSO-N-METHYLUREA"),
        new("300000101", "Methyl isobutyl ketone"),
        new("300000237", "Methyl tert-butyl ether"),
        new("300000247", "Iodomethane"),
        new("300000188", "Methanol"),
        new("300000181", "Methoxychlor"),
        new("300000099", "M-Xylene AKA 1, 3 - Dimethylbenzene"),
        new("300000007", "NITROGEN OXIDES N2O ETC"),
        new("300000138", "Naphthalene"),
        new("300000122", "Nitrobenzene"),
        new("300000253", "4-Nitrodiphenyl "),
        new("300000280", "NITROGEN TRIFLUORIDE"),
        new("300000016", "Ammonia"),
        new("300000035", "Nickel Powder"),
        new("300000035", "Nickel"),
        new("300000228", "Nickel Compounds"),
        new("300000228", "Nickel compounds"),
        new("300000146", "2-Nitropropane"),
        new("300000196", "N-Nitrosomorpholine"),
        new("300000006", "Nitric Oxide"),
        new("300000005", "Nitrogen Dioxide"),
        new("300000282", "NITROGEN OXIDES"),
        new("300000310", "Non-Volatile Organic Compounds"),
        new("300000311", "Organic Acids"),
        new("300000312", "Odors"),
        new("300000313", "OLEFINS"),
        new("300000330", "Other Emissions Other than road based"),
        new("300000132", "O-Xylene AKA - 1, 2-Dimethylbenzene"),
        new("300000013", "Phosphorus"),
        new("300000314", "Fine Particulates: High Probability of Violation"),
        new("300000315", "Fine Particulates: Low Probability of Violation"),
        new("300000269", "2,2,4-Trimethylpentane"),
        new("300000085", "Anthracene"),
        new("300000316", "Paraffins (Alkanes)"),
        new("300000202", "Parathion"),
        new("300000038", "Lead"),
        new("300000317", "Polybrom. Biphenyls"),
        new("300000318", "Lead Compounds"),
        new("300000045", "Polychlorinated biphenyls"),
        new("300000144", "Pentachloronitrobenzene"),
        new("300000140", "Pentachlorophenol"),
        new("300000100", "B3 PHENYLENEDIAMINE"),
        new("300000288", "PERFLUOROCARBONS"),
        new("300000199", "Propylene glycol"),
        new("300000095", "Phenol"),
        new("300000009", "Phosphine"),
        new("300000163", "Phosgene"),
        new("300000244", "beta-Propiolactone"),
        new("300000319", "Particulate Matter < 10 um"),
        new("300000320", "Particulate Matter < 2.5 um"),
        new("300000121", "p-Nitrophenol"),
        new("300000321", "Polycyclic Organic Matter"),
        new("300000327", "Primary PM10 (filterables and condensibles)"),
        new("300000326", "Primary PM2.5 (filterables and condensibles)"),
        new("300000256", "para-Quinone"),
        new("300000078", "Propanal"),
        new("300000249", "2-Methylaziridine (propyleneimine)"),
        new("300000278", "PROPANE SULTONE"),
        new("300000159", "Propylene oxide"),
        new("300000090", "Propylene"),
        new("300000322", "Total Particulate Matter"),
        new("300000142", "Phthalic anhydride"),
        new("300000323", "Pollutant X"),
        new("300000113", "P-Xylene AKA 1, 4 - Dimethlybenzene"),
        new("300000137", "Quinoline"),
        new("300000229", "Radionuclides (including radon)"),
        new("300000230", "Radionuclides (including Radon)"),
        new("300000231", "Reactive Organic Compounds"),
        new("300000324", "Reduced Silver Compounds"),
        new("300000030", "Antimony (TSP)"),
        new("300000232", "Antimony Compounds"),
        new("300000012", "Selenium Compounds"),
        new("300000041", "Sulfur hexafluoride"),
        new("10461", "Sulfur Dioxide"),
        new("300000020", "Sulfur Trioxide"),
        new("300000233", "Sulfates"),
        new("300000254", "Styrene Oxide"),
        new("300000119", "Styrene AKA-Ethenyl benzene"),
        new("300000212", "Semi-volatile organic compounds"),
        new("300000054", "Toluene-2,4-diisocyanate"),
        new("300000084", "1,2,4-Trichlorobenzene"),
        new("300000127", "2,4,5-Trichlorophenol"),
        new("300000139", "2,4,6-Trichlorophenol"),
        new("300000182", "1,1,1 - Trichloroethane"),
        new("300000001", "Tetrachlorodibenzofuran, 2, 3, 7, 8"),
        new("300000147", "1,1,2,2-Tetrachloroethane"),
        new("300000152", "1,1,2-Trichloroethane"),
        new("300000082", "Triethylamine"),
        new("300000242", "Total HAP"),
        new("300000033", "Thallium"),
        new("300000031", "Titanium (TSP)"),
        new("300000032", "Tin"),
        new("300000019", "Titanium tetrachloride"),
        new("300000234", "Total Non-Methane Organic Compounds"),
        new("300000097", "Toluene AKA - Methylbenzene"),
        new("300000130", "o-Toluidine"),
        new("300000008", "Toxaphene"),
        new("300000044", "Trifluralin"),
        new("300000003", "Total Reduced Sulfur Compounds"),
        new("300000289", "Total Suspended Particulate (physical property)"),
        new("300000205", "Urethane"),
        new("300000174", "Vinyl Chloride"),
        new("300000236", "Visible Emissions"),
        new("300000290", "VOLATILE ORGANIC HAZARDOUS AIR POLLUTANT"),
        new("300000243", "Volatile Organic Compounds"),
        new("300000102", "Vinyl acetate"),
        new("300000271", "Vinyl Bromide"),
        new("300000023", "Zinc"),
        new("300000238", "Zinc Compounds"),
    ];
}
