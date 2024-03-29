﻿using System;
using System.Linq;
using CodeClimber.GoogleReaderConnector;
using CodeClimber.GoogleReaderConnector.Model;
using CodeClimber.GoogleReaderConnector.Parameters;
using CodeClimber.GoogleReaderConnector.Services;
using CodeClimber.GoogleReaderConnector.Exceptions;

namespace CodeClimber.GoogleReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "";
            const string password = "";
            const string clientName = "test";

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
            //string itemId = string.Empty;
            //string feedId = string.Empty;
            //Console.WriteLine(" ----------- Post list ------------------");
            ////foreach (FeedItem item in rdr.GetFeed("http://feeds.feedburner.com/codeclimber", new ReaderParameters() { Direction = ItemDirection.Descending, MaxItems = 20 }))
            //foreach (FeedItem item in rdr.GetTag(ItemTag.ReadingList, new ReaderFeedParameters() { Direction = ItemDirection.Default, MaxItems = 1, Exclude = {ItemTag.Read}}))
            ////foreach (FeedItem item in rdr.GetLabel("ALT.net", new ReaderParameters()))
            //{
            //    itemId = item.Id;
            //    feedId = item.Blog.Id;
            //    Console.WriteLine(" - " + item.Blog.Title + " : " + item.Title + " by " + item.Author + "(" + item.Id +")");
            //}

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

            //Console.WriteLine(" ----------- Setting Tag ------------------");


            ////rdr.AddTagToItem(feedId, itemId, ItemTag.Like);
            //rdr.MarkItemRead(feedId, itemId);

            //Console.Write(" Tag set: [Press Enter]");
            //Console.ReadLine();

            //Console.WriteLine(" ----------- Removing Tag ------------------");
            
            ////rdr.RemoveTagFromItem(feedId, itemId, ItemTag.Like);
            //rdr.KeepItemUnread(feedId, itemId);

            //Console.Write(" Tag Removed: [Press Enter]");
            //Console.ReadLine();
            

            ReaderServiceAsync rdrAsync = new ReaderServiceAsync(builder, service);
            Console.WriteLine(" ----------- Post list Async------------------");

            PerformLogin(rdrAsync, () => TestGetTag(rdrAsync));

            ////TestGetFeed(rdr);
            //TestGetLabel(rdrAsync, loginService);

            Console.ReadLine();

        }

        private static void PerformLogin(ReaderServiceAsync rdr, Action onSuccess)
        {
            rdr.Login(
                loggedIn =>
                    {
                        if (loggedIn)
                        {
                            Console.WriteLine("Login completed");
                            onSuccess();
                        }
                        else
                            Console.WriteLine(
                                "Authentication failed, please check your username and password");
                    },
                    ex =>
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
                           },
                           () => Console.WriteLine("Type Any Key"));
        }

        private static void TestGetFeed(ReaderServiceAsync rdr)
        {
            rdr.GetFeed("http://feeds.feedburner.com/codeclimber",
                             new ReaderFeedParameters { Direction = ItemDirection.Descending, MaxItems = 20 },
                             items =>
                                 {

                                     foreach (var item in items)
                                     {
                                         Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
                                     }

                                     Console.WriteLine("Press [ENTER] to close");
                                 },
                             ex =>
                                 {
                                     if (ex is NetworkConnectionException)
                                     {
                                         Console.WriteLine(ex.Message);
                                     }
                                     else if (ex is GoogleResponseException)
                                     {
                                         Console.WriteLine(
                                             String.Format("There was a problem with the connection: {0}, {1}",
                                                           ((GoogleResponseException) ex).StatusCode, ex.Message));
                                     }
                                 },
                            () => Console.WriteLine("Press [ENTER] to close"));
        }


        private static void TestGetTag(ReaderServiceAsync rdr)
        {
            rdr.GetTag(ItemTag.ReadingList, new ReaderFeedParameters { Direction = ItemDirection.Default, MaxItems = 1 },
                            items =>
                            {
                                string itemId = string.Empty;
                                string feedId = string.Empty;
                                foreach (var item in items)
                                {
                                    itemId = item.Id;
                                    feedId = item.Blog.Id;
                                    Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
                                }

                                Console.WriteLine(" ----------- Setting Tag ------------------");
                                rdr.AddTagToItem(feedId,itemId,ItemTag.Like,
                                    added =>
                                        {
                                            if (added)
                                            {
                                                Console.Write(" Tag set: [Press Enter]");
                                                //Console.ReadLine();

                                                rdr.RemoveTagFromItem(feedId, itemId, ItemTag.Like,
                                                    removed =>
                                                        {
                                                            if(removed)
                                                            {
                                                                Console.Write(" Tag Removed: [Press Enter]");
                                                                Console.ReadLine();
                                                            }
                                                        }
                                                    );
                                            }
                                        });

                            },
                            ex =>
                            {
                                if (ex is LoginFailedException)
                                {
                                    PerformLogin(rdr, () => TestGetLabel(rdr));
                                }
                                else if (ex is NetworkConnectionException)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                else if (ex is GoogleResponseException)
                                {
                                    Console.WriteLine(
                                        String.Format("There was a problem with the connection: {0}, {1}",
                                                      ((GoogleResponseException)ex).StatusCode, ex.Message));
                                }
                            },
                            () => Console.WriteLine("Press [ENTER] to close"));
        }


        private static void TestGetLabel(ReaderServiceAsync rdr)
        {
            rdr.GetLabel("ALT.net", new ReaderFeedParameters { Direction = ItemDirection.Default, MaxItems = 5},
                            items =>
                                {
                                    foreach (var item in items)
                                    {
                                        Console.WriteLine(item.Blog.Title + " : " + item.Title + " by " + item.Author);
                                    }
                                },
                            ex =>
                                {
                                    if (ex is LoginFailedException)
                                    {
                                        PerformLogin(rdr, () => TestGetLabel(rdr));
                                    }
                                    else if (ex is NetworkConnectionException)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    else if (ex is GoogleResponseException)
                                    {
                                        Console.WriteLine(
                                            String.Format("There was a problem with the connection: {0}, {1}",
                                                          ((GoogleResponseException) ex).StatusCode, ex.Message));
                                    }
                                },
                            () => Console.WriteLine("Press [ENTER] to close"));
        }
    }
}
