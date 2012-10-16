<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClearenceWebPartUserControl.ascx.cs" Inherits="CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart.ClearenceWebPartUserControl" %>

<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/IPRDashboards/CAS_IPRDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
    <asp:Table runat="server" ID="m_ClearenceTable" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_ClearenceLabel" runat="server" Text="Clearence:" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="m_ClearenceTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Panel runat="server" ID="m_FiltersPanel" BorderColor="ActiveCaptionText" GroupingText="Filters">
        <asp:Table runat="server" ID="m_FilterTable" CssClass="Table">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel runat="server" ID="m_GroupPanel" BorderColor="ActiveCaptionText" GroupingText="Group">
                                    <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_SelectGroupRadioButtonList" runat="server" >
                                        <asp:ListItem Enabled="true" Selected="True" Text="Tobacco" Value="Tobacco"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="false" Text="Tobacco not allocated" Value="TobaccoNotAllocated"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="false" Text="Dust" Value="Dust"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="false" Text="Waste" Value="Waste"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="false" Text="Cartons" Value="Cartons"></asp:ListItem>
                                    </asp:RadioButtonList>
                    </asp:Panel><br/>
                    <asp:Panel runat="server" ID="m_CurrencyPanel" BorderColor="ActiveCaptionText" GroupingText="Currency" HorizontalAlign="Left">
                                    <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_SelectCurrencyRadioButtonList" runat="server">
                                        <asp:ListItem Enabled="true" Selected="False" Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="True" Text="PLN" Value="PLN"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Selected="False" Text="USD" Value="USD"></asp:ListItem>
                                    </asp:RadioButtonList>
                    </asp:Panel>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel runat="server" ID="m_PeriodPanel" BorderColor="ActiveCaptionText" GroupingText="Period">
                                    <asp:Label ID="m_AllDateLabel" runat="server" Text="All" CssClass="Label" />
                                    <asp:CheckBox ID="m_AllDate" runat="server" /><br />
                                    <asp:Label ID="m_StartDateLabel" runat="server" Text="Start date:" CssClass="Label" />
                                    <SharePoint:DateTimeControl ID="m_StartDateTimeControl" DateOnly="true" runat="server" />
                                    <asp:Label ID="m_EndDateLabel" runat="server" Text="End date:" CssClass="Label"/>
                                    <SharePoint:DateTimeControl ID="m_EndTimeControl1" DateOnly="true" runat="server" />
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Panel runat="server" ID="m_ProcedurePanel" BorderColor="ActiveCaptionText" GroupingText="Procedure">
        <asp:Table ID="m_ProcedureTable" runat="server" CssClass="Table">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_ProcedureRadioButtonList" runat="server">
                        <asp:ListItem Enabled="true" Selected="True" Text="4051" Value="4051"></asp:ListItem>
                        <asp:ListItem Enabled="true" Selected="False" Text="3151" Value="3151"></asp:ListItem>
                    </asp:RadioButtonList>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Table ID="m_ButtonsTable" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Add New" ID="m_NewButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Edit" ID="m_EditButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Save" ID="m_SaveButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Delete" ID="m_DeleteButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Cancel" ID="m_CancelButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="Clearence" ID="m_ClearButton" OnClientClick="return confirm('Na pewno? Późniejsza edycja nie będzie już możliwa');" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="m_OGLTable" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_OGLLabel" runat="server" Text="OGL:" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_OGLDropDownList" runat="server" CssClass="DropDown"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="m_GridViewTable" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_AvailableLabel" runat="server" Text="Available:" CssClass="Label"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:GridView ID="m_AvailableGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" EnableModelValidation="True" 
                    OnRowCancelingEdit="m_AvailableGridView_RowCancelingEdit" 
                    OnRowEditing="m_AvailableGridView_RowEditing" OnRowUpdated="m_AvailableGridView_RowUpdated" 
                    OnRowUpdating="m_AvailableGridView_RowUpdating" OnSorting="m_AvailableGridView_Sorting">
                    <Columns>
                        <asp:CommandField HeaderText="Select all" ShowEditButton="True" ShowSelectButton="True" />
                        <asp:BoundField HeaderText="Document No" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" />
                        <asp:BoundField DataField="DebtDate" HeaderText="Debt date" DataFormatString="{0:d}" SortExpression="DebtDate" ReadOnly="true" />
                        <asp:BoundField DataField="ValidTo" HeaderText="Valid To" DataFormatString="{0:d}" SortExpression="ValidTo" ReadOnly="true" />
                        <asp:BoundField DataField="SKU" HeaderText="SKU" SortExpression="SKU" ReadOnly="true" />
                        <asp:BoundField DataField="Batch" HeaderText="Batch" SortExpression="Batch" ReadOnly="true" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit price" ReadOnly="true" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" ReadOnly="true" SortExpression="Currency" />
                        <asp:TemplateField HeaderText="Quantity">
                            <EditItemTemplate>
                                <asp:TextBox ID="QuantityNewValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="QuantityOldValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" />
                        <asp:BoundField DataField="Created" HeaderText="Created" DataFormatString="{0:d}" ReadOnly="true" />
                        <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                            <EditItemTemplate>
                                <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                </asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_AssignedLabel" runat="server" Text="Assigned:" CssClass="Label"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:GridView ID="m_AssignedGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" EmptyDataText="Not selected" EnableModelValidation="True" OnRowCancelingEdit="m_AssignedGridView_RowCancelingEdit" OnRowEditing="m_AssignedGridView_RowEditing" OnRowUpdated="m_AssignedGridView_RowUpdated" OnRowUpdating="m_AssignedGridView_RowUpdating" OnSorting="m_AssignedGridView_Sorting">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="DocumentNo" HeaderText="Document No" ReadOnly="true" SortExpression="DocumentNo" />
                        <asp:BoundField DataField="DebtDate" DataFormatString="{0:d}" HeaderText="Debt date" ReadOnly="true" SortExpression="DebtDate" />
                        <asp:BoundField DataField="ValidTo" DataFormatString="{0:d}" HeaderText="Valid To" ReadOnly="true" SortExpression="ValidTo" />
                        <asp:BoundField DataField="SKU" HeaderText="SKU" ReadOnly="true" SortExpression="SKU" />
                        <asp:BoundField DataField="Batch" HeaderText="Batch" ReadOnly="true" SortExpression="Batch" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit price" ReadOnly="true" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" ReadOnly="true" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ReadOnly="false" />
                        <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true" SortExpression="Status" />
                        <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Created" ReadOnly="true" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                </asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
