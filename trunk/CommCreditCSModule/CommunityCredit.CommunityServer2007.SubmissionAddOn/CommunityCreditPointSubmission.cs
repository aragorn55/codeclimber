using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using CommunityCredit.CommunityServer2007.SubmissionAddOn.com.community_credit.www;
using CommunityServer.Blogs.Components;
using CommunityServer.Components;
using CommunityServer.Discussions.Components;

namespace CommunityCredit.CommunityServer2007.SubmissionAddOn
{
    internal class CommunityCreditPointSubmission : ICSModule
    {
        private static bool traceInitialized;
        private string _affiliateCode = string.Empty;
        private string _affiliateKey = string.Empty;
        private bool _trace;

        #region ICSModule Members

        public void Init(CSApplication csa, XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name.Equals("AffiliateKey"))
                {
                    _affiliateKey = child.Attributes["Value"].Value;
                }
                else if (child.Name.Equals("AffiliateCode"))
                {
                    _affiliateCode = child.Attributes["Value"].Value;
                }
                else if (child.Name.Equals("Trace"))
                {
                    _trace = Convert.ToBoolean(child.Attributes["Value"].Value);
                }
            }

            if (_trace && !traceInitialized)
            {
                string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                AppPath = AppPath.Substring(6);
                Trace.AutoFlush = true;
                Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(AppPath + @"\CommCreditSubmissionModule.txt", true)));
                traceInitialized = true;
            }

            csa.PostPostUpdate += csa_PostPostUpdate;
        }

        #endregion

        private void csa_PostPostUpdate(IContent content, CSPostEventArgs e)
        {
            if (e.State == ObjectState.Create)
            {
                if (e.ApplicationType == ApplicationType.Forum)
                {
                    var post = (ForumPost) content;

                    string url;
                    string category;
                    if (post.ParentID != post.PostID)
                    {
                        category = "Discussion";
                        url = ForumUrls.Instance().PostPermaLink(post.ParentID, post.PostID);
                    }
                    else
                    {
                        category = "DiscussionQuestion";
                        url = ForumUrls.Instance().PostPermaLink(post.PostID, post.PostID);
                    }
                    string urlComplete = CSContext.Current.HostPath + url;

                    SendNotification(urlComplete, category, post.Subject, post.User.CommonNameOrUserName, post.User.Email);
                }
                else if (e.ApplicationType == ApplicationType.Weblog)
                {
                    var post = (WeblogPost) content;

                    string url = string.Empty;
                    string category = string.Empty;
                    string description = string.Empty;
                    bool sendRequest = false;


                    if (post.BlogPostType == BlogPostType.Post)
                    {
                        category = "Blog";
                        url = post.ViewPostURL;
                        description = "Blogged about: " + post.Subject;
                        sendRequest = true;
                    }
                    else if (post.BlogPostType == BlogPostType.Comment)
                    {
                        if (post.User.CommonNameOrUserName.ToLower().Equals("anonymous"))
                            sendRequest = false;
                        else
                        {
                            category = "Feedback";
                            WeblogPost parentPost = WeblogPosts.GetPost(post.ParentID, false, true, true);
                            url = parentPost.ViewPostURL + "#" + post.PostID;
                            description = post.Subject;
                            sendRequest = true;
                        }
                    }

                    string urlComplete = CSContext.Current.HostPath + url;

                    if (sendRequest)
                        SendNotification(urlComplete, category, description, post.User.CommonNameOrUserName, post.User.Email);
                }
            }
        }


        private string SendNotification(string url, string category, string description, string name, string email)
        {
            if (_trace)
            {
                Trace.WriteLine(" ----------------");
                Trace.WriteLine("urlComplete = " + url);
                Trace.WriteLine("description = " + description);
                Trace.WriteLine("category = " + category);
                Trace.WriteLine("email = " + email);
                Trace.WriteLine("name = " + name);
                Trace.WriteLine(" ----------------");
            }

            var wsCommunityCredit = new AffiliateServices();
            string result = string.Empty;
            try
            {
                result = wsCommunityCredit.AddCommunityCredit(email, string.Empty, name, description, url, category, _affiliateCode, _affiliateKey);
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception raised");
            }

            if (_trace)
            {
                Trace.WriteLine("result = " + result);
            }

            return result;
        }
    }
}