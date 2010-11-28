using System;
using System.Collections.Generic;

namespace CodeClimber.GoogleReaderConnector.Parameters
{
    public class ReaderParametersChoosableOutput : ReaderParametersBase
    {
        public OutputType Output { get; set; }

        public ReaderParametersChoosableOutput()
        {
            Output = OutputType.Json;
        }

        public override void FormatParameters(DateTime currentTime)
        {
            _paramParts.Add(string.Format("output={0}", Output.ToString().ToLowerInvariant()));
            base.FormatParameters(currentTime);
        }
    }

    public enum OutputType
    {
        Json,
        Xml
    }
}