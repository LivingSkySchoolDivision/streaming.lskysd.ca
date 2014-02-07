<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingManager.Streams.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>All Scheduled Streams</h2>
    <a href="NewStream.aspx">Create new live broadcast</a><br /><br />
    <asp:Table ID="tblStreams" runat="server" CssClass="datatable" CellPadding="5" Width="100%">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableHeaderCell>Edit</asp:TableHeaderCell>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Location</asp:TableHeaderCell>
            <asp:TableHeaderCell>Expected Start</asp:TableHeaderCell>
            <asp:TableHeaderCell>Hidden</asp:TableHeaderCell>
            <asp:TableHeaderCell>Private</asp:TableHeaderCell>
            <asp:TableHeaderCell>Always Online</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>
