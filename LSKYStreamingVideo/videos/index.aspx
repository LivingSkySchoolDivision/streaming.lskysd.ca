﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.videos.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">    
    <div class="searchBox">
        <div class="searchTitle">Search Videos</div>
        <asp:TextBox CssClass="searchTextBox" ID="txtSearchTerms" runat="server"></asp:TextBox>
        <asp:Button CssClass="searchButton" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    </div>
    <h2 runat="server" id="searchResultsTitle" visible="false">Search Results</h2>
    <div style="margin-left: 10px;">
    <asp:Literal ID="litSearchResults" runat="server" Visible="false"></asp:Literal>
    </div>
    <h2>Video Categories</h2>
    <asp:Literal ID="litCategories" runat="server"></asp:Literal>
    

</asp:Content>
