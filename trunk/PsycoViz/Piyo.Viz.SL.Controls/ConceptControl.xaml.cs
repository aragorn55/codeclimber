#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System.Windows.Media;
using System.Windows.Shapes;

namespace Piyo.Viz.SL.Controls
{
    public class ConceptControl : EntityControl
    {
        private Path path;

        public ConceptControl()
        {
            path = FindName("circle") as Path;
        }

        protected override string ResourceName
        {
            get { return "ConceptControl.xaml"; }
        }


        protected override void ExecZoom()
        {
            path.Stretch = Stretch.Uniform;
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = ZoomFactor;
            scale.ScaleY = ZoomFactor;
            path.RenderTransform = scale;
        }
    }
}