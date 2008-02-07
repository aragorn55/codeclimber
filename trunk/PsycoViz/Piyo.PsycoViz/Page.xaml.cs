#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Piyo.Viz.SL.Controls;
using Piyo.Viz.SL.Entities;
using Piyo.Viz.SL.Repository;

namespace Piyo.PsycoViz
{
    [Scriptable]
    public partial class Page : Canvas
    {
        private TimeLineGrid _grid;
        private HtmlDocument document;
        private Canvas _container;

        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();

            try
            {
                document = HtmlPage.Document;
                _container = FindName("parentCanvas") as Canvas;
                BindEvents();

                AuthorRepository rep = new AuthorRepository();
                ItemRepository repItem = new ItemRepository();
                ConceptRepository repConcepts = new ConceptRepository();
                IList<Person> persons = rep.GetAuthors();

                _grid = FindName("grid") as TimeLineGrid;

                _grid.StopUpdate();
                _grid.Width = Width - 30;
                _grid.Height = Height;

                _grid.StartYear = _grid.StartViewYear = TimeLineHelper.GetMinYear(persons);
                _grid.EndYear = _grid.EndViewYear = TimeLineHelper.GetMaxYear(persons);

                _grid.Persons = persons;

                List<Book> books = new List<Book>();
                List<Seminar> seminars = new List<Seminar>();
                foreach (Person person in persons)
                {
                    _grid.Timemarkers.Add(person.Born.Year);
                    _grid.Timemarkers.Add(person.Dead.Year);
                    books.AddRange(repItem.GetBooks(person.Guid));
                    seminars.AddRange(repItem.GetSeminars(person.Guid));
                }

                _grid.Books = books;
                _grid.Seminars = seminars;
                _grid.Concepts = repConcepts.GetConcepts();

                _grid.StartUpdate();
                WebApplication.Current.RegisterScriptableObject("basic", this);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void BindEvents()
        {
            Image fsImg = FindName("fullScreen") as Image;
            fsImg.MouseLeftButtonUp += new MouseEventHandler(OnFullScreenClicked);
            BrowserHost.FullScreenChange += new EventHandler(BrowserHost_FullScreenChange);
            BrowserHost.Resize += BrowserHost_FullScreenChange;

            Image zoomInImg = FindName("zoomin") as Image;
            zoomInImg.MouseLeftButtonUp += new MouseEventHandler(zoomInImg_MouseLeftButtonUp);
            Image zoomOutImg = FindName("zoomout") as Image;
            zoomOutImg.MouseLeftButtonUp += new MouseEventHandler(zoomOutImg_MouseLeftButtonUp);

            Image firstImg = FindName("first") as Image;
            firstImg.MouseLeftButtonUp += new MouseEventHandler(firstImg_MouseLeftButtonUp);
            Image prevImg = FindName("prev") as Image;
            prevImg.MouseLeftButtonUp += new MouseEventHandler(prevImg_MouseLeftButtonUp);
            Image nextImg = FindName("next") as Image;
            nextImg.MouseLeftButtonUp += new MouseEventHandler(nextImg_MouseLeftButtonUp);
            Image lastImg = FindName("last") as Image;
            lastImg.MouseLeftButtonUp += new MouseEventHandler(lastImg_MouseLeftButtonUp);

            KeyUp += new KeyboardEventHandler(Page_KeyUp);
        }

        void Page_KeyUp(object sender, KeyboardEventArgs e)
        {
            switch(e.Key)
            {
                case 14:
                    _grid.PanLeft();
                    break;
                case 16:
                    _grid.PanRight();
                    break;
                case 15:
                    ZoomIn();
                    break;
                case 17:
                    ZoomOut();
                    break;

            }
        }

        private void firstImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _grid.PanStart();
        }

        private void prevImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _grid.PanLeft();
        }

        private void nextImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _grid.PanRight();
        }

        private void lastImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _grid.PanEnd();
        }

        private void zoomInImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            ZoomIn();
        }

        [Scriptable]
        public void ZoomIn()
        {
            _grid.ZoomFactor = _grid.ZoomFactor*2;
        }

        private void zoomOutImg_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            ZoomOut();
        }
        

        [Scriptable]
        public void ZoomOut()
        {
            if (_grid.ZoomFactor >= 2)
                _grid.ZoomFactor = _grid.ZoomFactor/2;
        }

        private void BrowserHost_FullScreenChange(object sender, EventArgs e)
        {
            _grid.StopUpdate();
            _grid.Width = BrowserHost.ActualWidth - 30;
            _grid.Height = BrowserHost.ActualHeight;
            _grid.StartUpdate();
        }

        private void OnFullScreenClicked(object sender, MouseEventArgs e)
        {
            BrowserHost.IsFullScreen = BrowserHost.IsFullScreen ? false : true;
        }
    }
}