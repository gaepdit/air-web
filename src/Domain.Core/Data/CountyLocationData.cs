namespace AirWeb.Domain.Core.Data;

public static class CountyLocationData
{
    public static List<NameLocation> GetAll { get; } = CountyLocations.OrderBy(nl => nl.Name).ToList();

    // ReSharper disable StringLiteralTypo
    private static IEnumerable<NameLocation> CountyLocations =>
    [
        new(Name: "Appling County", Location: "31.7176115,-82.4644557,11z"),
        new(Name: "Atkinson County", Location: "31.3017036,-82.8852805,11z"),
        new(Name: "Bacon County", Location: "31.563851,-82.4274291,11z"),
        new(Name: "Baker County", Location: "31.2665784,-84.3914024,11z"),
        new(Name: "Baldwin County", Location: "33.0582705,-83.2367086,11z"),
        new(Name: "Banks County", Location: "34.3446164,-83.503665,11z"),
        new(Name: "Barrow County", Location: "34.0110134,-83.7856506,12z"),
        new(Name: "Bartow County", Location: "34.2441785,-84.8456834,11z"),
        new(Name: "Ben Hill County", Location: "31.7517329,-83.4049051,11z"),
        new(Name: "Berrien County", Location: "31.2507251,-83.236239,11z"),
        new(Name: "Bibb County", Location: "32.8067175,-83.6907399,11z"),
        new(Name: "Bleckley County", Location: "32.428345,-83.31854,11z"),
        new(Name: "Brantley County", Location: "31.1920815,-82.0081276,11z"),
        new(Name: "Brooks County", Location: "30.8566819,-83.5263101,11z"),
        new(Name: "Bryan County", Location: "31.976985,-81.460018,10z"),
        new(Name: "Bulloch County", Location: "32.4030543,-81.896446,11z"),
        new(Name: "Burke County", Location: "33.0502456,-82.0939805,11z"),
        new(Name: "Butts County", Location: "33.3123347,-84.1303235,11z"),
        new(Name: "Calhoun County", Location: "31.535776,-84.6202915,11z"),
        new(Name: "Camden County", Location: "30.939046,-81.7854783,11z"),
        new(Name: "Candler County", Location: "32.4143157,-82.2514195,11z"),
        new(Name: "Carroll County", Location: "33.6189676,-85.0737055,11z"),
        new(Name: "Catoosa County", Location: "34.8776152,-85.204788,12z"),
        new(Name: "Charlton County", Location: "30.7157829,-82.15616,10z"),
        new(Name: "Chatham County", Location: "31.9709686,-81.4169698,10z"),
    ];
}
