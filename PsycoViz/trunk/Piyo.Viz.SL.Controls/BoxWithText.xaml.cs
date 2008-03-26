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
    public class BoxWithText : BaseControl
    {
        private Brush _backgroundBox1;
        private Brush _backgroundBox2;
        private Canvas _box1;
        private Canvas _box2;
        private TextBlock _tb;
        private string _text;

        private double _widthBox1;
        private double _widthBox2;

        public BoxWithText()
        {
            _tb = FindName("tb") as TextBlock;
            _box1 = FindName("box1") as Canvas;
            _box2 = FindName("box2") as Canvas;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public double WidthBox1
        {
            get { return _widthBox1; }
            set { _widthBox1 = value; }
        }

        public double WidthBox2
        {
            get { return _widthBox2; }
            set { _widthBox2 = value; }
        }

        public Brush BackgroundBox1
        {
            get { return _backgroundBox1; }
            set { _backgroundBox1 = value; }
        }

        public Brush BackgroundBox2
        {
            get { return _backgroundBox2; }
            set { _backgroundBox2 = value; }
        }

        protected override string ResourceName
        {
            get { return "BoxWithText.xaml"; }
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();

            _box1.Width = _widthBox1;
            _box2.Width = _widthBox2;
            _box1.Height = _box2.Height = Height;

            _box2.SetValue(Canvas.LeftProperty, WidthBox1);

            _box1.SetValue(Canvas.BackgroundProperty, _backgroundBox1);
            _box2.SetValue(Canvas.BackgroundProperty, _backgroundBox2);


            _tb.Text = _text;
            double textStartPosH = Width/2 - _tb.ActualWidth/2;
            double textStartPosV = Height/2 - _tb.ActualHeight/2;
            _tb.SetValue(Canvas.LeftProperty, textStartPosH);
            _tb.SetValue(Canvas.TopProperty, textStartPosV);
        }
    }
}