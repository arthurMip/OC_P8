using GpsUtil.Location;
using TourGuide.LibrairiesWrappers.Interfaces;

namespace TourGuide.LibrairiesWrappers;

public class GpsUtilWrapper : IGpsUtil
{
    private readonly GpsUtil.GpsUtil _gpsUtil;

    public GpsUtilWrapper()
    {
        _gpsUtil = new();
    }

    public Task<VisitedLocation> GetUserLocation(Guid userId)
    {
        return Task.Run(() => _gpsUtil.GetUserLocation(userId));
    }

    public List<Attraction> GetAttractions()
    {
        return _gpsUtil.GetAttractions();
    }
}
