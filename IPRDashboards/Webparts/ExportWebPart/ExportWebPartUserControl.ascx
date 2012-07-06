﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportWebPartUserControl.ascx.cs" Inherits="IPRDashboards.Webparts.ExportWebPart.ExportWebPartUserControl" %>

<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/IPRDashboards/CAS_IPRDashboards.css" />

<asp:Panel ID="m_Panel" runat="server">

<asp:Table ID="m_Table" runat="server" CssClass="Table">
<asp:TableRow>
    <asp:TableCell VerticalAlign="Top">
         <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Invoice content" ID="m_InvoicePanel">
            <asp:Table ID="m_InvoiceTable" runat="server" CssClass="Table">
                <asp:TableRow>
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceTitleLabel" runat="server" CssClass="Label" Text="Title" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="m_InvoiceTitleTextBox" runat="server" CssClass="TextBox" /> 
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="EditBatch" ID="m_EditBatchPanel">
                            <asp:Table ID="m_EditBatchTable" runat="server">
                            <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="m_EditBatchLabel" runat="server" CssClass="Label" Text="Edit Batch" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:CheckBox ID="m_EditBatchCheckBox" runat="server" /> 
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="m_BatchLabel" runat="server" CssClass="Label" Text="Batch" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="m_BatchTextBox" runat="server" CssClass="TextBox" /> 
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceQuantityLabel" runat="server" CssClass="Label" Text="Quantity" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="m_InvoiceQuantityTextBox" runat="server" CssClass="TextBox" /> 
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="m_InvoiceButtonsTable" runat="server" CssClass="Table">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button runat="server" CssClass="Button" Text="Add New" ID="m_NewButton" />
                        <asp:Button runat="server" CssClass="Button" Text="Edit" ID="m_EditButton" />
                        <asp:Button runat="server" CssClass="Button" Text="Save" ID="m_SaveButton" />
                        <asp:Button runat="server" CssClass="Button" Text="Delete" ID="m_DeleteButton" />
                        <asp:Button runat="server" CssClass="Button" Text="Cancel" ID="m_CancelButton" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
         </asp:Panel>
    </asp:TableCell>
</asp:TableRow>

</asp:Table>

</asp:Panel>