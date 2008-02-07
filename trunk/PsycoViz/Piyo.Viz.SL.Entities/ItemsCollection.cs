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
    public class ItemsCollection : List<Item>
    {
        public void Add(Guid guid)
        {
            Add(new Item(guid));
        }
    }
}