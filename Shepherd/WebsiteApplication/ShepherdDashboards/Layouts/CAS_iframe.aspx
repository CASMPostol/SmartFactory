<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages" %>
<%@ Page Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.DatePickerFrame" %> 
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<% SPSite spServer = SPControl.GetContextSite(Context); SPWeb spWeb = SPControl.GetContextWeb(Context); %>
<html dir="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_dir_value%>' EncodeMethod='HtmlEncode'/>">
	<head>
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<SharePoint:CssLink runat="server"/>
	<script type="text/javascript" src="./DatePicker.js"></script>
	<title>Date Picker</title>
    <link rel="stylesheet" type="text/css"href="/_layouts/1033/styles/datepicker.css"/>
	</head>
	<body onload="PositionFrame('DatePickerDiv');" onkeydown="OnKeyDown(event);" style="margin:0;">
	  <SharePoint:SPDatePickerControl id="DatePickerWebCustomControl" runat="server" >
		  </SharePoint:SPDatePickerControl>
	</body>
</html>
