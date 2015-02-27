<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Register TagPrefix="wssuc" TagName="LinksTable" Src="/_controltemplates/LinksTable.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="LinkSection" Src="/_controltemplates/LinkSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ActionBar" Src="/_controltemplates/ActionBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" Src="/_controltemplates/Welcome.ascx" %>

<%@ Page Language="C#" 
    DynamicMasterPageFile="~masterurl/default.master" 
    AutoEventWireup="true" 
    Inherits="CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts.CloseManyAccountsForm" 
    CodeBehind="CloseManyAccountsForm.aspx.cs" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<SharePoint:CssRegistration ID="CssRegistration1" Name="forms.css" runat="server" />
<SharePoint:CssRegistration ID="CssRegistration2" Name="layouts.css" runat="server" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
    <wssuc:InputFormSection ID="AvailableAccounts" Title="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AvailableAccounts%>" runat="server">
      <template_description><asp:Literal runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AvailableAccounts%>" /></template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <!-- SPDataGrid with CW all accounts -->
                        <SharePoint:SPGridView ID="m_AvailableGridView" runat="server" AllowSorting="False" AutoGenerateColumns="False" DataKeyNames="ID" AllowFiltering="False">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Select%>" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="x_IsSelected" runat="server" Checked="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Title%>" DataField="Title" SortExpression="Title" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CustomsDebtDate%>" DataField="CustomsDebtDate" SortExpression="CustomsDebtDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_DocumentNo%>" DataField="DocumentNo" SortExpression="DocumentNo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Grade%>" DataField="Grade" SortExpression="Grade" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_SKU%>" DataField="SKU" SortExpression="SKU" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_Batch%>" DataField="Batch" SortExpression="Batch" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_NetMass%>" DataField="NetMass" SortExpression="NetMass" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_AccountBalance%>" DataField="AccountBalance" SortExpression="AccountBalance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ValidToDate%>" DataField="ValidToDate" DataFormatString="{0:d}" SortExpression="ValidToDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="<%$Resources:CASSmartFactoryCW,CAS_ASPX_ClosingDate%>" DataField="ClosingDate" DataFormatString="{0:d}" SortExpression="ClosingDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Id" DataField="Id" Visible="False" SortExpression="Id" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                        </SharePoint:SPGridView>			
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
    <asp:Literal ID="m_PageTitle" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CloseManyAccountsForm%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" runat="server" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea">
    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:CASSmartFactoryCW,CAS_ASPX_CloseManyAccountsForm%>" />
</asp:Content>
