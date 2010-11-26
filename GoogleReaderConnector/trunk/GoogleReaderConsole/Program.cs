using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector;
using CodeClimber.GoogleReaderConnector.Services;
using CodeClimber.GoogleReaderConnector.Model;
using CodeClimber.GoogleReaderConnector.Exceptions;

namespace CodeClimber.GoogleReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "";
            string password = "";
            string clientName = "test";

            // Query.

            IHttpService service = new HttpService();
            IUriBuilder builder = new GoogleReaderUrlBuilder(clientName);
            IClientLoginService loginService = new GoogleReaderClientLogin(username, password, service, builder);
            service.ClientLogin = loginService;

            ReaderService rdr = new ReaderService(builder, service);

            //try
            //{
            //    Console.WriteLine(" -----------Authentication ------------------");
            //    bool login = loginService.Login();
            //    if (!login)
            //        Console.WriteLine("Authentication failed, please check your username and password");
            //}
            //catch (NetworkConnectionException ex) { Console.WriteLine(ex.Message); }
            //catch (AuthTokenException ex) { Console.WriteLine("Error retrieving authorization token"); }
            //catch (GoogleResponseException ex) { Console.WriteLine(String.Format("There was a problem with the connection: {0}, {1}", ex.StatusCode, ex.Message)); }

            //bool doAgain;
            //do
            //{
            //    doAgain = false;
            //    try
            //    {
            //        Console.WriteLine(" ----------- Post list ------------------");
            //        ////foreach (FeedItem item in rdr.GetFeed("http://feeds.feedburner.com/codeclimber", new ReaderParameters() { Direction=ItemDirection.Descending, MaxItems=20}))
            //        ////foreach (FeedItem item in rdr.GetState(StateType.SharedByFriends, new ReaderParameters() { Direction = ItemDirection.Default, MaxItems = 500, Exclude = { "user/-/state/com.google/read" } }))
            //        foreach (FeedItem item in rdr.GetTag("ALT.net", new ReaderParameters()))
            //        {
            //            Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
            //        }

            //        Console.WriteLine(" ----------- Friend Detail ------------------");

            //        Friend friend = rdr.GetFriend("14290265284323789574");
            //        Console.WriteLine(friend.DisplayName);

            //        Console.WriteLine(" ----------- Friend List ------------------");

            //        foreach (var item in rdr.GetFriends())
            //        {
            //            Console.WriteLine(item.DisplayName);
            //        }

            //        Console.WriteLine(" ----------- Unread Count ------------------");


            //        var unreadInfo = rdr.GetUnreadCount();

            //        Console.WriteLine("New Feeds: " + unreadInfo.Single(u => u.Type == CountType.All).Count);

            //        var sharedList = unreadInfo.SingleOrDefault(u => u.Type == CountType.AllShared);
            //        if (sharedList != null)
            //        {
            //            Console.WriteLine();
            //            Console.WriteLine("Shared by friends: " + sharedList.Count);
            //            foreach (var info in unreadInfo.Where(u => u.Type == CountType.Shared))
            //            {
            //                Console.WriteLine(" - {0} ({1})", rdr.GetFriend(info.UserId).DisplayName, info.Count);
            //            }
            //        }

            //        Console.WriteLine();
            //        Console.WriteLine("Unread count by State");
            //        foreach (var info in unreadInfo.Where(u => u.Type == CountType.State).OrderBy(u => u.Count))
            //        {
            //            Console.WriteLine(" - {0} ({1})", info.Name, info.Count);
            //        }
            //        Console.WriteLine();
            //        Console.WriteLine("Unread count by Label");
            //        foreach (var info in unreadInfo.Where(u => u.Type == CountType.Label).OrderByDescending(u => u.Count))
            //        {
            //            Console.WriteLine(" - {0} ({1})", info.Name, info.Count);
            //        }
            //        Console.WriteLine();
            //        Console.WriteLine("Unread count by Feed");
            //        foreach (var info in unreadInfo.Where(u => u.Type == CountType.Feed).OrderByDescending(u => u.Count))
            //        {
            //            Console.WriteLine(" - {0} ({1})", info.Name, info.Count);
            //        }                    
            //    }
            //    catch (NetworkConnectionException ex) { Console.WriteLine(ex.Message); }
            //    catch (GoogleResponseException ex) { Console.WriteLine(String.Format("There was a problem with the connection: {0}, {1}", ex.StatusCode, ex.Message)); }
            //    catch (LoginFailedException)
            //    {
            //        try
            //        {
            //            Console.WriteLine(" -----------Authentication ------------------");
            //            bool login = loginService.Login();
            //            if (!login)
            //                Console.WriteLine("Authentication failed, please check your username and password");
            //            else
            //                doAgain = true;
            //        }
            //        catch (NetworkConnectionException ex) { Console.WriteLine(ex.Message); }
            //        catch (AuthTokenException ex) { Console.WriteLine("Error retrieving authorization token"); }
            //        catch (GoogleResponseException ex) { Console.WriteLine(String.Format("There was a problem with the connection: {0}, {1}", ex.StatusCode, ex.Message)); }
            //    }
                
            //} while (doAgain);





            Console.WriteLine(" ----------- Post list Async------------------");

            loginService.LoginAsync((loggedIn)=>
                                        {
                                            if (loggedIn)
                                                Console.WriteLine("Login completed");
                                            else
                                                Console.WriteLine(
                                                    "Authentication failed, please check your username and password");
                                        }, (ex) =>
                                              {
                                                  if (ex is NetworkConnectionException)
                                                      Console.WriteLine(ex.Message);
                                                  else if (ex is AuthTokenException)
                                                  {
                                                      Console.WriteLine("Error retrieving authorization token");
                                                  }
                                                  else if (ex is GoogleResponseException)
                                                  {
                                                      
                                                      Console.WriteLine(String.Format("There was a problem with the connection: {0}, {1}", ((GoogleResponseException)ex).StatusCode, ex.Message));
                                                  }
                                              },()=>
                                        {
                                            Console.WriteLine("Type Any Key");
                                        } );

            //rdr.GetFeedAsync("http://feeds.feedburner.com/codeclimber",
            //            new ReaderParameters() { Direction = ItemDirection.Descending, MaxItems = 20 },
            //            delegate(IEnumerable<FeedItem> items)
            //            {
            //                foreach (var item in items)
            //                {
            //                    Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
            //                }

            //                Console.WriteLine("Press [ENTER] to close");
            //            }
            //    );

            //rdr.GetTagAsync("ALT.net", new ReaderParameters() { Direction = ItemDirection.Default, MaxItems = 100 },
            //        delegate(IEnumerable<FeedItem> items)
            //        {
            //            foreach (var item in items)
            //            {
            //                Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
            //            }

            //            Console.WriteLine("Press [ENTER] to close");
            //        },
            //        delegate (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    );

            Console.ReadLine();

        }
    }
}
