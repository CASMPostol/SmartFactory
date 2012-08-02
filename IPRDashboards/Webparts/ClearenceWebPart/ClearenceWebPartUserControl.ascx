﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
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
    <asp:Table runat="server" ID="m_FilterTable" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" ID="m_GroupPanel" BorderColor="ActiveCaptionText" GroupingText="Group">
                    <asp:Table ID="m_GroupTable" runat="server" CssClass="Table">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="m_SelectGroupLabel" runat="server" Text="Select group:" CssClass="Label"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="m_SelectGroupRadioButtonList" runat="server">
                                    <asp:ListItem Enabled="true" Selected="True" Text="Tobacco" Value="Tobacco"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Selected="false" Text="Dust" Value="Dust"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Selected="false" Text="Waste" Value="Waste"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Selected="false" Text="Cartons" Value="Cartons"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" ID="m_PeriodPanel" BorderColor="ActiveCaptionText" GroupingText="Period">
                    <asp:Table ID="m_PeriodTable" runat="server" CssClass="Table">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="m_StartDateLabel" runat="server" Text="Start date:" CssClass="Label"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <SharePoint:DateTimeControl ID="m_StartDateTimeControl" DateOnly="true" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="m_EndDateLabel" runat="server" Text="End date:" CssClass="Label"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <SharePoint:DateTimeControl ID="m_EndTimeControl1" DateOnly="true" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" ID="m_ProcedurePanel" BorderColor="ActiveCaptionText" GroupingText="Procedure">
                    <asp:Table ID="m_ProcedureTable" runat="server" CssClass="Table">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="m_SelectProcedureLabel" runat="server" Text="Select procedure:" CssClass="Label"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="m_ProcedureRadioButtonList" runat="server">
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
                                <asp:Button runat="server" CssClass="Button" Text="Save" ID="m_SaveButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="Delete" ID="m_DeleteButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="Cancel" ID="m_CancelButton" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button runat="server" CssClass="Button" Text="Clear" ID="m_ClearButton" />
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
                <asp:GridView ID="m_AvailableGridView" runat="server"></asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_AssignedLabel" runat="server" Text="Assigned:" CssClass="Label"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:GridView ID="m_AssignedGridView" runat="server"></asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table> 
</asp:Panel>