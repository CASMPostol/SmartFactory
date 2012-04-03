﻿<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 
         
<%@ Page language="C#" MasterPageFile="~masterurl/default.master" 
         Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage" %> 
         
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" 
             Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
<link href="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

  <div id="Body" >
    <table border="0" width="1100px">
        <tr>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="InboundLoad" runat="server"
                        Title="Inbound Load Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="OutboundLoad" runat="server"
                        Title="Outbound Load Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
        </tr>
        <tr>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="InboundShippings" runat="server"
                        Title="Inbound Shippings Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="OutboundShippings" runat="server"
                        Title="Outbound Shippings Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
        </tr>
    </table>
  </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Coordinator Dashboard
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Coordinator Dashboard
</asp:Content>