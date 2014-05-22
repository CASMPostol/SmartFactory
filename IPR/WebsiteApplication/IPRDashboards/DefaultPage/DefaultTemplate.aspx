<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 
         
<%@ Page language="C#" MasterPageFile="~masterurl/default.master" 
         Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage"  %> 
         
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" 
             Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
<link href="/_layouts/IPRDashboards/CAS_IPRDefault.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderMain" runat="server">
  <div id="Body" >
      <h1><asp:Literal runat="server" ID="m_Header" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DefaultHeader%>"/></h1>
    <table style="Width:940px">
      <tr>
        <td style="Width:940px;vertical-align:top" colspan="2" >
            <asp:Literal runat="server" ID="m_MainText" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DefaultMainText%>"/>
            <WebPartPages:WebpartZone
            ID="Top" runat="server"
            Title="1st Main Web Part Zone"
            FrameType="TitleBarOnly"/>
        </td>
      </tr>
      <tr>
        <td style="Width:470px;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Left" runat="server"
            Title="1st Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
        <td style="Width:470px;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Right" runat="server"
            Title="2nd Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
      </tr>
    </table>
  </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
<asp:Literal ID="m_PageTitle" runat="server" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_HomePage%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
<asp:Literal ID="m_PageTitleInTitleArea" runat="server" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_HomePage%>" />
</asp:Content>