#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Collections.Generic;

namespace Piyo.Viz.SL.Entities
{
    public class Person
    {
        private DateTime _born;
        private DateTime _dead;
        private DateTime _firstOccurrence;
        private string _fullName;
        private Guid _guid;
        private IList<Item> _items;

        public Person(Guid guid)
        {
            _guid = guid;
            _items = new List<Item>();
        }

        public Person(Guid guid, DateTime born, DateTime dead, DateTime firstEvent, string fullName) : this(guid)
        {
            _born = born;
            _dead = dead;
            _fullName = fullName;
            _firstOccurrence = firstEvent;
        }

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public IList<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public DateTime Born
        {
            get { return _born; }
            set { _born = value; }
        }

        public DateTime Dead
        {
            get { return _dead; }
            set { _dead = value; }
        }

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public DateTime FirstOccurrence
        {
            get { return _firstOccurrence; }
            set { _firstOccurrence = value; }
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return _guid.Equals(obj);
        }
    }
}