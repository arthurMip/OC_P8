using GpsUtil.Location;
using Microsoft.AspNetCore.Mvc;
using TourGuide.LibrairiesWrappers.Interfaces;
using TourGuide.Models;
using TourGuide.Services.Interfaces;
using TourGuide.Users;
using TripPricer;

namespace TourGuide.Controllers;

[ApiController]
[Route("[controller]")]
public class TourGuideController : ControllerBase
{
    private readonly ITourGuideService _tourGuideService;
    private readonly IRewardsService _rewardsService;
    private readonly IRewardCentral _rewardCentral;

    public TourGuideController(
        ITourGuideService tourGuideService,
        IRewardsService rewardsService,
        IRewardCentral rewardCentral)
    {
        _tourGuideService = tourGuideService;
        _rewardsService = rewardsService;
        _rewardCentral = rewardCentral;
    }

    [HttpGet("getLocation")]
    public  ActionResult<VisitedLocation> GetLocation([FromQuery] string userName)
    {
        User? user = GetUser(userName);
        if (user == null)
        {
            return NotFound();
        }

        var location = _tourGuideService.GetUserLocation(user);
        return Ok(location);
    }

    // TODO: Change this method to no longer return a List of Attractions.
    // Instead: Get the closest five tourist attractions to the user - no matter how far away they are.
    // Return a new JSON object that contains:
    // Name of Tourist attraction, 
    // Tourist attractions lat/long, 
    // The user's location lat/long, 
    // The distance in miles between the user's location and each of the attractions.
    // The reward points for visiting each Attraction.
    //    Note: Attraction reward points can be gathered from RewardsCentral
    [HttpGet("getNearbyAttractions")]
    public async Task<ActionResult<List<NearbyAttraction>>> GetNearbyAttractions([FromQuery] string userName)
    {
        User? user = GetUser(userName);
        if (user == null)
        {
            return NotFound();
        }

        var visitedLocation = await _tourGuideService.GetUserLocation(user);
        var attractions =  await _tourGuideService.GetNearByAttractions(visitedLocation);

        var result = attractions.Select(a =>
            new NearbyAttraction
            {
                AttractionName = a.AttractionName,
                AttractionLocation = a,
                UserLocation = visitedLocation.Location,
                Distance = _rewardsService.GetDistance(a, visitedLocation.Location),
                RewardPoints = _rewardCentral.GetAttractionRewardPoints(a.AttractionId, visitedLocation.UserId)
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("getRewards")]
    public ActionResult<List<UserReward>> GetRewards([FromQuery] string userName)
    {
        User? user = GetUser(userName);
        if (user == null)
        {
            return NotFound();
        }

        var rewards = _tourGuideService.GetUserRewards(user);
        return Ok(rewards);
    }

    [HttpGet("getTripDeals")]
    public ActionResult<List<Provider>> GetTripDeals([FromQuery] string userName)
    {
        User? user = GetUser(userName);
        if (user == null)
        {
            return NotFound();
        }

        var deals = _tourGuideService.GetTripDeals(user);
        return Ok(deals);
    }

    private User? GetUser(string userName)
    {
        return _tourGuideService.GetUser(userName);
    }
}
