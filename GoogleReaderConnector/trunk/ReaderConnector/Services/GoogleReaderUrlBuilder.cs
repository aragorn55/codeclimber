using System;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderUrlBuilder: IUriBuilder
    {
        private const string SERVICENAME = "reader";
        private const string CONTINUE = "http://www.google.com/";

        /// <summary>
        /// Base url for API actions.
        /// </summary>
        private const string ApiUrl = "https://www.google.com/reader/api/0/";

        
        // Contents paths
        private const string ContentsPath = "stream/contents/";
        private const string FeedUrl = ApiUrl + ContentsPath + "feed/";


        // User path
        private const string UserPath = ContentsPath + "user/-/";

        private const string StatePath = UserPath + "state/com.google/";
        private const string StateUrl = ApiUrl + StatePath;

        private const string LabelPath = UserPath + "label/";
        private const string LabelUrl = ApiUrl + LabelPath;


        //Other urls
        private const string PeopleProfleUrl = ApiUrl + "people/profile";
        private const string FriendListUrl = ApiUrl + "friend/list";
        private const string UnreadCountUrl = ApiUrl + "unread-count";

        //PhotoBaseUrl
        private const string PhotoBaseUrl = "http://s2.googleusercontent.com";


        /// <summary>
        /// Client login url where we'll post login data to.
        /// </summary>
        private const string ClientLoginUrl = @"https://www.google.com/accounts/ClientLogin";

        private string _clientName;

        public GoogleReaderUrlBuilder(string clientName)
        {
            _clientName = clientName;
        }

        #region IUriBuilder Members

        public Uri BuildUri(UrlType type, StateType state, ReaderParameters parameters)
        {
            return MakeUri(type, state.ConvertToString(), parameters);
        }

        public Uri BuildUri(UrlType type, string itemName, ReaderParameters parameters)
        {
            return MakeUri(type, itemName, parameters);
        }

        public Uri BuildUri(UrlType type, ReaderParameters parameters)
        {
            return MakeUri(type, "", parameters);
        }

        public Uri BuildUri(UrlType type)
        {
            return MakeUri(type, "", new ReaderParameters());
        }


        public Uri GetLoginUri()
        {
            return new Uri(ClientLoginUrl);
        }

        public NameValueCollection GetLoginData(string username, string password)
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("service", SERVICENAME);
            values.Add("Email", username);
            values.Add("Passwd", password);
            values.Add("source", _clientName);
            values.Add("continue", CONTINUE);

            return values;
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
                case UrlType.People:
                    return new Uri(PeopleProfleUrl + queryString, UriKind.Absolute);
                case UrlType.FriendsEdit:
                    return new Uri(FriendListUrl + queryString, UriKind.Absolute);
                case UrlType.UnreadCount:
                    return new Uri(UnreadCountUrl + queryString, UriKind.Absolute);
                default:
                    return new Uri("");
            }
        }

        #endregion
    }
}
