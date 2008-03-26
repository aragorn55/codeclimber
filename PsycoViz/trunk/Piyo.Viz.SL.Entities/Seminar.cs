#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;

namespace Piyo.Viz.SL.Entities
{
    public class Seminar : Item
    {
        private Book _paper;
        private bool _published;

        public Seminar(Guid guid, string title, DateTime date) : base(guid, title, date)
        {
        }

        public Seminar(Guid guid) : base(guid)
        {
        }

        public Seminar(Guid guid, string title, DateTime date, Book paper, bool published) : base(guid, title, date)
        {
            _paper = paper;
            _published = published;
        }

        public Seminar(Guid guid, Book paper, bool published) : base(guid)
        {
            _paper = paper;
            _published = published;
        }

        public Book Paper
        {
            get { return _paper; }
            set { _paper = value; }
        }

        public bool Published
        {
            get { return _published; }
            set { _published = value; }
        }
    }
}