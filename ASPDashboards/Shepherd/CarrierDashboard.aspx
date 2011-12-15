<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierDashboard.aspx.cs" Inherits="ASPDashboards.Shepherd.CarrierDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="10" border="0" width="100%">
            <tr>
                <td width="20%" valign="top">
                <!--Alarms and events -->
                    <table cellpadding="0" cellspacing="5" border="0" width="100%">
                        <tr>
                            <td>
                                <span style="font-size: 16px; font-family: Verdana, Arial; font-weight: bold;">Alarms and events</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView3" runat="server">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="80%">
                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                        <tr>
                            <td valign="top" colspan="4">
                                <span style="font-size: 16px; font-family: Verdana, Arial; font-weight: bold;">Shiping Data</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <span style="font-size: 12px; font-family: Verdana, Arial;">Time Slots:</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:GridView ID="GridView17" runat="server">
                                </asp:GridView>
                            </td>
                         </tr>
                        <tr>
                             <td valign="top" colspan="4"><!-- Load Description - pallet types and quantity -->
                                <span style="font-size: 12px; font-family: Verdana, Arial;">Load Description:</span>
                             </td>
                        </tr>
                        <tr>
                             <td valign="top" colspan="4">
                                <asp:GridView ID="GridView2" runat="server">
                                </asp:GridView>
                             </td>
                          </tr>
                        <tr>
                             <td valign="top" colspan="4"><!--DriversList-->
                                <span style="font-size: 12px; font-family: Verdana, Arial;">Drivers</span>
                             </td>
                        </tr>
                        <tr>
                             <td valign="top" colspan="4">
                                <asp:GridView ID="GridView18" runat="server">
                                </asp:GridView>
                             </td>
                          </tr>
                        <tr>
                             <td valign="top" colspan="4"><!--TrucksList-->
                                <span style="font-size: 12px; font-family: Verdana, Arial;">Trucks</span>
                             </td>
                        </tr>
                        <tr>
                             <td valign="top" colspan="4">
                                <asp:GridView ID="GridView6" runat="server">
                                </asp:GridView>
                             </td>
                          </tr>
                        <tr>
                             <td valign="top" colspan="4"><!--TrailersList-->
                                <span style="font-size: 12px; font-family: Verdana, Arial;">Trailers</span>
                             </td>
                        </tr>
                        <tr>
                             <td valign="top" colspan="4">
                                <asp:GridView ID="GridView20" runat="server">
                                </asp:GridView>
                             </td>
                        </tr>
                        <tr><!--Cancel/Save Shipping-->
                            <td colspan="4" align="right" valign="top">
                                <asp:Button ID="Button1" runat="server" Text="Create new shipping" />
                                <asp:Button ID="Button2" runat="server" Text="Cancel shipping operation" />
                                <asp:Button ID="Button3" runat="server" Text="Save changes" />
                            </td>
                        </tr>
                    </table>
                    <!--shipping operaions-->
                    <table cellpadding="0" cellspacing="5" border="0" width="100%">
                        <tr>
                            <td>
                                <span style="font-size: 16px; font-family: Verdana, Arial; font-weight: bold;">Shipping operations:</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView1" runat="server">
                                </asp:GridView>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>    
    </div>
    </form>
</body>
</html>
