<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsWebPartUserControl.ascx.cs"
    Inherits="CAS.SmartFactory.Shepherd.Dashboards.CommentsWebPart.CommentsWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_PanelAddEdit" runat="server" BorderColor="ActiveCaptionText" GroupingText="Add/Edit task">
    <asp:Table ID="m_TableMain" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell Width="600px" VerticalAlign="Top">
                <asp:Table ID="m_TableLeft" runat="server" CssClass="TableInside">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="m_TaskLabel" runat="server" CssClass="Label" Text="Operation:" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--<asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="m_TaskTitleTextBox" runat="server" CssClass="TextBoxLong" />
                        </asp:TableCell>
                    </asp:TableRow>--%>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="m_ExternalLabel" runat="server" CssClass="Label" Text="Is external" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="m_ExternalCheckBox" runat="server" Checked="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label runat="server" CssClass="Label" Text="Comment:" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <SharePoint:InputFormTextBox ID="m_ShippingCommentsTextBox" runat="server" RichText="true" RichTextMode="FullHtml" TextMode="MultiLine" Width="600" Rows="10" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Table ID="m_Actions" runat="server" CssClass="Table">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonAddNew" Text="Add comment" CssClass="Button" OnClick="m_ButtonAddNew_Click" />
                        </asp:TableCell>
                        <%--<asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonSave" Text="Save" CssClass="Button" />
                        </asp:TableCell>--%>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
