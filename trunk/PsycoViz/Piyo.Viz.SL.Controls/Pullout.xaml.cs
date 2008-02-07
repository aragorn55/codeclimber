#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Piyo.Viz.SL.Controls
{
    public class Pullout : BaseControl
    {
        private Point _containerSize;
        private Rectangle _rect;
        private Point _sourcePosition;
        private Storyboard _story;
        private Storyboard _storyTb;
        private TextBlock _tb;

        public Pullout()
        {
            _tb = FindName("tb") as TextBlock;
            _rect = FindName("rect") as Rectangle;
            _story = FindName("opacityAnim") as Storyboard;
            _storyTb = FindName("opacityAnimTxt") as Storyboard;
            _tb.TextWrapping = TextWrapping.Wrap;
            this.Loaded += new EventHandler(Pullout_Loaded);
            SetComponentsHeight(Height);
            SetComponentsWidth(Width);
        }

        public Point ContainerSize
        {
            get { return _containerSize; }
            set { _containerSize = value; }
        }

        public Point SourcePosition
        {
            get { return _sourcePosition; }
            set
            {
                _sourcePosition = value;
                SetPosition();
            }
        }

        public string Text
        {
            get { return _tb.Text; }
            set
            {
                _tb.Text = value;
                UpdateLayout();
            }
        }

        public string Background
        {
            get { return this.GetValue(Canvas.BackgroundProperty).ToString(); }
            set { this.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.Black)); }
        }


        protected override string ResourceName
        {
            get { return "Pullout.xaml"; }
        }


        private void Pullout_Loaded(object sender, EventArgs e)
        {
            _story.Begin();
            _storyTb.Begin();
        }


        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            SetComponentsWidth(Width);
            SetComponentsHeight(Height);


            double textStartPosH = Width/2 - _tb.ActualWidth/2;
            double textStartPosV = Height/2 - _tb.ActualHeight/2;
            _tb.SetValue(Canvas.LeftProperty, textStartPosH);
            _tb.SetValue(Canvas.TopProperty, textStartPosV);

            SetPosition();
        }

        private void SetPosition()
        {
            Point sourceCenter = _sourcePosition;

            Point targetPosition = new Point(sourceCenter.X + 5, sourceCenter.Y - Height - 10);

            if (targetPosition.X + Width > _containerSize.X)
                targetPosition.X = sourceCenter.X - Width - 5;

            if (targetPosition.Y < 0)
                targetPosition.Y = sourceCenter.Y + 10;


            SetValue(Canvas.LeftProperty, targetPosition.X);
            SetValue(Canvas.TopProperty, targetPosition.Y);
        }

        private void SetComponentsWidth(double minWidth)
        {
            double targetWidth = _tb.ActualWidth + 5;
            if (targetWidth < minWidth)
                targetWidth = minWidth;
            _tb.Width = targetWidth;
            _rect.Width = targetWidth;
            SetWidthNoUpdate(targetWidth);
        }


        private void SetComponentsHeight(double minHeight)
        {
            double targetHeight = _tb.ActualHeight + 5;
            if (targetHeight < minHeight)
                targetHeight = minHeight;
            _tb.Height = targetHeight;
            _rect.Height = targetHeight;
            SetHeightNoUpdate(targetHeight);
        }
    }
}