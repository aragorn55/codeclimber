using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderUrlBuilder: IUriBuilder
    {
        /// <summary>
        /// Base url for API actions.
        /// </summary>
        public const string ApiUrl = "https://www.google.com/reader/api/0/";

        /// <summary>
        /// State path.
        /// </summary>
        public const string ContentsPath = "stream/contents/";

        /// <summary>
        /// Feed url to be combined with the desired feed.
        /// </summary>
        public const string FeedUrl = ApiUrl + ContentsPath + "feed/";

        /// <summary>
        /// State path.
        /// </summary>
        public const string StatePath = ContentsPath + "user/-/state/com.google/";

        /// <summary>
        /// State url to be combined with desired state. For example: starred
        /// </summary>
        public const string StateUrl = ApiUrl + StatePath;

        /// <summary>
        /// Label path.
        /// </summary>
        public const string LabelPath = ContentsPath + "user/-/label/";

        /// <summary>
        /// Label url to be combined with the desired label.
        /// </summary>
        public const string LabelUrl = ApiUrl + LabelPath;

        /// <summary>
        /// Client login url where we'll post login data to.
        /// </summary>
        private static string clientLoginUrl =
            @"https://www.google.com/accounts/ClientLogin";

        /// <summary>
        /// Data to be sent with the post request.
        /// </summary>
        private static string postData =
            @"service={0}&Email={1}&Passwd={2}&source={3}&continue=http://www.google.com/";

        #region IUriBuilder Members

        public Uri BuildUri(UrlType type, string url)
        {
            switch (type)
            {
                case UrlType.Feed:
                    return new Uri(FeedUrl + url, UriKind.Absolute);
                default:
                    return new Uri("");
            }
        }

        #endregion
    }
}
