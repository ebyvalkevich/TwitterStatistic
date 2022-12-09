using Microsoft.AspNetCore.Mvc;
using TwitterHandler;

namespace TwitterAPIStatistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private ITwitterHandler twitterHandler;

        public TwitterController(ITwitterHandler twitterHandler)
        {
            this.twitterHandler = twitterHandler;
        }

        [HttpGet("GetStatistics")]
        public TrackTwitterModel GetStatistics()
        {
            return this.twitterHandler.GetStatistics();
        }

        [HttpGet("GetTopHashtags")]
        public List<string> GetTopHashtags()
        {
            return this.twitterHandler.GetTopHashtags();
        }
    }
}
