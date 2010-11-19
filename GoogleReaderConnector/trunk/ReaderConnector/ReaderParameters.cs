using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector.JsonHelpers;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderParameters
    {
        public DateTime FromDate { get; set; }
        public int MaxItems { get; set; }
        public ItemDirection Direction { get; set; }
        public string Client { get; set; }
        public IList<string> Exclude { get; set; }
        public OutputType Output { get; set; }
        public string UserId { get; set; }

        public ReaderParameters()
        {
            Exclude = new List<string>();
            Output = OutputType.Json;
        }

        public string MakeQueryString()
        {
            return MakeQueryString(DateTime.Now);
        }

        public string MakeQueryString(DateTime currentTime)
        {
            List<String> paramParts = new List<string>();
            if (FromDate != DateTime.MinValue)
                paramParts.Add(string.Format("ot={0}", FromDate.ConvertToUnixTimestamp()));

            if (!String.IsNullOrEmpty(Client))
                paramParts.Add(string.Format("client={0}", Client));

            if(MaxItems!=0)
                paramParts.Add(string.Format("n={0}", MaxItems));

            if(Direction!=ItemDirection.Default)
                switch (Direction)
                {
                    case ItemDirection.Descending:
                        paramParts.Add("r=d");
                        break;
                    case ItemDirection.Ascending:
                        paramParts.Add("r=o");
                        break;
                }

            if(Exclude.Count>0)
                foreach (var feedId in Exclude)
                {
                    paramParts.Add(string.Format("xt={0}", feedId));
                }

            if (!String.IsNullOrEmpty(UserId))
                paramParts.Add(string.Format("u={0}", UserId));

            paramParts.Add(string.Format("output={0}", Output.ToString().ToLowerInvariant()));
            paramParts.Add(string.Format("ck={0}", currentTime.ConvertToUnixTimestamp()));
            return String.Join("&",paramParts.ToArray());
        }
    }

    public enum ItemDirection
    { 
        Default,
        Descending,
        Ascending
    }

    public enum OutputType
    { 
        Json,
        Xml
    }
}
