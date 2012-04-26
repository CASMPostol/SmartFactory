<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DriverNewForm.aspx.cs" Inherits="CAS.SmartFactory.Shepherd.Dashboards.Layouts.ShepherdDashboards.DriverNewForm" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssuc" TagName="LinksTable" Src="/_controltemplates/LinksTable.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="LinkSection" Src="/_controltemplates/LinkSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ActionBar" Src="/_controltemplates/ActionBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" Src="/_controltemplates/Welcome.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<SharePoint:CssRegistration ID="CssRegistration1" Name="forms.css" runat="server" />
  <SharePoint:CssRegistration ID="CssRegistration2" Name="layouts.css" runat="server" />

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
    <wssuc:InputFormSection ID="InputFormSection1" Title="Name and surename" Description="Please add driver's name and surename"
      runat="server">
      <template_description>Please add driver name and surename</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:TextBox runat="server" ID="m_DriverTitle" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection2" Title="ID number" Description="Please add driver's identity document number"
      runat="server">
      <template_description>Please add driver's identity document number</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:TextBox runat="server" ID="m_DriverIDNumber" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection3" Title="Mobile number" Description="Please add driver's mobile phone number"
      runat="server">
      <template_description>Please add driver's mobile phone number</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:TextBox runat="server" ID="m_DriverMobileNo" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection4" Title="Partner name" Description="Please select partner name"
      runat="server">
      <template_description>Please select partner name</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:DropDownList runat="server" ID="m_PartnerTitle" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:ButtonSection ID="ButtonSection" runat="server" ShowStandardCancelButton="false">
      <template_buttons>
      <asp:PlaceHolder ID="PlaceHolder1" runat="server">                
        <asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="Save" id="m_SaveButton" /> &nbsp;
        <asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="Cancel" id="m_CancelButton"  />
      </asp:PlaceHolder>
      </template_buttons>
    </wssuc:ButtonSection>
  </table>
  <asp:HiddenField ID="m_ItemID" runat="server" />

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Add / edit drivers
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Add / edit drivers
</asp:Content>
