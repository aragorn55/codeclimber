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
            IUriBuilder builder = new GoogleReaderUrlBuilder(clientName);
            IClientLoginService loginService = new GoogleReaderClientLogin(username, password, service, builder);
            service.ClientLogin = loginService;

            using (ReaderService rdr = new ReaderService(builder, service))
            {
                //foreach (FeedItem item in rdr.GetFeed("http://feeds.feedburner.com/codeclimber", new ReaderParameters() { Direction=ItemDirection.Descending, MaxItems=20}))
                //foreach (FeedItem item in rdr.GetState(StateType.ReadingList, new ReaderParameters() { Direction = ItemDirection.Default, MaxItems=100 }, true))
                foreach (FeedItem item in rdr.GetTag("ALT.net", new ReaderParameters() { Direction = ItemDirection.Default, MaxItems = 100 }, true))
                {
                    Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " +item.Author );
                }
            }

            // Pause.
            Console.ReadLine();
        }
    }
}
