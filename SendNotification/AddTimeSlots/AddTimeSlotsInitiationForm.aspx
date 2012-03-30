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
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" 
    DynamicMasterPageFile="~masterurl/default.master" 
    AutoEventWireup="true" 
    Inherits="CAS.SmartFactory.Shepherd.SendNotification.AddTimeSlots.AddTimeSlotsInitiationForm" 
    CodeBehind="AddTimeSlotsInitiationForm.aspx.cs" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<SharePoint:CssRegistration ID="CssRegistration1" Name="forms.css" runat="server" />
  <SharePoint:CssRegistration ID="CssRegistration2" Name="layouts.css" runat="server" />

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
    <wssuc:InputFormSection ID="InputFormSection1" Title="Select year" Description="Please select a year from which you want to begin timeslots creation"
      runat="server">
      <template_description>Please a select year from which you want to begin timeslots creation</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:DropDownList runat="server" ID="m_Year" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection2" Title="Select month" Description="Please select a month from which you want to begin timeslots creation"
      runat="server">
      <template_description>Please select a month from which you want to begin timeslots creation</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:DropDownList runat="server" ID="m_Month" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection3" Title="Select day" Description="Please select a day from which you want to begin timeslots creation"
      runat="server">
      <template_description>Please select a day from which you want to begin timeslots creation</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:DropDownList runat="server" ID="m_Day" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:InputFormSection ID="InputFormSection4" Title="Duration" Description="Please select for how many weeks you want to create timeslots"
      runat="server">
      <template_description>Please select for how many weeks you want to create timeslots</template_description>
      <template_inputformcontrols>
				<tr valign="top">
					<td class="ms-authoringcontrols" width="10">&#160;</td>
					<td class="ms-authoringcontrols" colspan="2">
					    <asp:DropDownList runat="server" ID="m_Duration" Width="400" /> 						
					</td>
				</tr>
			</template_inputformcontrols>
    </wssuc:InputFormSection>
    <wssuc:ButtonSection ID="ButtonSection" runat="server" ShowStandardCancelButton="false">
      <template_buttons>
      <asp:PlaceHolder ID="PlaceHolder1" runat="server">                
        <asp:Button OnClick="StartWorkflow_Click" class="ms-ButtonHeightWidth" runat="server" Text="Start Workflow" ID="StartWorkFlow" />
        <asp:Button OnClick="Cancel_Click" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" ID="Cancel"  />
      </asp:PlaceHolder>
      </template_buttons>
    </wssuc:ButtonSection>
  </table>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Add TimeSlots
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" runat="server" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea">
    Add TimeSlots
</asp:Content>
