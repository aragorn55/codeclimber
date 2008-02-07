#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System;
using System.Collections.Generic;
using Piyo.Viz.SL.Entities;

namespace Piyo.Viz.SL.Repository
{
    public class AuthorRepository
    {
        public static readonly Guid FREUDGUID = new Guid("{09A84825-1064-4ff9-A610-3C01643DCDE5}");
        public static readonly Guid LACANGUID = new Guid("{32B62CBE-28DD-4808-909E-54030ED2D0BA}");
        public static Person Freud;
        public static Person Lacan;

        public IList<Person> GetAuthors()
        {
            List<Person> persons = new List<Person>(2);
            Freud = new Person(FREUDGUID, new DateTime(1856, 10, 25), new DateTime(1939, 10, 10), new DateTime(1909, 10, 23), "Sigmund Freud");
            persons.Add(Freud);
            Lacan = new Person(LACANGUID, new DateTime(1901, 7, 4), new DateTime(1981, 3, 15), new DateTime(1954, 10, 23), "Lacan");
            persons.Add(Lacan);
            return persons;
        }
    }
}