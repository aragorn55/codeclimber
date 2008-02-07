#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;

namespace Piyo.Viz.SL.Entities
{
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        private Guid _guid;
        private ItemsCollection _items;
        private string _name;

        public BaseEntity(Guid guid)
        {
            _guid = guid;
            _items = new ItemsCollection();
        }

        public abstract DateTime Date { get; set; }

        public ItemsCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        #region IEquatable<BaseEntity> Members

        public bool Equals(BaseEntity other)
        {
            return Guid.Equals(other.Guid);
        }

        #endregion

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