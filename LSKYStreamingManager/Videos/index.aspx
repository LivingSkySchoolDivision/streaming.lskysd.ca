<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingManager.Videos.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>All videos</h2>
    <a href="NewVideo.aspx">Create new video entry</a><br /><br />
    <asp:Table ID="tblVideos" runat="server" CssClass="datatable" CellPadding="5" Width="100%">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableHeaderCell>View</asp:TableHeaderCell>
            <asp:TableHeaderCell>Edit</asp:TableHeaderCell>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Category</asp:TableHeaderCell>
            <asp:TableHeaderCell>Dimensions</asp:TableHeaderCell>
            <asp:TableHeaderCell>Hidden</asp:TableHeaderCell>
            <asp:TableHeaderCell>Private</asp:TableHeaderCell>     
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>
