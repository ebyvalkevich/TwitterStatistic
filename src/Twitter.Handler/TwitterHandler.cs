using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

namespace TwitterHandler
{
    public class TwitterHandler : BackgroundService, ITwitterHandler
    {
        private readonly string _bearerToken;
        private readonly string _url;
        private int _tweetsPerMinute;
        private int _countTweets;
        private int _minutes;
        private List<string> _hashtags;

        public TwitterHandler(string url, string bearerToken)
        {
            _url = url;
            _bearerToken = bearerToken;
            _tweetsPerMinute = 0;
            _countTweets = 0;
            _minutes = 1;
            _hashtags = new List<string>();
        }

        private async Task GetSampleStream()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            var stream = await client.GetStreamAsync(_url);
            using var streamReader = new StreamReader(stream);
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(_minutes);

            var tweetsPerMinute = 0;
            var _ = new Timer((e) =>
            {
                _tweetsPerMinute = tweetsPerMinute;
                tweetsPerMinute = 0;
            }, null, startTimeSpan, periodTimeSpan);

            while (!streamReader.EndOfStream)
            {
                var streamLine = await streamReader.ReadLineAsync();
                _countTweets++;
                tweetsPerMinute++;
                _hashtags.Add(streamLine);
            }
        }
        public TrackTwitterModel GetStatistics()
        {
            return new TrackTwitterModel()
            {
                CountTweets = _countTweets,
                TweetsPerMinute = _tweetsPerMinute
            };
        }

        public List<string> GetTopHashtags()
        {
            var lastTop = Enumerable.Reverse(_hashtags).Take(10).Reverse().ToList();
            return lastTop;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(GetSampleStream, stoppingToken);
        }
    }
}
