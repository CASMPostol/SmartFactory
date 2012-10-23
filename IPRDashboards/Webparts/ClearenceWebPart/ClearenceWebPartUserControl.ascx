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
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" ID="m_ClearencePanel" BorderColor="ActiveCaptionText" GroupingText="Clearence">
                    <asp:TextBox ID="m_ClearenceTextBox" runat="server" CssClass="TextBox" Enabled="false"></asp:TextBox>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
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
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
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
                <asp:Button runat="server" CssClass="ButtonAlert" Text="Clearence" ID="m_ClearButton" OnClientClick="return confirm('Na pewno? Późniejsza edycja nie będzie już możliwa');" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Panel runat="server" ID="m_FiltersPanel" BorderColor="ActiveCaptionText" GroupingText="Filters">
        <asp:Table runat="server" ID="m_FilterTable" CssClass="Table">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel runat="server" ID="m_GroupPanel" BorderColor="ActiveCaptionText" GroupingText="Group">
                        <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_SelectGroupRadioButtonList" runat="server">
                            <asp:ListItem Enabled="true" Selected="false" Text="Tobacco" Value="Tobacco"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="Tobacco not allocated" Value="TobaccoNotAllocated"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="Dust" Value="Dust"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="Waste" Value="Waste"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="True" Text="Cartons" Value="Cartons"></asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                    <br />
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
                        <asp:CheckBox ID="m_AllDate" Checked="true" runat="server" /><br />
                        <asp:Label ID="m_StartDateLabel" runat="server" Text="Start date:" CssClass="Label" />
                        <SharePoint:DateTimeControl ID="m_StartDateTimeControl" DateOnly="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx"/>
                        <asp:Label ID="m_EndDateLabel" runat="server" Text="End date:" CssClass="Label" />
                        <SharePoint:DateTimeControl ID="m_EndTimeControl1" DateOnly="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx"/>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Table ID="m_GridViewTable" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_AvailablePanel" BorderColor="ActiveCaptionText" GroupingText="Available" HorizontalAlign="Left">
                    <SharePoint:SPGridView ID="m_AvailableGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID"
                        OnSelectedIndexChanging="m_AvailableGridView_SelectedIndexChanging" OnRowUpdating="m_AvailableGridView_RowUpdating" OnRowCancelingEdit="m_AvailableGridView_RowCancelingEdit"
                        AllowFiltering="true" FilterDataFields="DocumentNo,DebtDate,ValidTo,SKU,Batch,UnitPrice,Currency,Quantity,Status,Created" FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'">
                        <Columns>
                            <asp:BoundField HeaderText="Document No" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DebtDate" HeaderText="Debt date" DataFormatString="{0:d}" SortExpression="DebtDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ValidTo" HeaderText="Valid To" DataFormatString="{0:d}" SortExpression="ValidTo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SKU" HeaderText="SKU" SortExpression="SKU" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Batch" HeaderText="Batch" SortExpression="Batch" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitPrice" DataFormatString="{0:F3}" HeaderText="Unit price" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Currency" HeaderText="Currency" ReadOnly="true" SortExpression="Currency" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" ItemStyle-HorizontalAlign="Right">
                                <EditItemTemplate>
                                    <asp:TextBox ID="QuantityNewValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="QuantityOldValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Created" HeaderText="Created" DataFormatString="{0:d}" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="Select all" ShowEditButton="True" ShowSelectButton="True" ItemStyle-HorizontalAlign="Right" EditText="Split" UpdateText="Select" />
                        </Columns>
                    </SharePoint:SPGridView>
                    <SharePoint:SPGridViewPager ID="m_AvailableGridViewPager" GridViewId="m_AvailableGridView" runat="server" />
                    <asp:Label ID="m_AvailableGridViewQuntitySumLabel" CssClass="Label" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_GridViewActionsPanel" BorderColor="ActiveCaptionText" GroupingText="Mange selection list.">
                    <asp:Table runat="server" CssClass="Table" ID="m_ActionsTable">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="Add all" ID="m_GridViewAddAll" ToolTip="Add all items from available list to assigned list" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="Add displayed" ID="m_GridViewAddDisplayed" ToolTip="Add displayed items from available list to assigned list" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="Remove all" ID="m_GridViewRemoveAll" ToolTip="Remove all items from assigned list" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="Remove displayed" ID="m_GridViewRemoveDisplayed" ToolTip="Remove displayed items from assigned list" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_AssignedPanel" BorderColor="ActiveCaptionText" GroupingText="Assigned" HorizontalAlign="Left">
                    <SharePoint:SPGridView ID="m_AssignedGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID"
                        OnSelectedIndexChanging="m_AssignedGridView_SelectedIndexChanging"
                        AllowFiltering="true" FilterDataFields="DocumentNo,DebtDate,ValidTo,SKU,Batch,UnitPrice,Currency,Quantity,Status,Created" FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'">
                        <Columns>
                            <asp:BoundField DataField="DocumentNo" HeaderText="Document No" ReadOnly="true" SortExpression="DocumentNo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DebtDate" DataFormatString="{0:d}" HeaderText="Debt date" ReadOnly="true" SortExpression="DebtDate" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ValidTo" DataFormatString="{0:d}" HeaderText="Valid To" ReadOnly="true" SortExpression="ValidTo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SKU" HeaderText="SKU" ReadOnly="true" SortExpression="SKU" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Batch" HeaderText="Batch" ReadOnly="true" SortExpression="Batch" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="Unit price" DataFormatString="{0:F3}" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Currency" HeaderText="Currency" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:F2}" ReadOnly="false" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true" SortExpression="Status" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Created" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </SharePoint:SPGridView>
                    <SharePoint:SPGridViewPager ID="m_AssignedGridViewPager" GridViewId="m_AssignedGridView" runat="server" />
                    <asp:Label ID="m_AssignedGridViewQuantitySumLabel" CssClass="Label" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
