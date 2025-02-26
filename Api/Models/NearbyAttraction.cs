using GpsUtil.Location;

namespace TourGuide.Models;

public class NearbyAttraction
{
    public required string AttractionName { get; set; }
    public required Locations AttractionLocation { get; set; }
    public required Locations UserLocation { get; set; }
    public required double Distance { get; set; }
    public required int RewardPoints { get; set; }
}
