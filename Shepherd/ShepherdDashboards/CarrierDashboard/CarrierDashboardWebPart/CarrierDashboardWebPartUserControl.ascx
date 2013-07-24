<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarrierDashboardWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart.CarrierDashboardWebPartUserControl" %>

<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
  <asp:Table ID="m_CarrierDashboardWebPart" runat="server" CssClass="Table">
    <asp:TableRow>
      <asp:TableCell>
        <asp:Literal ID="m_StateLiteral" runat="server" />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_WarehousehHeaderLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Warehouse%>" CssClass="Label"
          ToolTip="<%$Resources:CASSmartFactoryShepherd,WarehouseToolTip%>">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_WarehouseLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TimeSlotLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,TimeSlot%>" CssClass="Label"
         ToolTip="<%$Resources:CASSmartFactoryShepherd,TimeSlotToolTip%>"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_TimeSlotTextBox" runat="server" CssClass="Label" Enabled="false"></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_DocumentLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,PONumber%>" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_DocumentTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_CommentsLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Comments%>" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_CommentsTextBox" runat="server" TextMode="Multiline" Height="60" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_EstimateDeliveryTimeLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,ETA%>" CssClass="Label"
          ToolTip="<%$Resources:CASSmartFactoryShepherd,ETAToolTip%>"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <SharePoint:DateTimeControl HoursMode24="true" ID="m_EstimateDeliveryTimeDateTimeControl" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx"
             OnDateChanged="m_EstimateDeliveryTimeDateTimeControl_DateChanged" AutoPostBack="true" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
          <asp:TableCell ColumnSpan="2">
              <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryShepherd,LoadingUnloadingTime%>" ID="m_LoadingUnloadingTime">
                  <asp:Table runat="server" >
                      <asp:TableRow ID="m_WarehouseStartTimeRow">  
                          <asp:TableCell>
                            <asp:Label ID="m_WarehouseStartTimeLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,WarehouseStartTime%>" CssClass="Label"></asp:Label>
                          </asp:TableCell>
                          <asp:TableCell ColumnSpan="2">
                            <SharePoint:DateTimeControl ID="m_WarehouseStartTimeControl" HoursMode24="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx"
                                OnDateChanged="m_WarehouseStartTimeControl_DateChanged", AutoPostBack="true"
                                ToolTip="<%$Resources:CASSmartFactoryShepherd,WarehouseStartTimeToolTip%>" />
                          </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="m_WarehouseEndTimeRow">  
                          <asp:TableCell>
                            <asp:Label ID="m_WarehouseEndTimeLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,WarehouseEndTime%>" CssClass="Label"></asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <SharePoint:DateTimeControl ID="m_WarehouseEndTimeControl" HoursMode24="true" runat="server" DatePickerFrameUrl="/_layouts/CAS_iframe.aspx" 
                                OnDateChanged="m_WarehouseEndTimeControl_DateChanged" AutoPostBack="true"
                                ToolTip="<%$Resources:CASSmartFactoryShepherd,WarehouseEndTimeToolTip%>" />
                          </asp:TableCell>
                          <asp:TableCell HorizontalAlign="Right">
                              <asp:Button ID="m_WarehouseEndTimeButton" runat="server" CssClass="Button" Text="<%$Resources:CASSmartFactoryShepherd,End%>"  OnClick="m_WarehouseEndTimeButton_Click"
                                  ToolTip="<%$Resources:CASSmartFactoryShepherd,EndToolTip%>" />
                          </asp:TableCell>
                      </asp:TableRow>
                  </asp:Table>
                  </asp:Panel>
          </asp:TableCell>
      </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_CityHeaderLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Destination%>" CssClass="Label">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_CityLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,SelectCity%>"
          CssClass="_Label"></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_PartnerHeaderLabel" runat="server" Text="Vendor" CssClass="Label">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_PartnerLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,SelectVendor%>"
          CssClass="_Label">
        </asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell ColumnSpan="2">
        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="<%$Resources:CASSmartFactoryShepherd,RouteEscortEditing%>"
         ToolTip="<%$Resources:CASSmartFactoryShepherd,RouteEscortEditingToolTip%>" ID="m_CoordinatorPanel">
          <asp:Table runat="server">
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="Edit" CssClass="Label" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,EditRouteEscort%>" ></asp:Label>
                <asp:CheckBox runat="server" TextAlign="Left" ToolTip="<%$Resources:CASSmartFactoryShepherd,EditRouteEscortToolTip%>" AutoPostBack="True" ID="m_CoordinatorEditCheckBox" />
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="SPAN" runat="server" CssClass="Label" Text="|"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_SecurityRequiredLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,SecurityRequired%>" CssClass="Label"></asp:Label>
                <asp:CheckBox ID="m_SecurityRequiredChecbox" AutoPostBack="true" runat="server" Checked="false"></asp:CheckBox>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
          <asp:Table runat="server">
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="m_RouteHeaderLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Route%>" CssClass="Label"></asp:Label></asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_RouteLabel" runat="server" Text="" CssClass="Label"></asp:Label></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="m_SecurityEscortHeaderLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,SecurityEscort%>" CssClass="Label"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_SecurityEscortLabel" runat="server" Text="" CssClass="Label"></asp:Label>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
        </asp:Panel>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TransportUnitTypeLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,TransportUnitType%>" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:DropDownList ID="m_TransportUnitTypeDropDownList" runat="server" CssClass="DropDown" />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="<%$Resources:CASSmartFactoryShepherd,DockNumber%>" ID="m_DocNumberLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_DockNumberTextBox" CssClass="TextBox" runat="server"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="<%$Resources:CASSmartFactoryShepherd,TrailerCondition%>" ID="m_TrailerConditionDropdownLabel" runat="server"
          CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:DropDownList ID="m_TrailerConditionDropdown" runat="server" CssClass="DropDown">
        </asp:DropDownList>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="<%$Resources:CASSmartFactoryShepherd,TrailerComments%>" ID="m_TrailerConditionCommentsLabel" ToolTip="<%$Resources:CASSmartFactoryShepherd,TrailerCommentsToolTip%>" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_TrailerConditionCommentsTextBox" TextMode="Multiline" Height="60" CssClass="TextBox" runat="server"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_ContainerNoLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,ContainerNumber%>" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_ContainerNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_NewShippingButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,AddNew%>" CssClass="Button" />
  <asp:Button ID="m_EditButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Edit%>" CssClass="Button" />
  <asp:Button ID="m_AbortButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Abort%>" CssClass="Button_Alert" OnClientClick="return confirm('Are you sure?');" />
  <asp:Button ID="m_SaveButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Save%>" CssClass="Button" />
  <asp:Button ID="m_CancelButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Cancel%>" CssClass="Button" />
</asp:Panel>
