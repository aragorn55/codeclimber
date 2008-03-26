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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Piyo.Viz.SL.Entities;

namespace Piyo.Viz.SL.Controls
{
    public class TimeLineGrid : BaseControl
    {
        private const int boxHeight = 20;
        private const int boxPadding = 5;
        private const int objectRows = 3;
        private const double rowPadding = 10;
        private static string[] colors = new string[6] { "#DECEE0", "#5A0964", "#EAEFD8", "#97B03B", "#00FFFF", "#00CCCC" };
        private IList<Book> _books;
        private IList<Concept> _concepts;
        private Dictionary<EntityControl, IList<Line>> _connections = new Dictionary<EntityControl, IList<Line>>();

        private Canvas _container;
        private Dictionary<BaseEntity, EntityControl> _elementMapping = new Dictionary<BaseEntity, EntityControl>();
        private int _endViewYear = 0;
        private int _endYear;
        private Dictionary<EntityControl, BaseEntity> _graphicRepresentation = new Dictionary<EntityControl, BaseEntity>();
        private IList<Person> _persons;

        private Dictionary<Visual, Pullout> _pullouts = new Dictionary<Visual, Pullout>();
        private Random _rndGenerator;
        private double _scale;
        private IList<Seminar> _seminars;
        private int _startViewYear = 0;

        private int _startYear;
        private List<BaseEntity> _stickyConnections = new List<BaseEntity>();
        private IList<int> _timemarkers;
        private bool _updateLayout = true;
        private int _zoomFactor = 1;


        public TimeLineGrid()
        {
            _container = FindName("parentCanvas") as Canvas;
            _persons = new List<Person>();
            _books = new List<Book>();
            _seminars = new List<Seminar>();
            _concepts = new List<Concept>();
            _timemarkers = new List<int>();
            _rndGenerator = new Random(DateTime.Now.Millisecond);
        }

        public int StartYear
        {
            get { return _startYear; }
            set
            {
                _startYear = value;
                UpdateLayout();
            }
        }

        public int EndYear
        {
            get { return _endYear; }
            set
            {
                _endYear = value;
                UpdateLayout();
            }
        }


        public int StartViewYear
        {
            get { return _startViewYear; }
            set
            {
                _startViewYear = value;
                _scale = UpdateScale();
                UpdateLayout();
            }
        }

        public int EndViewYear
        {
            get { return _endViewYear; }
            set
            {
                _endViewYear = value;
                _scale = UpdateScale();
                UpdateLayout();
            }
        }

        public int ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                int prevZoom = _zoomFactor;
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Zoom Factor must be bigger than 1.");
                _zoomFactor = value;
                if (UpdateViewport())
                    UpdateLayout();
                else
                    _zoomFactor = prevZoom;
            }
        }

        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                UpdateLayout();
            }
        }

        public override double Height
        {
            get { return base.Height; }
            set
            {
                base.Height = value;
                RecalculateVerticalDisplacement();
            }
        }

        public override double Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                _scale = UpdateScale();
            }
        }

        public IList<int> Timemarkers
        {
            get { return _timemarkers; }
            set { _timemarkers = value; }
        }

        public IList<Person> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                UpdateLayout();
            }
        }

        public IList<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                UpdateItemControls();
                UpdateLayout();
            }
        }

        public IList<Concept> Concepts
        {
            get { return _concepts; }
            set
            {
                _concepts = value;
                UpdateItemControls();
                UpdateLayout();
            }
        }

        public IList<Seminar> Seminars
        {
            get { return _seminars; }
            set
            {
                _seminars = value;
                UpdateItemControls();
                UpdateLayout();
            }
        }

        protected override string ResourceName
        {
            get { return "TimeLineGrid.xaml"; }
        }

        private void RecalculateVerticalDisplacement()
        {
            double spaceNeededBehindTimeline = GetSpaceNeededBehindTimeline();
            double rowHeight = GetRowHeight(spaceNeededBehindTimeline);

            foreach (EntityControl ctl in _elementMapping.Values)
            {
                ctl.RowHeight = rowHeight;
                ctl.RowMiddleTop = CalculateRowMiddle(rowHeight, ctl.RowNumber);
            }
        }

        public bool StartUpdate()
        {
            _updateLayout = true;
            UpdateLayout();
            return true;
        }

        public bool StopUpdate()
        {
            _updateLayout = false;
            return false;
        }

        protected override void UpdateLayout()
        {
            if (_updateLayout)
                ExecUpdateLayout();
        }

        public void PanStart()
        {
            StopUpdate();
            int viewport = _endViewYear - _startViewYear;
            StartViewYear = _startYear;
            EndViewYear = _startViewYear + viewport;
            StartUpdate();
        }

        public void PanEnd()
        {
            StopUpdate();
            int viewport = _endViewYear - _startViewYear;
            EndViewYear = _endYear;
            StartViewYear = _endViewYear - viewport;
            StartUpdate();
        }

        public void PanRight()
        {
            StopUpdate();
            int viewport = _endViewYear - _startViewYear;
            if (_endViewYear + viewport > _endYear)
                PanEnd();
            else
            {
                StartViewYear = _endViewYear;
                EndViewYear = _startViewYear + viewport;
                StartUpdate();
            }
        }

        public void PanLeft()
        {
            StopUpdate();
            int viewport = _endViewYear - _startViewYear;
            if (_startViewYear - viewport < _startYear)
                PanStart();
            else
            {
                EndViewYear = _startViewYear;
                StartViewYear = _endViewYear - viewport;
                StartUpdate();
            }
        }


        private void UpdateItemControls()
        {
            _elementMapping.Clear();
            _graphicRepresentation.Clear();

            double spaceNeededBehindTimeline = GetSpaceNeededBehindTimeline();
            double rowHeight = GetRowHeight(spaceNeededBehindTimeline);
            foreach (Concept concept in _concepts)
            {
                Item firstOccurence = concept.FirstOccurence;
                if (firstOccurence != null)
                {
                    int year = firstOccurence.Date.Year;
                    ConceptControl conceptCtl = new ConceptControl();
                    conceptCtl.RowNumber = 1;
                    SetupEntityControl(rowHeight, conceptCtl, year);


                    _graphicRepresentation.Add(conceptCtl, concept);
                    _elementMapping.Add(concept, conceptCtl);
                }
            }

            foreach (Book book in _books)
            {
                int year = book.Date.Year;
                BookControl bookCtl = new BookControl();
                bookCtl.RowNumber = 2;
                SetupEntityControl(rowHeight, bookCtl, year);

                _graphicRepresentation.Add(bookCtl, book);
                _elementMapping.Add(book, bookCtl);
            }

            foreach (Seminar seminar in _seminars)
            {
                int year = seminar.Date.Year;
                SeminarControl seminarCtl = new SeminarControl();
                seminarCtl.RowNumber = 3;
                SetupEntityControl(rowHeight, seminarCtl, year);

                _graphicRepresentation.Add(seminarCtl, seminar);
                _elementMapping.Add(seminar, seminarCtl);
            }
        }

        private void SetupEntityControl(double rowHeight, EntityControl entityCtl, int year)
        {
            entityCtl.MouseLeftButtonDown += item_Click;
            entityCtl.MouseEnter += entity_Over;
            entityCtl.MouseLeave += entity_Out;
            entityCtl.RandomDisplacement = RandomDisplacement(-1, 1);
            entityCtl.RowHeight = rowHeight;

            entityCtl.X = AbsoluteYearToPixel(year);
            entityCtl.RowMiddleTop = CalculateRowMiddle(rowHeight, entityCtl.RowNumber);
        }

        private static double CalculateRowMiddle(double rowHeight, int i)
        {
            return rowPadding + (rowHeight)*(i - 1) + rowHeight/2;
        }

        private double GetRowHeight(double neededBehindTimeline)
        {
            double usableSpace = Height - neededBehindTimeline - rowPadding*2;
            return usableSpace/objectRows;
        }

        private void entity_Out(object sender, EventArgs e)
        {
            try
            {
                EntityControl clickSource = sender as EntityControl;
                if (clickSource == null)
                    return;

                BaseEntity sourceItem = _graphicRepresentation[clickSource];
                if (sourceItem == null)
                    return;
                if (_pullouts.ContainsKey(clickSource))
                {
                    DeletePullout(clickSource);
                }
                if (_connections.ContainsKey(clickSource))
                {
                    if (!_stickyConnections.Contains(sourceItem))
                        DeleteConnections(clickSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void entity_Over(object sender, MouseEventArgs e)
        {
            try
            {
                EntityControl clickSource = sender as EntityControl;
                if (clickSource == null)
                    return;

                BaseEntity sourceItem = _graphicRepresentation[clickSource];
                if (sourceItem == null)
                    return;
                if (!_pullouts.ContainsKey(clickSource))
                {
                    DrawPullout(clickSource, sourceItem.Name);
                }
                if (!_connections.ContainsKey(clickSource))
                {
                    if (!_stickyConnections.Contains(sourceItem))
                        DrawConnections(clickSource, sourceItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void item_Click(object sender, MouseEventArgs e)
        {
            try
            {
                EntityControl clickSource = sender as EntityControl;
                if (clickSource == null)
                    return;

                BaseEntity sourceItem = _graphicRepresentation[clickSource];
                if (sourceItem == null)
                    return;
                if (!_stickyConnections.Contains(sourceItem))
                {
                    _stickyConnections.Add(sourceItem);
                }
                else
                {
                    _stickyConnections.Remove(sourceItem);
                    DeleteConnections(clickSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ReDrawAllStickyConnections()
        {
            _connections.Clear();
            foreach (BaseEntity entity in _stickyConnections)
            {
                EntityControl control = _elementMapping[entity];
                DrawConnections(control, entity);
            }
        }

        private void DrawConnections(EntityControl clickSource, BaseEntity sourceItem)
        {
            List<Line> lines = new List<Line>(sourceItem.Items.Count);
            _connections.Add(clickSource, lines);
            foreach (Item item in sourceItem.Items)
            {
                EntityControl destination = _elementMapping[item];
                Line connection = DrawConnection(clickSource, destination);
                _container.Children.Add(connection);
                lines.Add(connection);
            }
        }

        private Line DrawConnection(EntityControl source, EntityControl destination)
        {
            Point sourceCenter = source.Position;
            Point destCenter = destination.Position;
            Line connector = new Line();
            connector.X1 = sourceCenter.X;
            connector.Y1 = sourceCenter.Y;
            connector.X2 = destCenter.X;
            connector.Y2 = destCenter.Y;
            connector.SetValue(Line.StrokeProperty, "#000000");
            connector.SetValue(Line.StrokeThicknessProperty, 2*ZoomFactor);
            connector.Opacity = 0.5;
            connector.SetValue(ZIndexProperty, -1);
            return connector;
        }

        private void DeleteConnections(EntityControl clickSource)
        {
            List<Line> lines = (List<Line>) _connections[clickSource];
            foreach (Line line in lines)
            {
                _container.Children.Remove(line);
            }
            _connections.Remove(clickSource);
        }

        private void ExecUpdateLayout()
        {
            _container.Children.Clear();

            base.UpdateLayout();

            double spaceNeededBehindTimeline = GetSpaceNeededBehindTimeline();
            _container.Children.Add(DrawTimeline(spaceNeededBehindTimeline));
            //_container.Children.Add(DrawGrid(Height - spaceNeededBehindTimeline));
            foreach (int year in _timemarkers)
            {
                Line marker = DrawMarker(year, Height - spaceNeededBehindTimeline);
                if (marker != null)
                    _container.Children.Add(marker);
            }

            double boxTop = Height - spaceNeededBehindTimeline + boxPadding;
            int i = 0;
            foreach (Person person in _persons)
            {
                BoxWithText box = DrawAuthorBox(person, boxTop, i);
                if (box != null)
                {
                    _container.Children.Add(box);
                }
                boxTop += boxPadding + boxHeight;
                i++;
            }

            _container.Children.Add(DrawGrid(Height - boxPadding));

            foreach (BaseEntity item in _elementMapping.Keys)
            {
                int year = item.Date.Year;
                EntityControl ctl = _elementMapping[item];
                ctl.X = RelativeYearToPixel(year);
                ctl.ZoomFactor = ZoomFactor;
                _container.Children.Add(ctl);
            }

            ReDrawAllStickyConnections();
        }

        private double GetSpaceNeededBehindTimeline()
        {
            double spaceNeededBehindTimeline = 5;
            if (_persons != null)
                spaceNeededBehindTimeline += (boxPadding + boxHeight)*_persons.Count;
            return spaceNeededBehindTimeline;
        }

        private Line DrawMarker(int year, double top)
        {
            if (!IsYearInViewPort(year))
                return null;

            double pos = RelativeYearToPixel(year);
            string hex = "#000000";
            string xamlStr = "<Line X1=\"" + pos + "\" Y1=\"" + (top - 5) + "\" X2=\"" + pos + "\" Y2=\"" + (top + 5) + "\" Stroke=\"" + hex +
                             "\" StrokeThickness=\"2\"></Line>";
            xamlStr = xamlStr.Replace(',', '.');
            Line marker = (Line) XamlReader.Load(xamlStr);
            marker.Tag = year.ToString();
            marker.MouseEnter += new MouseEventHandler(marker_MouseEnter);
            marker.MouseLeave += new EventHandler(marker_MouseLeave);
            marker.Cursor = Cursors.Hand;
            return marker;
        }

        private void marker_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                Line clickSource = sender as Line;
                if (clickSource == null)
                    return;

                if (_pullouts.ContainsKey(clickSource))
                {
                    DeletePullout(clickSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void DeletePullout(Visual control)
        {
            Pullout popup = _pullouts[control];
            _container.Children.Remove(popup);
            _pullouts.Remove(control);
        }

        private void marker_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Line clickSource = sender as Line;
                if (clickSource == null)
                    return;

                if (!_pullouts.ContainsKey(clickSource))
                {
                    DrawPullout(clickSource, clickSource.Tag);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void DrawPullout(EntityControl control, string name)
        {
            Point sourcePosition = control.Position;
            DrawPullout(sourcePosition, control, name);
        }

        private void DrawPullout(Line marker, string year)
        {
            Point sourcePosition = new Point(marker.X1, marker.Y1);
            DrawPullout(sourcePosition, marker, year);
        }

        private void DrawPullout(Point sourcePosition, Visual control, string text)
        {
            Pullout pullout = new Pullout();
            pullout.Text = text;
            pullout.SourcePosition = sourcePosition;
            pullout.ContainerSize = new Point(Width, Height);
            _pullouts.Add(control, pullout);
            _container.Children.Add(pullout);
        }

        private bool UpdateViewport()
        {
            int timespan = _endYear - _startYear;
            if (_zoomFactor > timespan)
                return false;
            else
            {
                int targetEndView = _startViewYear + timespan/_zoomFactor;
                if (targetEndView > _endYear)
                    targetEndView = _endYear;

                _endViewYear = targetEndView;
                StartViewYear = _endViewYear - timespan/_zoomFactor;

                return true;
            }
        }

        private double UpdateScale()
        {
            double timespan = _endViewYear - _startViewYear;
            return Width/timespan;
        }

        private double YearToPixel(int value)
        {
            return value*_scale;
        }

        private double AbsoluteYearToPixel(int year)
        {
            return YearToPixel(year - _startYear);
        }

        private double RelativeYearToPixel(int year)
        {
            return YearToPixel(year - _startViewYear);
        }

        private bool IsYearInViewPort(int year)
        {
            if (year < _startViewYear)
                return false;
            if (year > _endViewYear)
                return false;
            return true;
        }

        private BoxWithText DrawAuthorBox(Person person, double top, int seq)
        {
            int startYear = person.Born.Year;
            int endYear = person.Dead.Year;
            int firstEventYear = person.FirstOccurrence.Year;

            if (endYear < _startViewYear)
                return null;
            if (startYear > _endViewYear)
                return null;

            if (startYear < _startViewYear)
                startYear = _startViewYear;

            if (endYear > _endViewYear)
                endYear = _endViewYear;

            if (firstEventYear > _endViewYear)
                firstEventYear = _endViewYear;

            if (firstEventYear < _startViewYear)
                firstEventYear = _startViewYear;

            int totalTimespan = endYear - startYear;
            int activeTimespan = endYear - firstEventYear;

            BoxWithText bwt = new BoxWithText();
            bwt.Height = boxHeight;
            bwt.Width = YearToPixel(totalTimespan);
            bwt.WidthBox1 = YearToPixel(totalTimespan - activeTimespan);
            bwt.WidthBox2 = YearToPixel(activeTimespan);
            bwt.Text = person.FullName;
            bwt.SetValue(Canvas.TopProperty, top);
            bwt.SetValue(Canvas.LeftProperty, RelativeYearToPixel(startYear));

            Color color1 = Helpers.HexToColor(colors[seq*2]);
            Color color2 = Helpers.HexToColor(colors[(seq*2) + 1]);

            bwt.BackgroundBox1 = new SolidColorBrush(color1);
            bwt.BackgroundBox2 = new SolidColorBrush(color2);

            return bwt;
        }

        private Line DrawTimeline(double top)
        {
            string hex = "#000000";
            string xamlStr = "<Line X1=\"0\" Y1=\"" + (Height - top) + "\" X2=\"" + Width + "\" Y2=\"" + (Height - top) + "\" Stroke=\"" + hex + "\" StrokeThickness=\"2\"></Line>";
            return (Line) XamlReader.Load(xamlStr);
        }


        private Canvas DrawGrid(double height)
        {
            Canvas gridBackGround = new Canvas();
            for (int i = _startViewYear; i < _endViewYear; i += 5)
            {
                gridBackGround.Children.Add(DrawGridLine(i, height));
            }
            gridBackGround.Children.Add(DrawGridLine(_endViewYear, height));
            gridBackGround.SetValue(Canvas.TopProperty, 0);
            gridBackGround.SetValue(Canvas.LeftProperty, 0);
            gridBackGround.Width = Width;
            gridBackGround.Height = height;
            return gridBackGround;
        }

        private Line DrawGridLine(int year, double height)
        {
            Line line = new Line();
            double yearPos = RelativeYearToPixel(year);
            line.X1 = yearPos;
            line.Y1 = 0;
            line.X2 = yearPos;
            line.Y2 = height;
            line.StrokeThickness = 1;
            line.SetValue(Line.StrokeProperty, "#000000");
            line.Opacity = 0.1;
            return line;
        }

        private double RandomDisplacement(int min, int max)
        {
            int rndNum = _rndGenerator.Next(min*100, max*100);
            return ((double) rndNum)/100;
        }
    }
}