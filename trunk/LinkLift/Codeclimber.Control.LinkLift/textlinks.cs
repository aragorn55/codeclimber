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

namespace CodeClimber.Controls.Linklift
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
	[System.SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", ElementName = "ll_data", IsNullable = false)]
	public partial class Ads
	{

		private TextLink[] adspaceField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("link", IsNullable = false)]
		public TextLink[] adspace
		{
			get
			{
				return this.adspaceField;
			}
			set
			{
				this.adspaceField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class TextLink
	{

		private string urlField;

		private string textField;

		private string prefixField;

		private string postfixField;

		private string rss_urlField;

		private string rss_textField;

		private string rss_prefixField;

		private string rss_postfixField;

		private byte nofollowField;

		/// <remarks/>
		public string url
		{
			get
			{
				return this.urlField;
			}
			set
			{
				this.urlField = value;
			}
		}

		/// <remarks/>
		public string text
		{
			get
			{
				return this.textField;
			}
			set
			{
				this.textField = value;
			}
		}

		/// <remarks/>
		public string prefix
		{
			get
			{
				return this.prefixField;
			}
			set
			{
				this.prefixField = value;
			}
		}

		/// <remarks/>
		public string postfix
		{
			get
			{
				return this.postfixField;
			}
			set
			{
				this.postfixField = value;
			}
		}

		/// <remarks/>
		public string rss_url
		{
			get
			{
				return this.rss_urlField;
			}
			set
			{
				this.rss_urlField = value;
			}
		}

		/// <remarks/>
		public string rss_text
		{
			get
			{
				return this.rss_textField;
			}
			set
			{
				this.rss_textField = value;
			}
		}

		/// <remarks/>
		public string rss_prefix
		{
			get
			{
				return this.rss_prefixField;
			}
			set
			{
				this.rss_prefixField = value;
			}
		}

		/// <remarks/>
		public string rss_postfix
		{
			get
			{
				return this.rss_postfixField;
			}
			set
			{
				this.rss_postfixField = value;
			}
		}

		/// <remarks/>
		public byte nofollow
		{
			get
			{
				return this.nofollowField;
			}
			set
			{
				this.nofollowField = value;
			}
		}
	}
}