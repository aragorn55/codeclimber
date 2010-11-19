using System.Collections.Generic;


namespace CodeClimber.GoogleReaderConnector.Model
{
    public class Friend
    {
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Nickname { get; set; }
        public string PhotoUrl { get; set; }
        public string Location { get; set; }
        public string Occupation { get; set; }
        public IList<string> EmailAddresses { get; set; }
    }
}
