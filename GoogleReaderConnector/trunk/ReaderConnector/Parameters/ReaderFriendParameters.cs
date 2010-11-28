using System;

namespace CodeClimber.GoogleReaderConnector.Parameters
{
    class ReaderFriendParameters: ReaderParametersChoosableOutput
    {
        public string UserId { get; set; }

        public override void FormatParameters(DateTime currentTime)
        {
            if (!String.IsNullOrEmpty(UserId))
                _paramParts.Add(string.Format("u={0}", UserId));
            base.FormatParameters(currentTime);
        }
    }
}
