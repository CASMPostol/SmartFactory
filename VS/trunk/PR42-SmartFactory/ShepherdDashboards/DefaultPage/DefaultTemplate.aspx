﻿<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 
         
<%@ Page language="C#" MasterPageFile="~masterurl/default.master" 
         Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage"  %> 
         
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" 
             Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <link href="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderMain" runat="server">
  <div id="Body" >
    <table style="Width:100px">
        <tr>
            <td style="Width:1000px;vertical-align:top">
                <img src="_layouts/images/ShepherdImages/DefaultBanner.jpg" />
            </td>
        </tr>
        <tr>
            <td>
                <h1 class="ms-rteElement-H1B" style="margin-bottom:0px">Welcome to Shepherd!</h1>
                <p>Shepherd is designed to simplify inbound/outbound management in the factory to gain maximum benefits. The main idea for inbound is to allow all vendors to book deliveries by themselves (like cinema tickets booking) and for outbound to allow the factory Export Department to book shipments by themselves. On the other hand, forwarders and security escort providers may see online information about planned shipments and provide truck and security escort information by themselves (all the information is available to execute shipments on time and in a safe way). For the global environment it is important that multilingual support is provided.</p>
                <p>Main Advantages:</p>
                <ul>
                <li>Improvement of inbound/outbound controlling</li>
                <li>Smoothing of inbound and outbound operations</li>
                <li>Improvement of work efficiency (work load of the warehouse will be predictable)</li>
                <li>Improvement of communication flow between inside and outside of the cloud</li>
                <li>Improvement of factory and transport safety</li>
                <li>Better and faster reporting </li>
                </ul>
            </td>
        </tr>
    </table>
    <table style="Width:1000px">
      <tr>
        <td style="Width:1000px;vertical-align:top" colspan="2" >
            <WebPartPages:WebpartZone
            ID="Top" runat="server"
            Title="1st Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
      </tr>
      <tr>
        <td style="Width:500px;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Left" runat="server"
            Title="1st Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
        <td style="Width:500px;vertical-align:top">
            <WebPartPages:WebpartZone
            ID="Right" runat="server"
            Title="2nd Main Web Part Zone"
            FrameType="TitleBarOnly" />
        </td>
      </tr>
    </table>
  </div>

</asp:Content>