<%@ Assembly Name="Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" MasterPageFile="~masterurl/default.master"
    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage" %>

<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <meta name="GENERATOR" content="Microsoft SharePoint" />
    <meta name="ProgId" content="SharePoint.WebPartPage.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="CollaborationServer" content="SharePoint Team Web Site" />
    <link href="/_layouts/CWDashboards/CAS_CWDashboards.css" rel="stylesheet" type="text/css" />
</asp:Content>



<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div id="Body">
        <table border="0" width="100%">
            <tr>
                <td valign="top">
                    <WebPartPages:WebPartZone
                        ID="DisposalRequestLibrary" runat="server"
                        Title="Disposal Request Library Web Part Zone"
                        FrameType="TitleBarOnly" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <WebPartPages:WebPartZone
                        ID="CWBook" runat="server"
                        Title="CWBook Web Part Zone"
                        FrameType="TitleBarOnly" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <WebPartPages:WebPartZone
                        ID="CWBookAll" runat="server"
                        Title="CWBook All Web Part Zone"
                        FrameType="TitleBarOnly" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <WebPartPages:WebPartZone
                        ID="Disposals" runat="server"
                        Title="Disposals Web Part Zone"
                        FrameType="TitleBarOnly" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <WebPartPages:WebPartZone
                        ID="StatementLibrary" runat="server"
                        Title="Statement Library Web Part Zone"
                        FrameType="TitleBarOnly" />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal ID="m_PageTitle" runat="server" Text="Statement" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <asp:Literal ID="m_PageTitleInTitleArea" runat="server" Text="Statement" />
</asp:Content>
