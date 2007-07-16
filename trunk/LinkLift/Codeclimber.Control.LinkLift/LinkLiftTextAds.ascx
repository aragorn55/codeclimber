<%--
/////////////////////////////////////////////////////
//   LinkLift Control for ASP.NET 
//   (C) Simone Chiaretta, 2007
//   http://codeclimber.net.nz
//
//   This project is licensed under the new BSD license.
//   See the License.txt file for more information.
//////////////////////////////////////////////////////
--%>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkLiftTextAds.ascx.cs" Inherits="CodeClimber.Controls.Linklift.LinkLixtTextAds" %>
<asp:Repeater ID="links" runat="server">
<HeaderTemplate><ul class="linklift"></HeaderTemplate>
<ItemTemplate>
<li><%# Eval("prefix") %> <a href='<%# Eval("url") %>'><%# Eval("text") %></a><%# Eval("postfix")%></li>
</ItemTemplate>
<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>