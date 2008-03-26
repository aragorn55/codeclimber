using System;
using System.Xml;
using CommunityServer.Components;
using CommunityServer.Discussions.Components;
using CommunityServer.Blogs.Components;
using CommunityCredit.CommunityServer2007.SubmissionAddOn.com.community_credit.www;

namespace CommunityCredit.CommunityServer2007.SubmissionAddOn
{
    class CommunityCreditPointSubmission: ICSModule
    {
        private string _affiliateKey = string.Empty;
        private string _affiliateCode = string.Empty;
        private bool _trace = false;
        private static bool traceInitialized = false;

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
                string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                AppPath = AppPath.Substring(6);
                System.Diagnostics.Trace.AutoFlush=true;
                System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(new System.IO.StreamWriter(AppPath + @"\CommCreditSubmissionModule.txt", true)));
                traceInitialized = true;
            }

            csa.PostPostUpdate += new CSPostEventHandler(csa_PostPostUpdate);
        }

        void csa_PostPostUpdate(IContent content, CSPostEventArgs e)
        {
            if (e.State == ObjectState.Create)
            {
                if (e.ApplicationType == ApplicationType.Forum)
                {
                    ForumPost post = (ForumPost)content;
                    
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
                    WeblogPost post = (WeblogPost)content;

                    string url=string.Empty;
					string category = string.Empty;
					string description = string.Empty;
					bool sendRequest=false;


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
							url = parentPost.ViewPostURL+"#"+post.PostID;
							description = post.Subject;
							sendRequest = true;
						}
					}

					string urlComplete = CSContext.Current.HostPath + url;

					if(sendRequest)
						SendNotification(urlComplete, category, description, post.User.CommonNameOrUserName, post.User.Email);

                }
            }
        }


        private string SendNotification(string url, string category, string description, string name, string email)
        {
            if (_trace)
            {
                System.Diagnostics.Trace.WriteLine(" ----------------");
                System.Diagnostics.Trace.WriteLine("urlComplete = " + url);
                System.Diagnostics.Trace.WriteLine("description = " + description);
                System.Diagnostics.Trace.WriteLine("category = " + category);
                System.Diagnostics.Trace.WriteLine("email = " + email);
                System.Diagnostics.Trace.WriteLine("name = " + name);
                System.Diagnostics.Trace.WriteLine(" ----------------");
            }

            AffiliateServices wsCommunityCredit = new AffiliateServices();
            string result=string.Empty;
            try
            {
                result = wsCommunityCredit.AddCommunityCredit(email, string.Empty, name, description, url, category, _affiliateCode, _affiliateKey);
            }
            catch (Exception)
            {
				System.Diagnostics.Trace.WriteLine("Exception raised");
            }
            
            if (_trace)
            {
                System.Diagnostics.Trace.WriteLine("result = " + result);
            }
            
            return result;
        }

        #endregion
    }
}
