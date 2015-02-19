<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" 
    DynamicMasterPageFile="~masterurl/default.master" 
    AutoEventWireup="true" 
    Inherits="CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts.CloseManyAccountsForm" 
    CodeBehind="CloseManyAccountsForm.aspx.cs" %>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
    <wssuc:InputFormSection ID="AvailableAccounts" Title="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AvailableAccounts%>" runat="server">
      <template_description><asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AvailableAccounts%>" /></template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <!-- SPDataGrid with CW all accounts -->
                        <SharePoint:SPGridView ID="m_AvailableGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" AllowFiltering="true" 
                            FilterDataFields="Title,CustomsDebtDate,DocumentNo,Grade,SKU,Batch,NetMass,AccountBalance,ValidToDate,ClosingDate">
                            <Columns>
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Title%>" DataField="Title" SortExpression="Title" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CustomsDebtDate%>" DataField="CustomsDebtDate" SortExpression="CustomsDebtDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_DocumentNo%>" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Grade%>" DataField="Grade" SortExpression="Grade" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SKU%>" DataField="SKU" SortExpression="SKU" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Batch%>" DataField="Batch" SortExpression="Batch" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_NetMass%>" DataField="NetMass" SortExpression="NetMass" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AccountBalance%>" DataField="AccountBalance" SortExpression="AccountBalance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ValidToDate%>" DataField="ValidToDate" SortExpression="ValidToDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ClosingDate%>" DataField="ClosingDate" SortExpression="ClosingDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                <asp:CommandField HeaderText="" ShowEditButton="True" ShowSelectButton="True" ItemStyle-HorizontalAlign="Right" UpdateText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Select%>" />
                            </Columns>
                        </SharePoint:SPGridView>
                        <SharePoint:SPGridViewPager ID="m_AvailableGridViewPager" GridViewId="m_AvailableGridView" runat="server" />				
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="SelectedAccounts" Title="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SelectedAccounts%>" runat="server">
      <template_description><asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SelectedAccounts%>" /></template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					     	<!--Selected accounts grid view -->
                        <SharePoint:SPGridView ID="m_SelectedGridView" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" AllowFiltering="true" 
                            FilterDataFields="CustomsDebtDate,DocumentNo,Grade,SKU,Batch,NetMass,AccountBalance,ValidToDate,ClosingDate">
                            <Columns>
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Title%>" DataField="Title" SortExpression="Title" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CustomsDebtDate%>" DataField="CustomsDebtDate" SortExpression="CustomsDebtDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_DocumentNo%>" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Grade%>" DataField="Grade" SortExpression="Grade" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SKU%>" DataField="SKU" SortExpression="SKU" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Batch%>" DataField="Batch" SortExpression="Batch" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_NetMass%>" DataField="NetMass" SortExpression="NetMass" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AccountBalance%>" DataField="AccountBalance" SortExpression="AccountBalance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ValidToDate%>" DataField="ValidToDate" SortExpression="ValidToDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ClosingDate%>" DataField="ClosingDate" SortExpression="ClosingDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:Label ID="IDEditLabel" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="IDItemLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                <asp:CommandField HeaderText="" ShowEditButton="True" ShowSelectButton="True" ItemStyle-HorizontalAlign="Right" UpdateText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Select%>" />
                            </Columns>
                        </SharePoint:SPGridView>
                        <SharePoint:SPGridViewPager ID="m_SelectedGridViewPager" GridViewId="m_SelectedGridView" runat="server" />					
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:ButtonSection ID="ButtonSection" runat="server" ShowStandardCancelButton="false">
      <template_buttons>
      <asp:PlaceHolder ID="PlaceHolder1" runat="server">                
        <asp:Button OnClick="StartWorkflow_Click" class="ms-ButtonHeightWidth" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CloseAccountsButton%>" ID="StartWorkflow" />
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button OnClick="Cancel_Click" class="ms-ButtonHeightWidth" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CancelButton%>" ID="Cancel"  />
      </asp:PlaceHolder>
      </template_buttons>
    </wssuc:ButtonSection>
  </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Workflow Initiation Form
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" runat="server" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea">
    Workflow Initiation Form
</asp:Content>
