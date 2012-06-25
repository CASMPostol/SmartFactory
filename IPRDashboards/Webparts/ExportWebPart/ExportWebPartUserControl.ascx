<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
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
         <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Invoice" ID="m_InvoicePanel">
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
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceItemNoLabel" runat="server" CssClass="Label" Text="Item no." /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="m_InvoiceItemNoTextBox" runat="server" CssClass="TextBox" /> 
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceProductTypeLabel" runat="server" CssClass="Label" Text="Product type" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:DropDownList ID="m_InvoiceProductTypeDropDown" runat="server" CssClass="DropDown" /> 
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceSKULabel" runat="server" CssClass="Label" Text="SKU" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="m_InvoiceSKUTextBox" runat="server" CssClass="TextBox" /> 
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                       <asp:Label ID="m_InvoiceBatchLabel" runat="server" CssClass="Label" Text="Batch" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="m_InvoiceBatchTextBox" runat="server" CssClass="TextBox" /> 
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
                        <asp:Button runat="server" CssClass="Button" Text="Button1" ID="m_InvoiceButton1" />
                        <asp:Button runat="server" CssClass="Button" Text="Button2" ID="m_InvoiceButton2" />
                        <asp:Button runat="server" CssClass="Button" Text="Button3" ID="m_InvoiceButton3" />
                        <asp:Button runat="server" CssClass="Button" Text="Button4" ID="m_InvoiceButton4" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
         </asp:Panel>
    </asp:TableCell>
    <asp:TableCell VerticalAlign="Top">
        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Batch" ID="m_BatchPanel">
            <asp:Table runat="server" CssClass="Table">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="m_BatchQuantity" Text="Quantity" CssClass="Label"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="m_BatchQuantityTextBox" CssClass="TextBox"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button runat="server" CssClass="Button" Text="Button1" ID="m_BatchButton1" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </asp:TableCell>
</asp:TableRow>

</asp:Table>

</asp:Panel>