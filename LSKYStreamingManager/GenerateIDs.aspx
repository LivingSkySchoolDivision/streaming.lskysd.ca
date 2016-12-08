<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="GenerateIDs.aspx.cs" Inherits="LSKYStreamingManager.GenerateIDs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>Video ID numbers</h1>
    <p>The following are video ID numbers that are not currently in use. Use these if you are manually entering videos into the database.</p>
    <asp:Table ID="tblIDs" runat="server"></asp:Table>
</asp:Content>
