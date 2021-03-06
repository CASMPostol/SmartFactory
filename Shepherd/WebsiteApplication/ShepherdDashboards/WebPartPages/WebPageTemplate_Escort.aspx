<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 
         
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
<link href="/_layouts/ShepherdDashboards/CAS_ShepherdLayouts.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

  <div id="Body" >
    <table border="0" width="1100px">
        <tr>
            <td valign="top">
            <table border="0" width="100%">
                <tr>
                    <td width="100%" valign="top">
                    <WebPartPages:WebpartZone
                    ID="Shippings" runat="server"
                    Title="Shippings Web Part Zone"
                    FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
            <hr />
            <table border="0" width="100%">
                <tr>
                    <td width="100%" valign="top">
                    <WebPartPages:WebpartZone
                    ID="Route" runat="server"
                    Title="Route Web Part Zone"
                    FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
            <hr />
            <table border="0" width="100%">
                <tr>
                    <td width="100%" valign="top">
                    <WebPartPages:WebpartZone
                    ID="Escort" runat="server"
                    Title="Escort Web Part Zone"
                    FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
            <hr />
            <table border="0" width="100%">
                <tr>
                    <td width="50%" valign="top">
                    <WebPartPages:WebpartZone
                    ID="TransportResource" runat="server"
                    Title="Transport Resource Web Part Zone"
                    FrameType="TitleBarOnly" />
                    </td>
                    <td width="50%" valign="top">
                    <WebPartPages:WebpartZone
                    ID="Empty" runat="server"
                    Title="Empty Web Part Zone"
                    FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
            <hr />
            <table border="0" width="100%">
                <tr>
                    <td valign="top" width="1200px">
                        <WebPartPages:WebpartZone
                        ID="AlarmsAndEvents" runat="server"
                        Title="AlarmsAndEvents Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
        </td>
    </table>
  </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
<asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Escort%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
<asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Escort%>" />
</asp:Content>