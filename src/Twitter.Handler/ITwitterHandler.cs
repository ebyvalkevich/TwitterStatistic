namespace TwitterHandler;

public interface ITwitterHandler
{
    TrackTwitterModel GetStatistics();
    List<string> GetTopHashtags();
}