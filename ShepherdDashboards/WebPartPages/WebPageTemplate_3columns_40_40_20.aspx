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
        <td style="Width:40%;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Main1" runat="server"
            Title="1st Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
        <td style="Width:40%;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Main2" runat="server"
            Title="2nd Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
        <td style="Width:20%;vertical-align:top" >
          <WebPartPages:WebPartZone 
            ID="Right" runat="server" 
            Title="Right Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
      </tr>
    </table>
  </div>

</asp:Content>