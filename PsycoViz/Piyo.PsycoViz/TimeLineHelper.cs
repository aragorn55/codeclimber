#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Collections.Generic;
using Piyo.Viz.SL.Entities;

namespace Piyo.PsycoViz
{
    public class TimeLineHelper
    {
        public static int GetMinYear(IList<Person> persons)
        {
            DateTime minYear = DateTime.MaxValue;
            foreach (Person person in persons)
            {
                if (person.Born < minYear)
                    minYear = person.Born;
            }
            return minYear.Year;
        }

        public static int GetMaxYear(IList<Person> persons)
        {
            DateTime maxYear = DateTime.MinValue;
            foreach (Person person in persons)
            {
                if (person.Dead > maxYear)
                    maxYear = person.Dead;
            }
            return maxYear.Year;
        }
    }
}