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
                <asp:Panel runat="server" ID="m_ClearencePanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Clearence%>">
                    <asp:TextBox ID="m_ClearenceTextBox" runat="server" CssClass="TextBox" Enabled="false"></asp:TextBox>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_ProcedurePanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Procedure%>">
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
                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddNew%>" ID="m_NewButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Edit%>" ID="m_EditButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Save%>" ID="m_SaveButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Delete%>" ID="m_DeleteButton" OnClientClick="return confirm('<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DeleteToolTip%>?');" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Cancel%>" ID="m_CancelButton" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button runat="server" CssClass="ButtonAlert" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Clearence%>" ID="m_ClearButton" OnClientClick="return confirm('<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ClearenceToolTip%>');" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Panel runat="server" ID="m_FiltersPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Filters%>">
        <asp:Table runat="server" ID="m_FilterTable" CssClass="Table">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel runat="server" ID="m_GroupPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Group%>">
                        <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_SelectGroupRadioButtonList" runat="server">
                            <asp:ListItem Enabled="true" Selected="false" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Tobacco%>" Value="Tobacco"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_TobaccoNotAllocated%>" Value="TobaccoNotAllocated"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Dust%>" Value="Dust"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="false" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Waste%>" Value="Waste"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="True" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Cartons%>" Value="Cartons"></asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="m_CurrencyPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Currency%>" HorizontalAlign="Left">
                        <asp:RadioButtonList RepeatDirection="Horizontal" ID="m_SelectCurrencyRadioButtonList" runat="server">
                            <asp:ListItem Enabled="true" Selected="False" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_All%>" Value="All"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="True" Text="PLN" Value="PLN"></asp:ListItem>
                            <asp:ListItem Enabled="true" Selected="False" Text="USD" Value="USD"></asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel runat="server" ID="m_PeriodPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Period%>">
                        <asp:Label ID="m_AllDateLabel" runat="server" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_All%>" CssClass="Label" />
                        <asp:CheckBox ID="m_AllDate" Checked="true" runat="server" /><br />
                        <asp:Label ID="m_StartDateLabel" runat="server" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_StartDate%>" CssClass="Label" />
                        <SharePoint:DateTimeControl ID="m_StartDateTimeControl" DateOnly="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx" />
                        <asp:Label ID="m_EndDateLabel" runat="server" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_EndDate%>" CssClass="Label" />
                        <SharePoint:DateTimeControl ID="m_EndTimeControl1" DateOnly="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx" />
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Table ID="m_GridViewTable" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_AvailablePanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Available%>" HorizontalAlign="Left">
                    <SharePoint:SPGridView ID="m_AvailableGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID"
                        OnSelectedIndexChanging="m_AvailableGridView_SelectedIndexChanging" OnRowUpdating="m_AvailableGridView_RowUpdating" OnRowEditing="m_AvailableGridView_RowEditing" 
                        OnRowCancelingEdit="m_AvailableGridView_RowCancelingEdit"
                        AllowFiltering="true" FilterDataFields="DocumentNo,DebtDate,ValidTo,SKU,Batch,UnitPrice,Currency,Quantity,Status,Created" FilteredDataSourcePropertyName="FilterExpression" 
                        FilteredDataSourcePropertyFormat="{1} = '{0}'">
                        <Columns>
                            <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DocumentNo%>" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DebtDate" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DebtDate%>" SortExpression="DebtDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ValidTo" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ValidTo%>" SortExpression="ValidTo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SKU" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_SKU%>" SortExpression="SKU" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Batch" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Batch%>" SortExpression="Batch" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitPrice" DataFormatString="{0:F3}" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_UnitPrice%>" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Currency" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Currency%>" ReadOnly="true" SortExpression="Currency" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_QuantityKG%>" SortExpression="Quantity" ItemStyle-HorizontalAlign="Right">
                                <EditItemTemplate>
                                    <asp:TextBox ID="QuantityNewValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="QuantityOldValue" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Status%>" SortExpression="Status" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Created" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Created%>" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="" ShowEditButton="True" ShowSelectButton="True" ItemStyle-HorizontalAlign="Right" EditText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Split%>" UpdateText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Select%>" />
                        </Columns>
                    </SharePoint:SPGridView>
                    <SharePoint:SPGridViewPager ID="m_AvailableGridViewPager" GridViewId="m_AvailableGridView" runat="server" />
                    <asp:Label ID="m_AvailableGridViewQuntitySumLabel" CssClass="Label" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_GridViewActionsPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ManageList%>">
                    <asp:Table runat="server" CssClass="Table" ID="m_ActionsTable">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddAll%>" ID="m_GridViewAddAll" ToolTip="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddAllToolTip%>" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddDisplayed%>" ID="m_GridViewAddDisplayed" ToolTip="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_AddDisplayedToolTip%>" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_RemoveAll%>" ID="m_GridViewRemoveAll" ToolTip="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_RemoveAllToolTip%>" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="ButtonLong" Text="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_RemoveDisplayed%>" ID="m_GridViewRemoveDisplayed" ToolTip="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_RemoveDisplayedToolTip%>" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="m_AssignedPanel" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Assigned%>" HorizontalAlign="Left">
                    <SharePoint:SPGridView ID="m_AssignedGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID"
                        OnSelectedIndexChanging="m_AssignedGridView_SelectedIndexChanging"
                        AllowFiltering="true" FilterDataFields="DocumentNo,DebtDate,ValidTo,SKU,Batch,UnitPrice,Currency,Quantity,Status,Created" FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'">
                        <Columns>
                            <asp:BoundField DataField="DocumentNo" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DocumentNo%>" ReadOnly="true" SortExpression="DocumentNo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DebtDate" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_DebtDate%>" ReadOnly="true" SortExpression="DebtDate" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ValidTo" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_ValidTo%>" ReadOnly="true" SortExpression="ValidTo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SKU" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_SKU%>" ReadOnly="true" SortExpression="SKU" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Batch" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Batch%>" ReadOnly="true" SortExpression="Batch" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_UnitPrice%>" DataFormatString="{0:F3}" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Currency" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Currency%>" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_QuantityKG%>" DataFormatString="{0:F2}" ReadOnly="false" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Status%>" ReadOnly="true" SortExpression="Status" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Created" HeaderText="<%$Resources:CASSmartFactoryIPR,CAS_ASPX_Created%>" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="" ShowEditButton="false" ShowSelectButton="True" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </SharePoint:SPGridView>
                    <SharePoint:SPGridViewPager ID="m_AssignedGridViewPager" GridViewId="m_AssignedGridView" runat="server" />
                    <asp:Label ID="m_AssignedGridViewQuantitySumLabel" CssClass="Label" runat="server" />
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
