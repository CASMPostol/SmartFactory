<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" MasterPageFile="~masterurl/default.master"
    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage" UICulture="auto" Culture="auto" %>

<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="GENERATOR" content="Microsoft SharePoint" />
    <meta name="ProgId" content="SharePoint.WebPartPage.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="CollaborationServer" content="SharePoint Team Web Site" />
    <link href="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div id="Body">
        <table border="0" width="1100px">
            <tr>
                <td valign="top">
                    <table border="0" width="100%">
                        <tr>
                            <td width="100%" valign="top">
                                <WebPartPages:WebPartZone
                                    ID="Shippings" runat="server"
                                    Title="Shippings Web Part Zone"
                                    FrameType="TitleBarOnly" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table border="0" width="100%">
                        <tr>
                            <td width="50%" valign="top">
                                <WebPartPages:WebPartZone
                                    ID="ShippingManager" runat="server"
                                    Title="Shipping Manager Web Part Zone"
                                    FrameType="TitleBarOnly" />
                            </td>
                            <td width="50%" valign="top">
                                <WebPartPages:WebPartZone
                                    ID="TimeSlot" runat="server"
                                    Title="TimeSlot Manager Web Part Zone"
                                    FrameType="TitleBarOnly" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table border="0" width="100%">
                        <tr>
                            <td width="50%" valign="top">
                                <WebPartPages:WebPartZone
                                    ID="LoadManager" runat="server"
                                    Title="Load Manager Web Part Zone"
                                    FrameType="TitleBarOnly" />
                            </td>
                            <td width="50%" valign="top">
                                <WebPartPages:WebPartZone
                                    ID="TransportResource" runat="server"
                                    Title="Transport Resource Manager Web Part Zone"
                                    FrameType="TitleBarOnly" />
                            </td>
                        </tr>
                    </table>
                        <table border="0" width="100%">
                            <tr>
                                <td width="40%" valign="top">
                                    <WebPartPages:WebPartZone
                                        ID="CommentsWebPart" runat="server"
                                        Title="Comments Web Part Zone"
                                        FrameType="TitleBarOnly" />
                                </td>
                                <td width="60%" valign="top">
                                    <WebPartPages:WebPartZone
                                        ID="CommentsList" runat="server"
                                        Title="Commenst List Web Part Zone"
                                        FrameType="TitleBarOnly" />
                                </td>
                            </tr>
                        </table>
                </td>
        </table>
    </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Supervisor%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Supervisor%>" />
</asp:Content>
