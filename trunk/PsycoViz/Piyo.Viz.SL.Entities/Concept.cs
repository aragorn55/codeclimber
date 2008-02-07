#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;

namespace Piyo.Viz.SL.Entities
{
    public class Concept : BaseEntity
    {
        private Item _firstOccurence;

        public Concept(string name) : base(Guid.NewGuid())
        {
            Name = name;
        }

        public Item FirstOccurence
        {
            get
            {
                if (_firstOccurence == null)
                    _firstOccurence = CalcFirstOccurrence();
                return _firstOccurence;
            }
        }

        public override DateTime Date
        {
            get
            {
                if (FirstOccurence != null)
                    return FirstOccurence.Date;
                else
                    return DateTime.MinValue;
            }
            set { }
        }

        private Item CalcFirstOccurrence()
        {
            Item firstItem = null;
            foreach (Item item in Items)
            {
                if (firstItem == null)
                    firstItem = item;
                else if (item.Date < firstItem.Date)
                    firstItem = item;
            }
            return firstItem;
        }
    }
}