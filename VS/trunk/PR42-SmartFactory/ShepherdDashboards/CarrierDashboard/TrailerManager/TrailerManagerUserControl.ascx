<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrailerManagerUserControl.ascx.cs" Inherits="CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TrailerManager.TrailerManagerUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel runat="server" ID="m_Panel" >

<asp:Table ID="m_TrailerManager" runat="server" CssClass="Table">
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="m_TrailerNameLabel" runat="server" CssClass="Label" Text="Registration no." ToolTip="Trailer's registration no."></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="m_TrailerTitle" runat="server" CssClass="TextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="m_CommentsLabel" runat="server" CssClass="Label" Text="Comments" ToolTip="Comment that will allow you to easily identify the trailer"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="m_Comments" runat="server" CssClass="TextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:Button runat="server" CssClass="Button" Text="Add new" ID="m_AddNewButton" />
<asp:Button runat="server" CssClass="Button" Text="Edit" ID="m_EditButton" />
<asp:Button runat="server" CssClass="Button" Text="Save" ID="m_SaveButton" />
<asp:Button runat="server" CssClass="Button" Text="Delete" ID="m_DeleteButton" />
<asp:Button runat="server" CssClass="Button" Text="Cancel" ID="m_CancelButton" />
</asp:Panel>