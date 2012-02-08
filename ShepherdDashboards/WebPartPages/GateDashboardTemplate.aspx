<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 
         
<%@ Page language="C#" MasterPageFile="~masterurl/default.master" 
         Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage"  %> 
         
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" 
             Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <link href="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderMain" runat="server">

  <div id="Body" >
    <table style="Width:100%" >
      <tr>
        <td style="Width:30%;vertical-align:top" >
          <WebPartPages:WebPartZone 
            ID="Left" runat="server" 
            Title="Left Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
        <td style="Width:70%;vertical-align:top">
            <table style="Width:100%" >
                <tr>
                    <td colspan="2">
                        <WebPartPages:WebPartZone
                        ID="SelectedShippingZone" runat="server"
                        Title="Selected Shipping Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <WebPartPages:WebPartZone
                        ID="DriversTeam" runat="server"
                        Title="Drivers Team Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                    <td>
                        <WebPartPages:WebPartZone
                        ID="GateWebPart" runat="server"
                        Title="Gate Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <WebPartPages:WebPartZone
                        ID="InboundZone" runat="server"
                        Title="Inbound Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                    <td>
                        <WebPartPages:WebPartZone
                        ID="OutboundZone" runat="server"
                        Title="Outbound Web Part Zone"
                        FrameType="TitleBarOnly" />
                    </td>
                </tr>
            </table>
        </td>
      </tr>
    </table>
  </div>

</asp:Content>