using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector.JsonHelpers;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderParameters
    {
        public DateTime From { get; set; }
        public int MaxItems { get; set; }
        public ItemDirection Direction { get; set; }
        public string Client { get; set; }

        public string MakeQueryString()
        {
            return MakeQueryString(DateTime.Now);
        }

        public string MakeQueryString(DateTime currentTime)
        {
            List<String> paramParts = new List<string>();
            if (From != DateTime.MinValue)
                paramParts.Add(string.Format("ot={0}", From.ConvertToUnixTimestamp()));

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
}
