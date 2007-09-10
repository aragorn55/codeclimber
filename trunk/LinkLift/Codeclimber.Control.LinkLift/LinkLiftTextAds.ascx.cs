#region License
/////////////////////////////////////////////////////
//   LinkLift Control for ASP.NET 
//   (C) Simone Chiaretta, 2007
//   http://codeclimber.net.nz
//
//   This project is licensed under the new BSD license.
//   See the License.txt file for more information.
//////////////////////////////////////////////////////
#endregion

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml.Serialization;

namespace CodeClimber.Controls.Linklift
{
	public partial class LinkLixtTextAds : System.Web.UI.UserControl
	{
		private const string ADSCACHE = "ADS-CACHE";
		private const string ADSEXPIRATION = "ADS-EXPIRATION";
		
		private int _recheck = 1*60*24;

        /// <summary>
        /// Number of minutes to keep a local version of the XML file.
        /// Default to 1 day.
        /// </summary>
		public int CheckAfter
		{
			get { return _recheck; }
			set { _recheck = value; }
		} 

		private const string URLFORMAT = "http://{0}/external/textlink_data.php5?adspace={1}";

		private int _webRequestTimeout = 7000;

        /// <summary>
        /// Timeout for the request to the remote server (in milliseconds).
        /// Default value 7 seconds.
        /// </summary>
		public int WebRequestTimeout
		{
			get { return _webRequestTimeout; }
			set { _webRequestTimeout = value; }
		}

        private string _domain = "www.linklift.de";

        /// <summary>
        /// Domain of the LinkLift server.
        /// Default value is www.linklift.de.
        /// </summary>
		public string Domain
		{
			get { return _domain; }
			set { _domain = value; }
		}

		private string _fileName;

        /// <summary>
        /// Path of the local cache of the Ad definition file.
        /// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		private string _adspace;

        /// <summary>
        /// Unique Identifier of the Advertiser.
        /// (The one provided by LinkLift)
        /// </summary>
		public string Adspace
		{
			get { return _adspace; }
			set { _adspace = value; }
		}

		private string FullPath
		{
			get { return Server.MapPath(VirtualPathUtility.ToAbsolute(_fileName)); }
		}
	

		protected void Page_Load(object sender, EventArgs e)
		{
		    Ads adsConfig;
            if(String.IsNullOrEmpty(FileName))
            {
                adsConfig = new Ads();
                adsConfig.adspace = new TextLink[1];
                adsConfig.adspace[0] = new TextLink();
                adsConfig.adspace[0].text = "FileName is missing";
                adsConfig.adspace[0].prefix = "Error:";
                adsConfig.adspace[0].url = "#";
            }
		    else
                adsConfig = GetAds();
			if (adsConfig != null && adsConfig.adspace.Length > 0)
			{
				links.DataSource = adsConfig.adspace;
				links.DataBind();
			}
			else
				Visible = false;
		}

		private Ads GetAds()
		{
			DateTime expiration = GetExpirationDate();
			if (expiration < DateTime.Now)
				Cache.Remove(ADSCACHE);

			Ads retVal = Cache.Get(ADSCACHE) as Ads;

			if (retVal == null)
			{
				try
				{
					if (!File.Exists(FullPath) || expiration < DateTime.Now)
						RetrieveAdsFromServer();
					retVal = LoadAdsFromFile();
					Cache.Insert(ADSCACHE, retVal, new System.Web.Caching.CacheDependency(FullPath));
				}
				catch
				{
				}
			}
			return retVal;
		}

		private Ads LoadAdsFromFile()
		{
			FileStream fs = new FileStream(FullPath, FileMode.Open);
			XmlSerializer xs = new XmlSerializer(typeof(Ads));
			Ads retVal = (Ads) xs.Deserialize(fs);
			fs.Close();
			return retVal;
		}

		private void RetrieveAdsFromServer()
		{
			File.WriteAllText(FullPath, DownloadAdvertisementXml(String.Format(URLFORMAT, _domain, _adspace), _webRequestTimeout));
		}

		private DateTime GetExpirationDate()
		{
			object test =  Cache.Get(ADSEXPIRATION);
			DateTime retVal;

			if (test == null)
			{
				FileInfo info = new FileInfo(FullPath);
				if (info.Exists)
				{
					retVal = info.LastWriteTime.AddMinutes(_recheck);
					Cache.Insert(ADSEXPIRATION, retVal, new System.Web.Caching.CacheDependency(FullPath),retVal,System.Web.Caching.Cache.NoSlidingExpiration);
				}
				else
					retVal = DateTime.MinValue;
			}
			else
				retVal = (DateTime)test;
			return retVal;
		}

		private static string DownloadAdvertisementXml(string url, int timeout)
		{
			if (url == null) throw new ArgumentNullException("url");
			if (timeout == 0) throw new ArgumentNullException("timeout");

			try
			{
				WebRequest request = WebRequest.Create(url);
				request.Timeout = timeout;

				using (WebResponse response = request.GetResponse())
				{
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						return reader.ReadToEnd();
					}
				}
			}
			catch
			{

			}

			return string.Empty;
		}
	}
}