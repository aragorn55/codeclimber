using System;
using System.Collections.Generic;
using System.Linq;
using CodeClimber.GoogleReaderConnector.JsonHelpers;

namespace CodeClimber.GoogleReaderConnector.Parameters
{
    public class ReaderFeedParameters : ReaderParametersBase
    {
        public DateTime FromDate { get; set; }
        public int MaxItems { get; set; }
        public ItemDirection Direction { get; set; }
        public IList<string> Exclude { get; set; }

        public ReaderFeedParameters()
        {
            Exclude = new List<string>();

        }

        public override void FormatParameters(DateTime currentTime)
        {
            if (FromDate != DateTime.MinValue)
                _paramParts.Add(string.Format("ot={0}", FromDate.ConvertToUnixTimestamp()));

            if(MaxItems!=0)
                _paramParts.Add(string.Format("n={0}", MaxItems));

            if(Direction!=ItemDirection.Default)
                switch (Direction)
                {
                    case ItemDirection.Descending:
                        _paramParts.Add("r=d");
                        break;
                    case ItemDirection.Ascending:
                        _paramParts.Add("r=o");
                        break;
                }

            if(Exclude.Count>0)
                _paramParts.AddRange(Exclude.Select(feedId => string.Format("xt={0}", (object) feedId)));
            base.FormatParameters(currentTime);

        }
    }

    public enum ItemDirection
    {
        Default,
        Descending,
        Ascending
    }
}