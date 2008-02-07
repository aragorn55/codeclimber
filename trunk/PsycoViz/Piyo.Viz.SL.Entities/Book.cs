#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;

namespace Piyo.Viz.SL.Entities
{
    public class Book : Item
    {
        public Book(Guid guid) : base(guid)
        {
        }

        public Book(Guid guid, string title, DateTime date) : base(guid, title, date)
        {
        }
    }
}