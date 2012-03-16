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
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

  <div id="Body" >
    <table border="0" width="1200">
        <tr>
            <td valign="top">
                <table border="0" width="1000px">
                    <tr>
                        <td width="1000px" valign="top">
                        <WebPartPages:WebpartZone
                        ID="Warehouse" runat="server"
                        Title="Warehouse Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                    </tr>
                </table>
                <table border="0" width="1000px">
                    <tr>
                        <td width="1000px" valign="top">
                        <WebPartPages:WebpartZone
                        ID="Shippings" runat="server"
                        Title="Shippings Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                    </tr>
                </table>
                <table border="0" width="1000px">
                    <tr>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="ShippingManager" runat="server"
                        Title="Shipping Manager Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="TimeSlot" runat="server"
                        Title="TimeSlot Manager Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                    </tr>
                    <tr>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="LoadManager" runat="server"
                        Title="Load Manager Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                        <td width="50%" valign="top">
                        <WebPartPages:WebpartZone
                        ID="TransportResource" runat="server"
                        Title="Transport Resource Manager Web Part Zone"
                        FrameType="TitleBarOnly" />
                        </td>
                    </tr>
                </table>
        </td>
        <td valign="top" width="200px" >
            <WebPartPages:WebpartZone
            ID="AlarmsAndEvents" runat="server"
            Title="AlarmsAndEvents Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
    </table>
  </div>

</asp:Content>