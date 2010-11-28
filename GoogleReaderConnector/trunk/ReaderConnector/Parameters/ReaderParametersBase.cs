using System;
using System.Collections.Generic;
using CodeClimber.GoogleReaderConnector.JsonHelpers;

namespace CodeClimber.GoogleReaderConnector.Parameters
{
    public abstract class ReaderParametersBase
    {
        public ReaderParametersBase()
        {
            _paramParts = new List<string>();
        }

        internal protected string Client { get; set; }
        protected List<string> _paramParts;

        public string MakeQueryString()
        {
            return MakeQueryString(DateTime.Now);
        }

        public string MakeQueryString(DateTime currentTime)
        {
            FormatParameters(currentTime);
            return String.Join("&", _paramParts.ToArray());
        }

        public virtual void FormatParameters(DateTime currentTime)
        {
            if (!String.IsNullOrEmpty(Client))
                _paramParts.Add(string.Format("client={0}", Client));
            _paramParts.Add(string.Format("ck={0}", currentTime.ConvertToUnixTimestamp()));            
        }
    }
}