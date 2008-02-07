#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Windows;
using System.Windows.Controls;

namespace Piyo.Viz.SL.Controls
{
    public abstract class BaseControl : Control
    {
        protected FrameworkElement _implementationRoot;

        public BaseControl()
        {
            System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("Piyo.Viz.SL.Controls." + ResourceName);

            _implementationRoot = this.InitializeFromXaml(new System.IO.StreamReader(s).ReadToEnd());
            base.Width = _implementationRoot.Width;
            base.Height = _implementationRoot.Height;


            Loaded += new EventHandler(OnLoaded);
        }

        protected FrameworkElement ImplementationRoot
        {
            get { return _implementationRoot; }
        }

        public virtual Point Position
        {
            get { return new Point((int) GetValue(Canvas.LeftProperty), (int) GetValue(Canvas.TopProperty)); }
            set { }
        }

        public new virtual double Width
        {
            get { return _implementationRoot.Width; }
            set
            {
                base.Width = _implementationRoot.Width = value;
                UpdateLayout();
            }
        }

        public new virtual double Height
        {
            get { return _implementationRoot.Height; }
            set
            {
                base.Height = _implementationRoot.Height = value;
                UpdateLayout();
            }
        }

        protected abstract string ResourceName { get; }

        protected virtual void OnLoaded(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        public void SetWidthNoUpdate(double value)
        {
            base.Width = _implementationRoot.Width = value;
        }

        public void SetHeightNoUpdate(double value)
        {
            base.Height = _implementationRoot.Height = value;
        }

        public new DependencyObject FindName(string name)
        {
            return _implementationRoot.FindName(name);
        }

        protected virtual void UpdateLayout()
        {
        }
    }
}