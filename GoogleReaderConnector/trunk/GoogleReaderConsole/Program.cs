using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector;
using CodeClimber.GoogleReaderConnector.Services;
using CodeClimber.GoogleReaderConnector.Model;

namespace CodeClimber.GoogleReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");

            string username = "simone.chiaretta@gmail.com";
            string password = "e79d?poz";
            string clientName = "testing the API contact simone@piyosailing.com";

            // Query.

            IHttpService service = new HttpService();
            IUriBuilder builder = new GoogleReaderUrlBuilder();

            using (ReaderService rdr = new ReaderService(username, password, clientName, builder, service))
            {
                foreach (FeedItem item in rdr.GetFeedContent("http://feeds.feedburner.com/codeclimber", 5))
                {
                    Console.WriteLine("  - " + item.Author + ": " + item.Title);
                }
            }

            // Pause.
            Console.ReadLine();
        }
    }
}
