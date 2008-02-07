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
    public class ItemRepository
    {
        public static readonly Guid FREUDBOOK1GUID = new Guid("{858C2535-92C5-43ca-89FB-84BCF0636179}");
        public static readonly Guid FREUDBOOK2GUID = new Guid("{B5CAACED-24CF-432b-8BEA-704F9C8D5106}");
        public static readonly Guid FREUDBOOK3GUID = new Guid("{46C7EFF9-B1D9-4014-8955-1D6657B39626}");
        public static readonly Guid FREUDBOOK4GUID = new Guid("{F95678E7-BBCC-4e76-A51C-B335EF30C997}");
        public static readonly Guid FREUDBOOK5GUID = new Guid("{EDD2CB65-2A90-4b62-8C68-F8355E7757C7}");
        public static readonly Guid FREUDSEM1GUID = new Guid("{2A68076C-2550-4b33-B088-2149C680DC71}");
        public static readonly Guid FREUDSEM2GUID = new Guid("{1221C85A-3603-43a4-9EEE-FB9B2E9666EB}");
        public static readonly Guid FREUDSEM3GUID = new Guid("{0CB96BF9-0439-47dd-AC0D-42B152FBB349}");
        public static readonly Guid FREUDSEM4GUID = new Guid("{EF620040-58AF-4d29-91CA-299A2510F818}");
        public static readonly Guid FREUDSEM5GUID = new Guid("{C54E4E15-F526-4e7d-9B8C-E9CA82C04C0C}");

        public static readonly Guid LACANBOOK1GUID = new Guid("{D43852F6-98F4-4f19-8D4B-2768F1F96888}");
        public static readonly Guid LACANBOOK2GUID = new Guid("{76100C9C-3FD8-4243-82E5-C4F8F9746CD2}");
        public static readonly Guid LACANBOOK3GUID = new Guid("{7F366CCA-8700-48d8-9D97-AD690E4103AB}");
        public static readonly Guid LACANBOOK4GUID = new Guid("{41247EF4-7E76-4404-ACCE-17E36DCF5F8F}");
        public static readonly Guid LACANBOOK5GUID = new Guid("{66196C71-799E-412a-BFB8-1A5E76777721}");
        public static readonly Guid LACANBOOK6GUID = new Guid("{EFC93537-9343-47ed-95FC-163C16C827E3}");

        public static readonly Guid LACANSEM1GUID = new Guid("{CE5DE90E-0082-4b68-828C-1923AD4D7D7D}");
        public static readonly Guid LACANSEM2GUID = new Guid("{6AB06AB9-D41E-43c1-AA93-0A803078DC00}");
        public static readonly Guid LACANSEM3GUID = new Guid("{DC21561A-5AB2-40cc-A977-E712636D5346}");
        public static readonly Guid LACANSEM4GUID = new Guid("{96191C66-2992-4073-9FF0-7A909FA56874}");
        public static readonly Guid LACANSEM5GUID = new Guid("{8A4E95E1-862D-4c64-9D00-D11C37CA327D}");

        public IList<Book> GetBooks(Guid _guidAuthor)
        {
            List<Book> books = new List<Book>();
            if (_guidAuthor.Equals(AuthorRepository.FREUDGUID))
            {
                Book book1 = new Book(FREUDBOOK1GUID, "Psychoanalitiche bemerkungeb...", new DateTime(1910, 10, 23));
                book1.Author = AuthorRepository.Freud;
                books.Add(book1);
                Book book2 = new Book(FREUDBOOK2GUID, "La perte de la realité dans la nevrose et la psicose", new DateTime(1924, 5, 10));
                book2.Author = AuthorRepository.Freud;
                books.Add(book2);
                Book book3 = new Book(FREUDBOOK3GUID, "Zur Ephfuhrung der Narzissimus", new DateTime(1914, 2, 5));
                book3.Author = AuthorRepository.Freud;
                books.Add(book3);
                Book book4 = new Book(FREUDBOOK4GUID, "Book F4", new DateTime(1920, 6, 14));
                book4.Author = AuthorRepository.Freud;
                books.Add(book4);
                //Book book5 = new Book(FREUDBOOK5GUID, "Book5", new DateTime(1927, 12, 31));
                //book5.Author = AuthorRepository.Freud;
                //books.Add(book5);

                //book5.RelatedItems.Add(book3);
                //book5.RelatedItems.Add(book2);
                //book5.RelatedItems.Add(book1);

                //book4.RelatedItems.Add(book1);

                //book3.RelatedItems.Add(book1);
                //book3.RelatedItems.Add(book2);

                //book2.RelatedItems.Add(book1);
                //book2.AddConcept(ConceptRepository.C1);

                book1.AddConcept(ConceptRepository.C7);
                book1.Items.Add(FREUDSEM1GUID);
                book2.AddConcept(ConceptRepository.C8);
                book2.Items.Add(book4);
                book3.AddConcept(ConceptRepository.C9);
                book4.AddConcept(ConceptRepository.C1);
            }
            else if (_guidAuthor.Equals(AuthorRepository.LACANGUID))
            {
                Book book1 = new Book(LACANBOOK1GUID, "D'une questione preliminare à tout traitement possible de la psicose", new DateTime(1958, 1, 23));
                book1.Author = AuthorRepository.Lacan;
                books.Add(book1);
                Book book2 = new Book(LACANBOOK2GUID, "Preface a edition anglaise du seminaire XI", new DateTime(1976, 5, 10));
                book2.Author = AuthorRepository.Lacan;
                books.Add(book2);
                Book book3 = new Book(LACANBOOK3GUID, "Joyce le symptome", new DateTime(1975, 2, 5));
                book3.Author = AuthorRepository.Lacan;
                books.Add(book3);
                //Book book4 = new Book(LACANBOOK4GUID, "Book4", new DateTime(1965, 6, 14));
                //book4.Author = AuthorRepository.Lacan;
                //books.Add(book4);
                //Book book5 = new Book(LACANBOOK5GUID, "Book5", new DateTime(1966, 11, 30));
                //book5.Author = AuthorRepository.Lacan;
                //books.Add(book5);
                //Book book6 = new Book(LACANBOOK6GUID, "Book6", new DateTime(1968, 12, 31));
                //book6.Author = AuthorRepository.Lacan;
                //books.Add(book6);

                book1.AddConcept(ConceptRepository.C4);
                book2.AddConcept(ConceptRepository.C5);
                book3.AddConcept(ConceptRepository.C6);

                book1.Items.Add(LACANSEM1GUID);
                book1.Items.Add(FREUDBOOK4GUID);

                book2.Items.Add(LACANSEM3GUID);
                book3.Items.Add(LACANSEM2GUID);
            }
            return books;
        }


        public IList<Seminar> GetSeminars(Guid _guidAuthor)
        {
            List<Seminar> seminars = new List<Seminar>();
            if (_guidAuthor.Equals(AuthorRepository.FREUDGUID))
            {
                Seminar seminar1 = new Seminar(FREUDSEM1GUID, "Seminar1", new DateTime(1909, 10, 23));
                seminar1.Author = AuthorRepository.Freud;
                seminars.Add(seminar1);
                Seminar seminar2 = new Seminar(FREUDSEM2GUID, "Seminar2", new DateTime(1914, 5, 10));
                seminar2.Author = AuthorRepository.Freud;
                seminars.Add(seminar2);
                Seminar seminar3 = new Seminar(FREUDSEM3GUID, "Seminar3", new DateTime(1920, 2, 5));
                seminar3.Author = AuthorRepository.Freud;
                seminars.Add(seminar3);


                seminar1.AddConcept(ConceptRepository.C7);

            }
            else if (_guidAuthor.Equals(AuthorRepository.LACANGUID))
            {
                Seminar seminar1 = new Seminar(LACANSEM1GUID, "Les ecrits techniques de Freud", new DateTime(1954, 10, 23));
                seminar1.Author = AuthorRepository.Lacan;
                seminars.Add(seminar1);
                Seminar seminar2 = new Seminar(LACANSEM2GUID, "Le psycoses", new DateTime(1956, 5, 10));
                seminar2.Author = AuthorRepository.Lacan;
                seminars.Add(seminar2);
                Seminar seminar3 = new Seminar(LACANSEM3GUID, "Le sinthome", new DateTime(1976, 2, 5));
                seminar3.Author = AuthorRepository.Lacan;
                seminars.Add(seminar3);
                //Seminar seminar4 = new Seminar(LACANSEM4GUID, "Seminar4", new DateTime(1961, 6, 14));
                //seminar4.Author = AuthorRepository.Lacan;
                //seminars.Add(seminar4);
                //Seminar seminar5 = new Seminar(LACANSEM5GUID, "Seminar5", new DateTime(1965, 12, 31));
                //seminar5.Author = AuthorRepository.Lacan;
                //seminars.Add(seminar5);

                seminar1.AddConcept(ConceptRepository.C1);
                seminar1.Items.Add(FREUDBOOK4GUID);
                seminar2.AddConcept(ConceptRepository.C10);
                seminar3.AddConcept(ConceptRepository.C5);
                seminar3.AddConcept(ConceptRepository.C9);
            }
            return seminars;
        }
    }
}