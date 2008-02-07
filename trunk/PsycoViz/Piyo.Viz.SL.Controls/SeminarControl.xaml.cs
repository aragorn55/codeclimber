#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System.Windows.Controls;
using System.Windows.Media;

namespace Piyo.Viz.SL.Controls
{
    public class SeminarControl : EntityControl
    {
        private Canvas _cont;

        public SeminarControl()
        {
            _cont = FindName("parentContainer") as Canvas;
        }

        protected override string ResourceName
        {
            get { return "SeminarControl.xaml"; }
        }

        protected override void ExecZoom()
        {
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = ZoomFactor;
            scale.ScaleY = ZoomFactor;
            _cont.RenderTransform = scale;
        }
    }
}