<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="AlertManager.aspx.cs" Inherits="LSKYStreamingManager.Alerts.AlertManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>Alerts</h2>

    <h3>Create new alert</h3>
    <asp:Table ID="tblNewAlert" runat="server" Width="950" CellPadding="4" CssClass="datatable" HorizontalAlign="Center">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableCell>Alert Content</asp:TableCell>
            <asp:TableCell>Display starting</asp:TableCell>
            <asp:TableCell>Display until</asp:TableCell>
            <asp:TableCell>Importance</asp:TableCell> 
            <asp:TableCell></asp:TableCell> 
        </asp:TableHeaderRow>                
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top"><asp:TextBox ID="txtAlertContent" runat="server" TextMode="MultiLine" Height="50" Width="300"></asp:TextBox></asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpYear_From" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="drpMonth_From" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtDay_From" runat="server" Width="25"></asp:TextBox><br />
                <asp:TextBox ID="txtHour_From" runat="server" Width="25"></asp:TextBox>:<asp:TextBox ID="txtMinute_From" runat="server" Width="25"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpYear_To" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="drpMonth_To" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtDay_To" runat="server" Width="25"></asp:TextBox>
                <br />
                <asp:TextBox ID="txtHour_To" runat="server" Width="25"></asp:TextBox>
                <asp:TextBox ID="txtMinute_To" runat="server" Width="25"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpImportance" runat="server">
                    <asp:ListItem Value="0">Normal</asp:ListItem>
                    <asp:ListItem Value="1">High</asp:ListItem>
                </asp:DropDownList> 
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Button ID="btnSaveNewAlert" runat="server" Text="Create alert" OnClick="btnSaveNewAlert_Click" /></asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <h3>Existing alerts</h3>
    <asp:Table ID="tblAlerts" runat="server" Width="950" CellPadding="4" CssClass="datatable" HorizontalAlign="Center">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableCell>Alert Content</asp:TableCell>
            <asp:TableCell>Display starting</asp:TableCell>
            <asp:TableCell>Display until</asp:TableCell>
            <asp:TableCell>Importance</asp:TableCell>  
            <asp:TableCell>Delete</asp:TableCell>
        </asp:TableHeaderRow>                
    </asp:Table>
</asp:Content>
