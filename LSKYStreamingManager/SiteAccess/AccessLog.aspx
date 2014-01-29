<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="AccessLog.aspx.cs" Inherits="LSKYStreamingManager.SiteAccess.AccessLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
        <h2>Recent Login Events</h2>
        <p>Most recent 200 events, Newest at top</p>
        <asp:Table ID="tblLogins_All" runat="server" CssClass="datatable" Width="1000" CellPadding="5">
            <asp:TableHeaderRow CssClass="datatable_header">            
                <asp:TableHeaderCell>Type</asp:TableHeaderCell>
                <asp:TableHeaderCell>Time</asp:TableHeaderCell>
                <asp:TableHeaderCell>Username</asp:TableHeaderCell>
                <asp:TableHeaderCell>IP Address</asp:TableHeaderCell>
                <asp:TableHeaderCell>Info</asp:TableHeaderCell>
                <asp:TableHeaderCell>User Agent</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
</asp:Content>
