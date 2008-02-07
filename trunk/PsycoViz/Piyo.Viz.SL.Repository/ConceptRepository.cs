#region Disclaimer

// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.

#endregion

using System.Collections.Generic;
using Piyo.Viz.SL.Entities;

namespace Piyo.Viz.SL.Repository
{
    public class ConceptRepository
    {
        public static Concept C1 = new Concept("Psicose");
        public static Concept C10 = new Concept("Seminaire III");
        public static Concept C11 = new Concept("C11");
        public static Concept C2 = new Concept("Psicose Paranoiaque");
        public static Concept C3 = new Concept("Paranoia");
        public static Concept C4 = new Concept("Psychotique");
        public static Concept C5 = new Concept("Autre");
        public static Concept C6 = new Concept("Signifiant");
        public static Concept C7 = new Concept("Senatsprasident");
        public static Concept C8 = new Concept("Castration");
        public static Concept C9 = new Concept("Réelle");


        public IList<Concept> GetConcepts()
        {
            List<Concept> concepts = new List<Concept>();
            concepts.Add(C1);
            concepts.Add(C2);
            concepts.Add(C3);
            concepts.Add(C4);
            concepts.Add(C5);
            concepts.Add(C6);
            concepts.Add(C7);
            concepts.Add(C8);
            concepts.Add(C9);
            concepts.Add(C10);
            concepts.Add(C11);
            return concepts;
        }
    }
}