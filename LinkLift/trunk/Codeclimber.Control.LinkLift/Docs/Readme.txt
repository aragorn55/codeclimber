This is LinkLift control for ASP.NET, to include link text ads into your site.

How to use it:

1 - copy the files into your web application. The zip file contains 3 files:

    * LinkLift.dll, this must be copied to the bin folder of your web application
    * LinkLiftTextAds.ascx, this is the web user control, and must copied inside your website
    * LinkLiftTextAds.css, the stylesheet with the CSS styles for the ads.

2 - Register the control on the page:
	<%@ Register TagPrefix="uc1" TagName="LinkText" Src="Controls/LinkLiftTextAds.ascx" %>

3 - Add the control inside the page:

<uc1:LinkText id="ll" runat="server"
  Domain="www.linklift.de"
  FileName="~/LL_<temporaryfilename>.xml"
  Adspace="<yourAdspaceId>"
  CheckAfter="1440" />

There a few properties you can set:

    * Domain: is the host name of the server you want to get the text links from
    * Filename: is the name of the file that will be created on your server to locally cache the link definition
    * Adspace: your unique identifier, the one retrieved during the registration
    * CheckAfter: duration of the local cache specified in minutes. After this time has passed the control will update the text link definition retrieving a new file from the server
    * WebRequestTimeout: timeout in milliseconds for the request to the server (not in the snippet above, and defaults to 7sec).

Instructions on how to use it inside SubText: http://codeclimber.net.nz/archive/2007/07/08/LinkLift-control-for-ASP.NET.aspx

For more informations on the control please visit my blog: http://codeclimber.net.nz