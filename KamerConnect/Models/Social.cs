namespace KamerConnect.Models;

    public class Social
    {
        public SocialType? Type { get; set; }
        public string? Url { get; set; }

        public Social(SocialType? type, string? url)
        {
            Type = type;
            Url = url;
        }
    }