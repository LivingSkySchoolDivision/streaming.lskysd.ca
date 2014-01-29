<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="SessionManager.aspx.cs" Inherits="LSKYStreamingManager.SiteAccess.SessionManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h3>Current Sessions:</h3>
    <asp:Table ID="tsblSessions" runat="server" Width="900">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableHeaderCell>Username</asp:TableHeaderCell>
            <asp:TableHeaderCell>Session Start</asp:TableHeaderCell>
            <asp:TableHeaderCell>Session expires</asp:TableHeaderCell>
            <asp:TableHeaderCell>IP</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>    
</asp:Content>
