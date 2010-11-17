using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderUrlBuilder: IUriBuilder
    {
        private const string SERVICENAME = "reader";

        /// <summary>
        /// Base url for API actions.
        /// </summary>
        private const string ApiUrl = "https://www.google.com/reader/api/0/";

        /// <summary>
        /// State path.
        /// </summary>
        private const string ContentsPath = "stream/contents/";

        /// <summary>
        /// Feed url to be combined with the desired feed.
        /// </summary>
        private const string FeedUrl = ApiUrl + ContentsPath + "feed/";

        /// <summary>
        /// State path.
        /// </summary>
        private const string StatePath = ContentsPath + "user/-/state/com.google/";

        /// <summary>
        /// State url to be combined with desired state. For example: starred
        /// </summary>
        private const string StateUrl = ApiUrl + StatePath;

        /// <summary>
        /// Label path.
        /// </summary>
        private const string LabelPath = ContentsPath + "user/-/label/";

        /// <summary>
        /// Label url to be combined with the desired label.
        /// </summary>
        private const string LabelUrl = ApiUrl + LabelPath;

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
        
        private string _clientName;

        public GoogleReaderUrlBuilder(string clientName)
        {
            _clientName = clientName;
        }

        #region IUriBuilder Members

        public Uri BuildUri(UrlType type, StateType state, ReaderParameters parameters)
        {
            string stateString = "";
            switch (state)
            {
                case StateType.ReadingList:
                    stateString = "reading-list";
                    break;
                case StateType.Starred:
                    stateString = "starred";
                    break;
                case StateType.Shared:
                    stateString = "broadcast";
                    break;
                case StateType.Like:
                    stateString = "like";
                    break;
                case StateType.Read:
                    stateString = "read";
                    break;
                case StateType.KeptUnread:
                    stateString = "tracking-kept-unread";
                    break;
                default:
                    stateString = "";
                    break;
            }
            return MakeUri(type, stateString, parameters);
        }

        public Uri BuildUri(UrlType type, string itemName, ReaderParameters parameters)
        {
            return MakeUri(type, itemName, parameters);
        }

        public Uri GetLoginUri()
        {
            return new Uri(clientLoginUrl);
        }

        public string GetLoginData(string Username, string Password)
        {
            return string.Format(postData, SERVICENAME, Username, Password, _clientName);
        }


        private Uri MakeUri(UrlType type, string item, ReaderParameters parameters)
        {
            parameters.Client = _clientName;
            string queryString = "?" + parameters.MakeQueryString();

            switch (type)
            {
                case UrlType.Feed:
                    return new Uri(FeedUrl + item + queryString, UriKind.Absolute);
                case UrlType.State:
                    return new Uri(StateUrl + item + queryString, UriKind.Absolute);
                case UrlType.Tag:
                    return new Uri(LabelUrl + item + queryString, UriKind.Absolute);
                default:
                    return new Uri("");
            }
        }

        #endregion
    }
}
