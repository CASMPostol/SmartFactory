<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportWebPartUserControl.ascx.cs"
    Inherits="CAS.SmartFactory.IPR.Dashboards.Webparts.ExportWebPart.ExportWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/IPRDashboards/CAS_IPRDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
    <asp:Table ID="m_Table" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_EditInvoiceQuantity%>"
                    ID="m_InvoicePanel">
                    <asp:Table ID="m_InvoiceTable" runat="server" CssClass="Table">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Invoice%>"
                                    ID="m_EditInvoicePanel">
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label ID="m_InvoiceLabel" runat="server" CssClass="Label" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Invoice%>" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="m_InvoiceTextBox" runat="server" CssClass="TextBox" Enabled="false" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label ID="m_InvoiceContentLabel" runat="server" CssClass="Label" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_InvoiceItem%>" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="m_InvoiceContentTextBox" runat="server" CssClass="TextBox" Enabled="false" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ExportProcedure%>"
                                    ID="m_ExportProcedurePanel">
                                        <asp:Table ID="m_ExportProcedureTable" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:RadioButtonList ID="m_ExportProcedureRadioButtonList" runat="server">
                                                        <asp:ListItem Enabled="true" Selected="True" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Export%> " Value="Export"></asp:ListItem>
                                                        <asp:ListItem Enabled="true" Selected="False" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_RevertToFC%>" Value="Revert"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_EditBatch%>"
                                    ID="m_EditBatchPanel">
                                    <asp:Table ID="m_EditBatchTable" runat="server">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="m_EditBatchLabel" runat="server" CssClass="Label" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ChangeBatch%>" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:CheckBox ID="m_EditBatchCheckBox" runat="server" AutoPostBack="true" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="m_BatchLabel" runat="server" CssClass="Label" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Batch%>" />
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="m_BatchTextBox" runat="server" CssClass="TextBox" Enabled="false" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="m_InvoiceQuantityLabel" runat="server" CssClass="Label" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Quantity%>" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="m_InvoiceQuantityTextBox" runat="server" CssClass="TextBox" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Table ID="m_InvoiceButtonsTable" runat="server" CssClass="Table">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddNew%>" ID="m_NewButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Edit%>" ID="m_EditButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Save%>" ID="m_SaveButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Delete%>" ID="m_DeleteButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Cancel%>" ID="m_CancelButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Export%>" ID="m_ExportButton" ToolTip="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ExportToolTip%>"  />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
