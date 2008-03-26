#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Piyo.Viz.SL.Controls
{
    public abstract class EntityControl : BaseControl
    {
        private Point _position;
        private double _randomDisplacement;
        private double _rowHeight;
        private double _rowMiddleTop;
        private int _rowNumber;
        private int _zoomFactor = 1;

        protected EntityControl()
        {
            Cursor = Cursors.Hand;
        }

        public override Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public double X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public double Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }


        public double RandomDisplacement
        {
            get { return _randomDisplacement; }
            set
            {
                _randomDisplacement = value;
                UpdateY();
            }
        }

        public double RowHeight
        {
            get { return _rowHeight; }
            set { _rowHeight = value; }
        }

        public double RowMiddleTop
        {
            get { return _rowMiddleTop; }
            set
            {
                _rowMiddleTop = value;
                UpdateY();
            }
        }

        public int RowNumber
        {
            get { return _rowNumber; }
            set { _rowNumber = value; }
        }

        public int ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                int prev = _zoomFactor;
                _zoomFactor = value;
                if (prev < _zoomFactor)
                    ZoomIn();
                else if (prev > _zoomFactor)
                    ZoomOut();
            }
        }

        protected override void UpdateLayout()
        {
            SetValue(Canvas.TopProperty, _position.Y - Height/2);
            SetValue(Canvas.LeftProperty, _position.X - Width/2);
        }

        private void UpdateY()
        {
            Y = _rowMiddleTop + _randomDisplacement*_rowHeight/2;
        }

        protected virtual void ZoomIn()
        {
            Width = Width*2;
            Height = Height*2;
            ExecZoom();
        }

        protected virtual void ZoomOut()
        {
            Width = Width/2;
            Height = Height/2;
            ExecZoom();
        }

        protected abstract void ExecZoom();
    }
}