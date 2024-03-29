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
<link href="/_layouts/CWDashboards/CAS_CWDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

  <div id="Body" >
    <table border="0" width="100%">
        <tr>
            <td valign="top">
                        <WebPartPages:WebpartZone
                        ID="CWBookGrid" runat="server"
                        Title="CWBook Grid Web Part Zone"
                        FrameType="TitleBarOnly" />
            </td>
        </tr>
    </table>
  </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
<asp:Literal ID="m_PageTitle" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SupplementCWBookDashboard%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
<asp:Literal ID="m_PageTitleInTitleArea" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SupplementCWBookDashboard%>" />
</asp:Content>