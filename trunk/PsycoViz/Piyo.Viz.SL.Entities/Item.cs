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
    public class Item : BaseEntity
    {
        private Person _author;
        private IList<Concept> _concepts;
        private DateTime _date;

        public Item(Guid guid) : base(guid)
        {
            _concepts = new List<Concept>();
        }

        public Item(Guid guid, string title, DateTime date) : this(guid)
        {
            Name = title;
            _date = date;
        }

        public Person Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Title
        {
            get { return Name; }
            set { Name = value; }
        }

        public IList<Concept> Concepts
        {
            get { return _concepts; }
            set { _concepts = value; }
        }

        public override DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public void AddConcept(Concept concept)
        {
            _concepts.Add(concept);
            if (!concept.Items.Contains(this))
                concept.Items.Add(this);
        }
    }
}