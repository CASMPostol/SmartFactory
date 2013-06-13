<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadDescriptionWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart.LoadDescriptionWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_LoadDescriptionPanel" runat="server"> 
        <asp:Table ID="LoadDescriptionList" CssClass="TableInside" runat="server">
          <asp:TableRow>
            <asp:TableCell>
              <asp:GridView ID="m_LoadDescriptionGridView" runat="server" Enabled="true" AutoGenerateSelectButton="true" GridLines="Horizontal" BorderColor="#91cdf2" BorderStyle="None" BorderWidth="1px" CellPadding="6">
                  <HeaderStyle BackColor="#91cdf2" Font-Bold="True" ForeColor="White" />
                  <AlternatingRowStyle BackColor="#E8E8E8" />
                  <RowStyle BackColor="#FFFFFF" BorderColor="#91cdf2" />
              </asp:GridView>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
    <asp:Table CssClass="TableInside" runat="server">
    <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_Label4" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Shipping%>" CssClass="Label" Enabled="false"></asp:Label></asp:TableCell>
            <asp:TableCell>
              <asp:Label ID="m_ShippingLabel" runat="server" CssClass="Label"></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
    </asp:Table>
  <asp:Table ID="_Outside" CssClass="TableInside" runat="server">
    <asp:TableRow>
      <asp:TableCell VerticalAlign="Top">
        <asp:Table ID="_LoadDescriptionManager_CommonPart" CssClass="TableInside" runat="server">
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_LoadDescriptionNumberLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,PODelNo%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:TextBox ID="m_LoadDescriptionNumberTextBox" runat="server" CssClass="TextBoxLD" OnTextChanged="m_LoadDescriptionNumberTextBox_TextChanged"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_PalletTypesLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,PalletType%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:DropDownList ID="m_PalletTypesDropDown" CssClass="DropDownLD" runat="server">
              </asp:DropDownList>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_NumberOfPalletsLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,PalletQty%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:TextBox ID="m_NumberOfPalletsTextBox" runat="server" CssClass="TextBoxLD"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_CommodityLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Commodity%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:DropDownList ID="m_CommodityDropDown" runat="server" CssClass="DropDownLD">
              </asp:DropDownList>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
    </asp:TableCell>
    <asp:TableCell VerticalAlign="Top">
          <asp:Table ID="m_OutboundControlsTable"  CssClass="TableInside" runat="server">
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_GoodsQuantityLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,GoodsQty%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:TextBox ID="m_GoodsQuantityTextBox" runat="server" CssClass="TextBoxLD"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_MarketLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Market%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:DropDownList ID="m_MarketDropDown" runat="server" CssClass="DropDownLD">
              </asp:DropDownList>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_CMRLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,CMRNo%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:TextBox ID="m_CMRTextBox" runat="server" CssClass="TextBoxLD"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell>
              <asp:Label ID="m_InvoiceLabel" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,InvoiceNo%>" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
              <asp:TextBox ID="m_InvoiceTextBox" runat="server" CssClass="TextBoxLD"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
       </asp:Table>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_NewLoadDescriptionButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,AddNew%>" CssClass="Button" />
  <asp:Button ID="m_EditLoadDescriptionButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Edit%>" CssClass="Button" />
  <asp:Button ID="m_SaveLoadDescriptionButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Save%>" CssClass="Button" />
  <asp:Button ID="m_DeleteLoadDescriptionButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Delete%>" CssClass="Button_Alert" OnClientClick="return confirm('Are you sure?');" />
  <asp:Button ID="m_CancelLoadDescriptionButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Cancel%>" CssClass="Button" />
</asp:Panel>