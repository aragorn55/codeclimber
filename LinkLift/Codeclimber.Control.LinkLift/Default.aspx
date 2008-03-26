<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CodeClimber.Controls.Linklift.Default" %>
<%@ Register TagPrefix="uc1" TagName="LinkText" Src="~/LinkLiftTextAds.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
	<link href="LinkLiftTextAds.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <uc1:LinkText id="ll" runat="server"
	Domain="www.linklift.de"
	FileName="~/LL_496fId26b30.xml"
	Adspace="69d64bf23I0"
	CheckAfter="5" />
    </div>
    </form>
</body>
</html>
